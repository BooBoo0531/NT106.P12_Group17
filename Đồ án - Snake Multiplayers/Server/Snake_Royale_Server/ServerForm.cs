using Snake_Royale_Server.Handlers;

namespace Snake_Royale_Server
{
    public partial class ServerForm : Form
    {
        Server server;
        public ServerForm()
        {
            InitializeComponent();
        }

        public void AddServerInfo(string info)
        {
            txtServerInfo.Invoke(() =>
            {
                txtServerInfo.Text += info + "\n";
            });
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string portText = txtPort.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("Please enter IP address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(portText))
            {
                MessageBox.Show("Please enter port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int port = -1;
            try
            {
                port = int.Parse(portText);
            }
            catch (Exception ex)
            {

            }
            if (port == -1)
            {
                MessageBox.Show("Please enter port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            server = new Server(this);
            server.ExecuteServer(ip, port);
            startBtn.Enabled = false;
            closeBtn.Enabled = true;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.StopServer();
                startBtn.Enabled = true;
                closeBtn.Enabled = false;
                Gameplay.Instance.ResetGame();
            }
        }
    }
}
