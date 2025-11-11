using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Cau4
{
    public partial class Server : Form
    {

        private Dictionary<string, List<string>> soldTickets = new Dictionary<string, List<string>>();
        private readonly object _lock = new object(); 

        public Server()
        {
            InitializeComponent();
            cbb_Movies.Items.AddRange(movies.Select(a => a.Name).ToArray());
            cbb_Rooms.Items.AddRange(movies.SelectMany(a => a.Rooms)
                                           .Distinct()
                                           .Select(i => (object)i)
                                           .ToArray());
            cbb_Movies.SelectedIndex = 0;
            cbb_Rooms.SelectedIndex = 0;
        }

        public class Movie
        {
            public String Name { get; set; }
            public int Price { get; set; }
            public List<int> Rooms { get; set; } = new List<int>();
        }

        protected List<Movie> movies = new List<Movie>()
        {
            new Movie { Name = "Đào, phở và piano" , Price = 45000, Rooms = {1,2,3 } },
            new Movie { Name = "Mai" , Price = 100000, Rooms = {2,3 } },
            new Movie { Name = "Gặp lại chị bầu" , Price = 70000, Rooms = {1 } },
            new Movie { Name = "Tarot" , Price = 90000, Rooms = {3} }
        };
        private void Server_Load(object sender, EventArgs e)
        {

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();
            btn_start.Enabled = false;
            AddLog("Server đã bắt đầu lắng nghe trên cổng 8080...");
        }

        void StartServer()
        {
            Socket listenerSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Any, 8080);

            listenerSocket.Bind(ipepServer);
            listenerSocket.Listen(10);

            while (true)
            {
                try
                {
                    Socket clientSocket = listenerSocket.Accept();
                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    AddLog("Lỗi khi chấp nhận kết nối: " + ex.Message);
                }
            }
        }

        void HandleClient(Socket clientSocket)
        {
            string clientEndPoint = clientSocket.RemoteEndPoint.ToString();
            AddLog($"Client đã kết nối: {clientEndPoint}");
            try
            {
                // Thêm StreamWriter để gửi dữ liệu nhất quán với Client
                using (var stream = new NetworkStream(clientSocket))
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    // === VÒNG LẶP ĐỂ NHẬN NHIỀU YÊU CẦU TỪ CÙNG MỘT CLIENT ===
                    while (true)
                    {
                        string receivedData = reader.ReadLine();
                        // Nếu client ngắt kết nối, ReadLine sẽ trả về null -> thoát khỏi vòng lặp
                        if (receivedData == null)
                        {
                            break;
                        }

                        AddLog($"Nhận từ {clientEndPoint}: {receivedData}");
                        string[] parts = receivedData.Split('|');

                        string responseMessage = "";

                        if (parts.Length == 5 && parts[0] == "BOOK")
                        {
                            // --- Toàn bộ logic xử lý đặt vé của bạn giữ nguyên ở đây ---
                            string movieName = parts[1];
                            string roomNumber = parts[2];
                            string customerName = parts[3];
                            List<string> requestedSeats = parts[4].Split(',').ToList();

                            string ticketKey = $"{movieName}-{roomNumber}";
                            bool success = true;
                            string failedSeat = "";

                            lock (_lock)
                            {
                                if (!soldTickets.ContainsKey(ticketKey))
                                {
                                    soldTickets[ticketKey] = new List<string>();
                                }

                                foreach (var seat in requestedSeats)
                                {
                                    if (soldTickets[ticketKey].Contains(seat))
                                    {
                                        success = false;
                                        failedSeat = seat;
                                        break;
                                    }
                                }

                                if (success)
                                {
                                    soldTickets[ticketKey].AddRange(requestedSeats);
                                    responseMessage = $"SUCCESS|Đặt vé thành công cho {customerName} tại các ghế: {parts[4]}";
                                    AddLog($"Thành công: {customerName} đã đặt ghế {parts[4]} cho phim {movieName} tại rạp {roomNumber}.");
                                }
                                else
                                {
                                    responseMessage = $"FAIL|Ghế {failedSeat} đã được bán. Vui lòng chọn lại.";
                                    AddLog($"Thất bại: Yêu cầu đặt ghế {failedSeat} bị từ chối (đã bán).");
                                }
                            }
                        }
                        else
                        {
                            responseMessage = "FAIL|Dữ liệu không hợp lệ.";
                        }

                        // === THAY ĐỔI QUAN TRỌNG: DÙNG WRITER.WRITELINE() ===
                        writer.WriteLine(responseMessage);
                    }
                }
            }
            catch (IOException)
            {
                // Lỗi này xảy ra khi client ngắt kết nối đột ngột, đây là hành vi bình thường.
            }
            catch (Exception ex)
            {
                AddLog($"Lỗi xử lý client {clientEndPoint}: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
                AddLog($"Đã ngắt kết nối với client: {clientEndPoint}");
            }
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string selectedMovie = cbb_Movies.SelectedItem.ToString();
            string selectedRoom = cbb_Rooms.SelectedItem.ToString();
            string key = $"{selectedMovie}-{selectedRoom}";


            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb && cb.Name.StartsWith("cb_seat_"))
                {
                    cb.Enabled = true;
                    cb.Checked = false;
                    cb.BackColor = default(Color);
                }
            }

            lock (_lock)
            {
                if (soldTickets.ContainsKey(key))
                {
                    List<string> seats = soldTickets[key];
                    foreach (string seatName in seats)
                    {
                        string controlName = $"cb_seat_{seatName}";
                        Control[] foundControls = this.Controls.Find(controlName, true);
                        if (foundControls.Length > 0 && foundControls[0] is CheckBox cb)
                        {
                            cb.Enabled = false;
                            cb.Checked = true;
                            cb.BackColor = Color.Red;
                        }
                    }
                }
            }
        }


        private void btn_prc_Click(object sender, EventArgs e)
        {
            StringBuilder output = new StringBuilder();


            lock (_lock)
            {

                var movieStats = movies.Select(movie =>
                {
                    int soldCount = 0;
                    foreach (var room in movie.Rooms)
                    {
                        string key = $"{movie.Name}-{room}";
                        if (soldTickets.ContainsKey(key))
                        {
                            soldCount += soldTickets[key].Count;
                        }
                    }

                    int revenue = soldCount * movie.Price;

                    int totalSeats = movie.Rooms.Count * 15;

                    return new
                    {
                        Name = movie.Name,
                        SoldCount = soldCount,
                        TotalSeats = totalSeats,
                        Revenue = revenue
                    };
                })
                .OrderByDescending(stats => stats.Revenue) 
                .ToList();

                output.AppendLine("BÁO CÁO DOANH THU PHÒNG VÉ");
                output.AppendLine("--------------------------------------------------");

                int rank = 1; 
                foreach (var stat in movieStats)
                {
                    int remaining = stat.TotalSeats - stat.SoldCount;
                    double sellRatio = stat.TotalSeats > 0 ? (double)stat.SoldCount / stat.TotalSeats * 100 : 0;

                    output.AppendLine($"Phim: {stat.Name}");
                    output.AppendLine($" - Xếp hạng doanh thu: {rank}");
                    output.AppendLine($" - Doanh thu: {stat.Revenue:N0} VNĐ"); 
                    output.AppendLine($" - Số vé bán ra: {stat.SoldCount}");
                    output.AppendLine($" - Số vé tồn: {remaining}");
                    output.AppendLine($" - Tỉ lệ lấp đầy: {sellRatio:F2}%");
                    output.AppendLine("--------------------------------------------------");

                    rank++;
                }
            }

            tb_prc.Text = output.ToString();
        }
        private void cbb_Movies_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbb_Rooms.Items.Clear();

            Movie movie = movies[cbb_Movies.SelectedIndex];
            Console.WriteLine(cbb_Movies.SelectedIndex);
            foreach (int x in movie.Rooms)
            {
                cbb_Rooms.Items.Add(x.ToString());
            }
            if (cbb_Rooms.Items.Count > 0)
            {
                cbb_Rooms.SelectedIndex = 0;
            }
        }


        private void AddLog(string log)
        {
            if (tb_user.InvokeRequired)
            {
                tb_user.Invoke(new Action<string>(AddLog), log);
            }
            else
            {
                tb_user.AppendText(log + Environment.NewLine);
            }
        }
    }
}