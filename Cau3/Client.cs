using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Cau3
{
    public partial class Client : Form
    {
        private TcpClient tcpClient;
        private NetworkStream stream;

        public Client()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient = new TcpClient();
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

                tcpClient.Connect(iPEndPoint);

                stream = tcpClient.GetStream();

                MessageBox.Show("Đã Kết Nối tới Server!");

                btn_connect.Enabled = false;
                btn_send.Enabled = true;
                btn_close.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Kết Nối: " + ex.Message);
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (tcpClient.Connected && stream != null)
            {
                try
                {

                    byte[] data = Encoding.UTF8.GetBytes(tb_send.Text + "\n");

                    stream.Write(data, 0, data.Length);
                    stream.Flush();

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
            if (tcpClient != null && tcpClient.Connected)
            {
                stream.Close();
                tcpClient.Close();

                MessageBox.Show("Đã Ngắt Kết Nối");

                btn_connect.Enabled = true;
                btn_send.Enabled = false;
                btn_close.Enabled = false;
            }
        }
    }
}