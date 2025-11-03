using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cau4
{
    public partial class Client : Form
    {
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;

        public Client()
        {
            InitializeComponent();

            cbb_Movies.Items.AddRange(movies.Select(a => a.Name).ToArray());
            var allRooms = movies.SelectMany(a => a.Rooms).Distinct().OrderBy(r => r).Select(i => (object)i).ToArray();
            cbb_Rooms.Items.AddRange(allRooms);
            cbb_Movies.SelectedIndex = 0;
            cbb_Rooms.SelectedIndex = 0;

            btn_confirm.Enabled = false; 
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

        private async void btn_connect_Click(object sender, EventArgs e)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                MessageBox.Show("Bạn đã kết nối rồi!");
                return;
            }

            try
            {
                tcpClient = new TcpClient();
                Task connectTask = tcpClient.ConnectAsync("127.0.0.1", 8080);
                if (await Task.WhenAny(connectTask, Task.Delay(3000)) != connectTask)
                {
                    tcpClient.Close();
                    throw new Exception("Kết nối tới server quá hạn (timeout).");
                }

                var stream = tcpClient.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                Task.Run(() => ListenForMessages());

                btn_connect.Enabled = false;
                btn_confirm.Enabled = true;
                MessageBox.Show("Kết nối tới server thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect(); 
            }
        }

        private void ListenForMessages()
        {
            try
            {
                while (tcpClient != null && tcpClient.Connected)
                {

                    string serverMessage = reader.ReadLine();
                    if (serverMessage == null)
                    {
                        break;
                    }

                    this.Invoke(new Action(() => {

                        MessageBox.Show(serverMessage, "Phản hồi từ Server");
                        if (serverMessage.StartsWith("SUCCESS"))
                        {
                            ResetForm();
                        }
                    }));
                }
            }
            catch (IOException) { /* Lỗi này thường xảy ra khi kết nối bị ngắt đột ngột */ }
            catch (ObjectDisposedException) { /* Lỗi này xảy ra khi client bị đóng trong lúc đang chờ đọc */ }
            finally
            {
                this.Invoke(new Action(() => Disconnect()));
            }
        }


        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (tcpClient == null || !tcpClient.Connected)
            {
                MessageBox.Show("Vui lòng kết nối đến server trước.", "Chưa kết nối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string movieName = cbb_Movies.SelectedItem.ToString();
            string roomNumber = cbb_Rooms.SelectedItem.ToString();
            string customerName = tb_NAME.Text.Trim();
            List<string> selectedSeats = new List<string>();
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb && cb.Checked && cb.Name.StartsWith("cb_seat_"))
                {
                    selectedSeats.Add(cb.Name.Substring(8));
                }
            }
            if (string.IsNullOrWhiteSpace(customerName) || selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn ghế.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string seatsString = string.Join(",", selectedSeats);
            string message = $"BOOK|{movieName}|{roomNumber}|{customerName}|{seatsString}";

            try
            {

                writer.WriteLine(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();
            }
        }


        private void Disconnect()
        {
            if (tcpClient != null)
            {
                reader?.Close();
                writer?.Close();
                tcpClient.Close();
                tcpClient = null; 

                MessageBox.Show("Đã ngắt kết nối khỏi server.");
            }

            btn_connect.Enabled = true;
            btn_confirm.Enabled = false;
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }

        private void ResetForm()
        {
            tb_NAME.Clear();
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox cb && cb.Name.StartsWith("cb_seat_"))
                {
                    cb.Checked = false;
                }
            }
        }

    }
}