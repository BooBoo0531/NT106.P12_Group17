using SimpleTCP;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;
        private TextBox someObject;

        private void btnConncet_Click(object sender, EventArgs e)
        {
            btnConncet.Enabled = false;
            try
            {
                client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text)); // Thêm kết nối ở đây
                txtStatus.Text += "Connected to server...\n";
            }
            catch (Exception ex)
            {
                txtStatus.Text += $"Connection failed: {ex.Message}\n";
                btnConncet.Enabled = true; // Cho phép thử lại nếu kết nối thất bại
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += $"Server said: {e.MessageString}\n";
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (client.TcpClient.Connected) // Kiểm tra kết nối đã sẵn sàng
            {
                // Gửi tin nhắn đến server
                var response = client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));

                // Hiển thị tin nhắn bạn vừa gửi với định dạng mong muốn
                txtStatus.Text += $"You said: {txtMessage.Text}{Environment.NewLine}";

                // Hiển thị phản hồi từ server nếu cần
                if (response != null)
                {
                    txtStatus.Text += $"Server replied: {response.MessageString}{Environment.NewLine}";
                }

                // Xóa text box sau khi gửi
                txtMessage.Clear();
            }
            else
            {
                txtStatus.Text += $"Client is not connected to the server.{Environment.NewLine}";
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a File";
                openFileDialog.Filter = "All Files (*.*)|*.*"; // Lọc tệp

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ///TextBox txtStatus = (TextBox)someObject; // Ép kiểu về TextBox
                    //txtStatus.Text = "Your text here";
                    txtStatus.Text = openFileDialog.FileName;// Giờ bạn có thể truy cập thuộc tính Text
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string filePath = txtStatus.Text; // Lấy đường dẫn tệp từ txtFilePath

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid file to send.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                byte[] fileData = File.ReadAllBytes(filePath); // Đọc nội dung tệp
                client.Write(fileData); // Gửi tệp dưới dạng byte
                txtStatus.Invoke((MethodInvoker)delegate ()
                {
                    txtStatus.Text += $"File sent: {filePath}\n"; // Hiển thị thông báo đã gửi tệp
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending file: {ex.Message}", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
