using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cau1
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btn_Client_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Show();
        }

        private void btn_Server_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.Show();
        }
    }
}
