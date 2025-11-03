using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cau6
{
    public partial class Server : Form
    {
        private List<Socket> clientSockets = new List<Socket>();
        private readonly object lockObj = new object();
        private Socket listenerSocket;
        private volatile bool isRunning;

        public Server()
        {
            InitializeComponent();
        }

        private void AddLog(string logMessage)
        {
            if (lw_log.InvokeRequired)
            {
                lw_log.Invoke(new Action<string>(AddLog), logMessage);
            }
            else
            {
                lw_log.Items.Add(logMessage);
            }
        }

        void StartServer()
        {
            listenerSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Any, 8080);

            try
            {
                listenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listenerSocket.Bind(ipepServer);
                listenerSocket.Listen(10);
                isRunning = true;

                AddLog("Server đang lắng nghe trên cổng 8080...");

                while (isRunning)
                {
                    Socket clientSocket = listenerSocket.Accept();
                    AddLog("Client kết nối: " + clientSocket.RemoteEndPoint.ToString());
                    lock (lockObj)
                    {
                        clientSockets.Add(clientSocket);
                    }

                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                if (!isRunning)
                {
                    AddLog("Server đã dừng.");
                }
                else
                {
                    AddLog("Lỗi Socket: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                AddLog("Lỗi Server: " + ex.Message);
            }
        }


        void HandleClient(Socket clientSocket)
        {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] buffer = new byte[2048];
            int bytesReceived;

            try
            {
                while ((bytesReceived = clientSocket.Receive(buffer)) > 0)
                {
                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));

                    while (messageBuilder.ToString().Contains("\n"))
                    {
                        int newlineIndex = messageBuilder.ToString().IndexOf('\n');
                        string completeMessage = messageBuilder.ToString(0, newlineIndex);
                        messageBuilder.Remove(0, newlineIndex + 1);

                        if (!string.IsNullOrEmpty(completeMessage))
                        {
                            string messageToBroadcast = $"{clientSocket.RemoteEndPoint}: {completeMessage.Trim()}";
                            AddLog(messageToBroadcast);


                            BroadcastMessage(messageToBroadcast, clientSocket);
                        }
                    }
                }
                AddLog("Client " + clientSocket.RemoteEndPoint.ToString() + " đã ngắt kết nối.");
            }
            catch (SocketException)
            {
                AddLog("Client " + clientSocket.RemoteEndPoint.ToString() + " đã ngắt kết nối đột ngột.");
            }
            catch (Exception ex)
            {
                AddLog("Lỗi xử lý client: " + ex.Message);
            }
            finally
            {
                lock (lockObj)
                {
                    clientSockets.Remove(clientSocket);
                }
                clientSocket?.Shutdown(SocketShutdown.Both);
                clientSocket?.Close();
            }
        }

        void BroadcastMessage(string message, Socket excludeClient)
        {
            byte[] data = Encoding.UTF8.GetBytes(message + "\n");

            lock (lockObj)
            {
                foreach (Socket client in clientSockets)
                {
                    if (client != excludeClient && client.Connected)
                    {
                        try
                        {
                            client.Send(data);
                        }
                        catch (Exception ex)
                        {
                            AddLog("Lỗi gửi tin tới " + client.RemoteEndPoint + ": " + ex.Message);
                        }
                    }
                }
            }
        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();

            btn_listen.Enabled = false;
            // btn_stop.Enabled = true; // Bật nút stop
        }

        // === THÊM HÀM XỬ LÝ CHO NÚT STOP ===
        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;

                // Đóng tất cả các kết nối client
                lock (lockObj)
                {
                    foreach (Socket client in clientSockets)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    clientSockets.Clear();
                }

                // Đóng socket lắng nghe chính để thoát khỏi vòng lặp Accept()
                listenerSocket?.Close();

                btn_listen.Enabled = true;
                // btn_stop.Enabled = false;
            }
        }

        // Đảm bảo server tắt hẳn khi đóng form
        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            btn_stop_Click(null, null);
        }
    }
}