using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cau3
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }
        private void AddLog(string logMessage)
        {
            if (lw_log.InvokeRequired)
            {

                lw_log.Invoke(new Action<string>(AddLog), new object[] { logMessage });
                return;
            }

            lw_log.Items.Add(logMessage);
        }

        void StartServer()
        {
            Socket listenerSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            try
            {
                listenerSocket.Bind(ipepServer);
                listenerSocket.Listen(10); 

                AddLog("Server is listening on 127.0.0.1:8080...");


                    Socket clientSocket = listenerSocket.Accept();
                    AddLog("Client Kết Nối: " + clientSocket.RemoteEndPoint.ToString());

                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
            }
            catch (Exception ex)
            {
                AddLog("Server Error: " + ex.Message);
            }
            finally
            {
                listenerSocket?.Close();
            }
        }

        void HandleClient(Socket clientSocket)
        {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] buffer = new byte[1024];
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

                        if (!string.IsNullOrEmpty(completeMessage))
                        {
                            AddLog($"{clientSocket.RemoteEndPoint}: " + completeMessage.Trim());
                        }
                        messageBuilder.Remove(0, newlineIndex + 1);
                    }
                }

                AddLog("Client: " + clientSocket.RemoteEndPoint.ToString() + " Đã Rời Khỏi.");
            }
            catch (SocketException)
            {
                AddLog("Lỗi Client: " + clientSocket.RemoteEndPoint.ToString());
            }
            catch (Exception ex)
            {
                AddLog("Lỗi: " + ex.Message);
            }
            finally
            {
                clientSocket?.Shutdown(SocketShutdown.Both);
                clientSocket?.Close();
            }
        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true; 
            serverThread.Start();
            btn_listen.Enabled = false;
        }
    }
}
