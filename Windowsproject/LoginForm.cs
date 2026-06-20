using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Windowsproject
{
    public partial class LoginForm : Form
    {
        public string LoggedInUser { get; private set; }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Button btnForgetpassword;
        private Label lblMessage;
        private bool _isResetMode = false;

        private string _csvPath;

        public LoginForm()
        {
            InitializeComponent();
            _csvPath = Path.Combine(Application.StartupPath, "Resources\\users.csv");
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "智慧圖書管理系統 － 登入";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Microsoft JhengHei UI", 10f);

            // 標題
            var lblTitle = new Label
            {
                Text = "📚 智慧圖書管理系統",
                Font = new Font("Microsoft JhengHei UI", 13f, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                Size = new Size(320, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            var sep = new Panel
            {
                BackColor = Color.Black,
                Location = new Point(20, 55),
                Size = new Size(320, 2)
            };
            this.Controls.Add(sep);

            // 帳號
            var lblUser = new Label
            {
                Text = "帳號",
                Location = new Point(40, 75),
                Size = new Size(50, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblUser);

            txtUsername = new TextBox
            {
                Location = new Point(100, 75),
                Size = new Size(220, 28),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Microsoft JhengHei UI", 10f)
            };
            this.Controls.Add(txtUsername);

            // 密碼
            var lblPass = new Label
            {
                Text = "密碼",
                Location = new Point(40, 115),
                Size = new Size(50, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblPass);

            txtPassword = new TextBox
            {
                Location = new Point(100, 115),
                Size = new Size(220, 28),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Microsoft JhengHei UI", 10f),
                PasswordChar = '●'   // 密碼遮罩
            };
            this.Controls.Add(txtPassword);

            // 訊息列（登入失敗提示）
            lblMessage = new Label
            {
                Text = "",
                Location = new Point(40, 148),
                Size = new Size(290, 20),
                ForeColor = Color.FromArgb(200, 50, 50),
                Font = new Font("Microsoft JhengHei UI", 9f),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblMessage);

            // 登入按鈕
            btnLogin = new Button
            {
                Text = "登入",
                Location = new Point(45, 175),
                Size = new Size(90, 36),
                BackColor = Color.BurlyWood,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // 註冊按鈕
            btnRegister = new Button
            {
                Text = "註冊",
                Location = new Point(150, 175),
                Size = new Size(90, 36),
                BackColor = Color.BurlyWood,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            btnForgetpassword = new Button
            {
                Text = "忘記密碼",
                Location = new Point(265, 175),   // ← 放在最右邊
                Size = new Size(90, 36),
                BackColor = Color.BurlyWood,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnForgetpassword.FlatAppearance.BorderSize = 0;
            btnForgetpassword.Click += BtnForgetpassword_Click;
            this.Controls.Add(btnForgetpassword);   


            // Enter 鍵觸發登入
            this.AcceptButton = btnLogin;
        }

        // ── 登入邏輯 ──────────────────────────────────────────────
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                lblMessage.Text = "帳號與密碼不能為空！";
                return;
            }

            var users = LoadUsers();
            if (users.ContainsKey(user) && users[user] == pass)
            {
                LoggedInUser = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblMessage.Text = "帳號或密碼錯誤，請再試一次。";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ── 註冊邏輯 ──────────────────────────────────────────────
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                lblMessage.Text = "帳號與密碼不能為空！";
                return;
            }

            var users = LoadUsers();

            if (users.ContainsKey(user))
            {
                lblMessage.Text = "此帳號已存在，請換一個。";
                return;
            }

            // 寫入新帳號
            using (var sw = new StreamWriter(_csvPath, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine($"{user},{pass}");
            }

            lblMessage.ForeColor = Color.FromArgb(30, 130, 60);
            lblMessage.Text = $"註冊成功！請用新帳號登入。";
            txtPassword.Clear();
        }

        private void BtnForgetpassword_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();

            if (!_isResetMode)
            {
                // ── 第一次點：進入重設模式 ──────────────────────────
                if (string.IsNullOrEmpty(user))
                {
                    lblMessage.ForeColor = Color.FromArgb(200, 50, 50);
                    lblMessage.Text = "請先輸入帳號！";
                    return;
                }

                var users = LoadUsers();
                if (!users.ContainsKey(user))
                {
                    lblMessage.ForeColor = Color.FromArgb(200, 50, 50);
                    lblMessage.Text = "此帳號不存在。";
                    return;
                }

                // 切換到重設模式
                _isResetMode = true;
                btnForgetpassword.Text = "重設密碼";
                txtPassword.Clear();
                txtPassword.Focus();

                lblMessage.ForeColor = Color.FromArgb(30, 100, 180);
                lblMessage.Text = "請在密碼欄輸入新密碼。";
            }
            else
            {
                // ── 第二次點：寫入新密碼 ────────────────────────────
                string newPass = txtPassword.Text;

                if (string.IsNullOrEmpty(newPass))
                {
                    lblMessage.ForeColor = Color.FromArgb(200, 50, 50);
                    lblMessage.Text = "新密碼不能為空！";
                    return;
                }

                // 讀出所有帳號，更新目標帳號的密碼
                var users = LoadUsers();
                users[user] = newPass;

                // 重寫整個 CSV
                using (var sw = new StreamWriter(_csvPath, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine("Username,Password");
                    foreach (var kvp in users)
                        sw.WriteLine($"{kvp.Key},{kvp.Value}");
                }

                // 還原狀態
                _isResetMode = false;
                btnForgetpassword.Text = "忘記密碼";
                txtPassword.Clear();

                lblMessage.ForeColor = Color.FromArgb(30, 130, 60);
                lblMessage.Text = "密碼重設成功！請重新登入。";
            }
        }

        // ── 讀取所有帳號 ─────────────────────────────────────────
        private Dictionary<string, string> LoadUsers()
        {
            var dict = new Dictionary<string, string>();
            if (!File.Exists(_csvPath)) return dict;

            using (var sr = new StreamReader(_csvPath, System.Text.Encoding.UTF8))
            {
                string line;
                bool isHeader = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isHeader) { isHeader = false; continue; }
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(',');
                    if (parts.Length >= 2)
                        dict[parts[0].Trim()] = parts[1].Trim();
                }
            }
            return dict;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}