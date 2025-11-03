using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cau1
{
    public partial class Server : Form
    {
        private UdpClient udpServer;
        private Thread listenThread;
        private volatile bool _isRunning;

        public Server()
        {
            InitializeComponent();
            btn_Close.Enabled = false;
            tb_port.Text = "8080";
        }

        private void btn_listen_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tb_port.Text, out int port))
            {
                MessageBox.Show("Port không hợp lệ!");
                return;
            }


            listenThread = new Thread(() => ListenForMessages(port));
            listenThread.IsBackground = true;
            listenThread.Start();

            btn_listen.Enabled = false;
            btn_Close.Enabled = true;

            LogMessage("Server is listening on 127.0.0.1:8080...");
        }

        private void ListenForMessages(int port)
        {
            try
            {
                udpServer = new UdpClient(port);
                _isRunning = true;

                while (_isRunning)
                {

                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);


                    byte[] receivedBytes = udpServer.Receive(ref remoteEP);

                    if (receivedBytes.Length > 0)
                    {
                        string dataReceived = Encoding.UTF8.GetString(receivedBytes);
                        string clientAddress = remoteEP.Address.ToString();
                        LogMessage($"{clientAddress}: {dataReceived.Trim()}");
                    }
                }
            }
            catch (SocketException ex)
            {
                if (_isRunning)
                {
                    LogMessage("Lỗi Socket: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Server: " + ex.Message);
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            _isRunning = false;
            udpServer?.Close();

            if (listenThread != null && listenThread.IsAlive)
            {
                listenThread.Join(500);
            }

            btn_listen.Enabled = true;
            btn_Close.Enabled = false;
            LogMessage("Đã Dừng Server.");
        }

        private void LogMessage(string message)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => LogMessage(message)));
            }
            else
            {
                listView1.Items.Add(message);
            }
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isRunning)
            {
                btn_Close_Click(null, null);
            }
        }
    }
}