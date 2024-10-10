using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Client : Form
    {
        private TcpClient tcpClient;
        private StreamReader sReader;
        private StreamWriter sWriter;
        private Thread clientThread;
        private int serverPort = 8000;
        private bool stopTcpClient = true;
        private string messageBuffer = ""; 
        private string selectedFilePath = null;

        public Client()
        {
            InitializeComponent();
        }

        private void ClientRecv()
        {
            StreamReader sr = new StreamReader(tcpClient.GetStream());
            try
            {
                while (!stopTcpClient && tcpClient.Connected)
                {
                    Application.DoEvents();
                    string data = sr.ReadLine();
                    UpdateChatHistoryThreadSafe($"{data}\n");
                }
            }
            catch (SocketException sockEx)
            {
                tcpClient.Close();
                sr.Close();

            }
        }

        private delegate void SafeCallDelegate(string text);

        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.Text += text;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(sendMsgTextBox.Text))
                {
                    messageBuffer += $"{textBox3.Text};{sendMsgTextBox.Text}\n"; 
                    sendMsgTextBox.Text = ""; 
                }

                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    string fileData = Convert.ToBase64String(File.ReadAllBytes(selectedFilePath)); 
                    messageBuffer += $"File: {Path.GetFileName(selectedFilePath)};{fileData}\n";
                    selectedFilePath = ""; 
                }

                if (!string.IsNullOrEmpty(messageBuffer))
                {
                    SendMessageAsync(messageBuffer); 
                    messageBuffer = ""; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void SendMessageAsync(string message)
        {
            if (tcpClient.Connected)
            {
                byte[] data = Encoding.UTF8.GetBytes(message); 
                NetworkStream ns = tcpClient.GetStream();

                ns.BeginWrite(data, 0, data.Length, new AsyncCallback(SendCallback), ns);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            ns.EndWrite(ar);

            MessageBox.Show("Message and file sent successfully!");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                stopTcpClient = false;

                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(new IPEndPoint(IPAddress.Parse(textBox2.Text), serverPort));
                this.sWriter = new StreamWriter(tcpClient.GetStream());
                this.sWriter.AutoFlush = true;
                sWriter.WriteLine(this.textBox1.Text);
                clientThread = new Thread(this.ClientRecv);
                clientThread.Start();
                MessageBox.Show("Connected");
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show(sockEx.Message, "Network error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All files (*.*)|*.*"; 
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName; 
                    MessageBox.Show("File selected: " + selectedFilePath);
                }
            }

        }
    }
}
