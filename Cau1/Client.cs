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

namespace Cau1
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            tb_port.Text = "8080";
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                Byte[] sendBytes = Encoding.UTF8.GetBytes(tb_send.Text);
                udpClient.Send(sendBytes, sendBytes.Length, tb_host.Text, 8080);
                tb_send.ResetText();
            }
            catch (Exception ex)
            {
                tb_send.Text = ex.Message;
            };
        }
    }
}
