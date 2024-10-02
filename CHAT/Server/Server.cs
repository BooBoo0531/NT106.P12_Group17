using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;

namespace CHAT
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        private void Sever_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; // enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                if (e.Data != null && e.Data.Length > 0) // Kiểm tra dữ liệu
                {
                    // Ghi dữ liệu vào tệp
                    string filePath = Path.Combine(Environment.CurrentDirectory, "ReceivedFile.dat"); // Tạo đường dẫn lưu tệp
                    File.WriteAllBytes(filePath, e.Data);
                    txtStatus.Text += $"File received and saved as: {filePath}\n";
                }
                else
                {
                    txtStatus.Text += e.MessageString; // Nhận tin nhắn văn bản
                    e.ReplyLine(string.Format("You said: {0}", e.MessageString));
                }
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Server starting...\n";
            if (!IPAddress.TryParse(txtHost.Text, out var ip))
            {
                MessageBox.Show("Invalid IP Address.");
                return;
            }
            server.Start(ip, Convert.ToInt32(txtPort.Text));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(server.IsStarted) 
                server.Stop();
        }
    }
}
