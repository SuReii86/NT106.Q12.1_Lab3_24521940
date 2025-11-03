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
using System.IO;

namespace Cau6
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }
        private TcpClient tcpClient;
        private Thread receiveThread;

        private NetworkStream stream;
        private volatile bool isConnected;


        private void UpdateMessage(string msg)
        {
            if (tb_server.InvokeRequired)
            {
                tb_server.Invoke(new Action<string>(UpdateMessage), msg);
            }
            else
            {
                tb_server.AppendText(msg + Environment.NewLine);
            }
        }

        private void ListenMessage()
        {
            byte[] buffer = new byte[2048];
            int bytesRead;

            try
            {
                while (isConnected && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string serverMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    UpdateMessage(serverMessage.Trim());
                }
            }
            catch (IOException)
            {
                if (isConnected)
                {
                    UpdateMessage("Mất kết nối tới server.");
                }
            }
            catch (Exception ex)
            {
                if (isConnected) MessageBox.Show(ex.Message);
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient = new TcpClient();
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                tcpClient.Connect(iPEndPoint);
                isConnected = true;
                stream = tcpClient.GetStream();
                receiveThread = new Thread(ListenMessage);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                UpdateMessage("Đã kết nối tới Server!");

                btn_connect.Enabled = false;
                btn_send.Enabled = true;
                btn_close.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Kết Nối: " + ex.Message);
                isConnected = false;
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (isConnected && stream != null)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(tb_send.Text + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();
                    tb_server.AppendText("Me: " + tb_send.Text + Environment.NewLine);
                    tb_send.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Chưa Kết nối tới Server");
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                isConnected = false;
                stream?.Close();
                tcpClient?.Close();

                UpdateMessage("Đã ngắt kết nối.");

                btn_connect.Enabled = true;
                btn_send.Enabled = false;
                btn_close.Enabled = false;
            }
        }
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isConnected)
            {
                btn_close_Click(null, null);
            }
        }
    }
}