using System;
using System.Drawing;
using System.Windows.Forms;

namespace Board
{
    public partial class OnlineGame : Form
    {
        private System.Windows.Forms.Timer pollTimer = new System.Windows.Forms.Timer();
        private ComboBox cboRoom;

        public OnlineGame()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP!", "Thông báo");
                return;
            }

            int roomId = cboRoom.SelectedIndex + 1;

            lblStatus.Text = "Đang kết nối phòng " + roomId + "...";
            lblStatus.ForeColor = Color.Yellow;
            btnConnect.Enabled = false;

            bool success = NetworkManager.Connect(ip, 8888, roomId);
            if (!success)
            {
                lblStatus.Text = "Không thể kết nối! Kiểm tra lại IP.";
                lblStatus.ForeColor = Color.Red;
                btnConnect.Enabled = true;
                return;
            }

            lblStatus.Text = "Đã kết nối phòng " + roomId + "! Đang chờ...";
            lblStatus.ForeColor = Color.LightGreen;

            pollTimer.Interval = 300;
            pollTimer.Tick += PollTimer_Tick;
            pollTimer.Start();
        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            if (NetworkManager.MyRole == 0)
                lblStatus.Text = "Phòng " + NetworkManager.RoomId + " - Bạn là Quân Đỏ. Đang chờ đối thủ...";
            else if (NetworkManager.MyRole == 1)
                lblStatus.Text = "Phòng " + NetworkManager.RoomId + " - Bạn là Quân Đen. Đang chờ game...";
            else if (NetworkManager.MyRole == 2)
                lblStatus.Text = "Phòng " + NetworkManager.RoomId + " - Khán giả. Đang chờ 2 người chơi...";

            if (NetworkManager.GameStarted)
            {
                pollTimer.Stop();
                NetworkManager.IsOnline = true;

                if (NetworkManager.IsSpectator)
                {
                    Management.nameOfGamer1 = "Quân Đỏ";
                    Management.nameOfGamer2 = "Quân Đen";
                }
                else if (NetworkManager.MyRole == 0)
                {
                    Management.nameOfGamer1 = txtName.Text.Trim() != "" ? txtName.Text.Trim() : "Quân Đỏ";
                    Management.nameOfGamer2 = "Đối thủ (Quân Đen)";
                }
                else
                {
                    Management.nameOfGamer1 = "Đối thủ (Quân Đỏ)";
                    Management.nameOfGamer2 = txtName.Text.Trim() != "" ? txtName.Text.Trim() : "Quân Đen";
                }

                Management.minuteOfGamer1 = 15;
                Management.secondsOfGamer1 = 0;
                Management.minuteOfGamer2 = 15;
                Management.secondsOfGamer2 = 0;
                Management.isTimeCal = true;

                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pollTimer.Stop();
            NetworkManager.Disconnect();
            this.Close();
        }

        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRoom = new System.Windows.Forms.Label();
            this.cboRoom = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();

            this.lblTitle.Text = "CỜ TƯỚNG ONLINE";
            this.lblTitle.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.Gold;
            this.lblTitle.Location = new System.Drawing.Point(60, 15);
            this.lblTitle.Size = new System.Drawing.Size(320, 35);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblName.Text = "Tên của bạn:";
            this.lblName.ForeColor = Color.White;
            this.lblName.Location = new System.Drawing.Point(30, 60);
            this.lblName.Size = new System.Drawing.Size(100, 25);

            this.txtName.Location = new System.Drawing.Point(140, 60);
            this.txtName.Size = new System.Drawing.Size(200, 25);
            this.txtName.BackColor = Color.FromArgb(60, 60, 60);
            this.txtName.ForeColor = Color.White;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Text = "Người chơi";

            this.lblRoom.Text = "Chọn phòng:";
            this.lblRoom.ForeColor = Color.White;
            this.lblRoom.Location = new System.Drawing.Point(30, 95);
            this.lblRoom.Size = new System.Drawing.Size(100, 25);

            this.cboRoom.Location = new System.Drawing.Point(140, 95);
            this.cboRoom.Size = new System.Drawing.Size(200, 25);
            this.cboRoom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboRoom.Items.AddRange(new object[] { "Phòng 1", "Phòng 2" });
            this.cboRoom.SelectedIndex = 0;

            this.lblIP.Text = "IP Server:";
            this.lblIP.ForeColor = Color.White;
            this.lblIP.Location = new System.Drawing.Point(30, 130);
            this.lblIP.Size = new System.Drawing.Size(100, 25);

            this.txtIP.Location = new System.Drawing.Point(140, 130);
            this.txtIP.Size = new System.Drawing.Size(200, 25);
            this.txtIP.BackColor = Color.FromArgb(60, 60, 60);
            this.txtIP.ForeColor = Color.White;
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIP.Text = "127.0.0.1";

            this.lblStatus.Text = "Chọn phòng, nhập IP rồi nhấn Kết Nối";
            this.lblStatus.ForeColor = Color.LightGray;
            this.lblStatus.Location = new System.Drawing.Point(20, 170);
            this.lblStatus.Size = new System.Drawing.Size(400, 30);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnConnect.Text = "Kết Nối";
            this.btnConnect.Location = new System.Drawing.Point(90, 215);
            this.btnConnect.Size = new System.Drawing.Size(100, 35);
            this.btnConnect.BackColor = Color.FromArgb(0, 150, 0);
            this.btnConnect.ForeColor = Color.White;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            this.btnCancel.Text = "Hủy";
            this.btnCancel.Location = new System.Drawing.Point(210, 215);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.BackColor = Color.FromArgb(150, 0, 0);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.ClientSize = new System.Drawing.Size(440, 270);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.cboRoom);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnCancel);
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chơi Online";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRoom;

        public string GetPlayerName()
        {
            return txtName.Text.Trim() != "" ? txtName.Text.Trim() : "Người chơi";
        }
    }
}
