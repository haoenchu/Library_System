using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Windowsproject
{
    public partial class Form1 : Form
    {
        private ToolStripDropDown _filterDropDown;
        private CheckedListBox _clbCategory;

        // 右側Panel的小元件
        private Label lblTitleBar;
        private PictureBox pbCover;
        private Label lblTitle, lblDesc, lblAuthor, lblRating, lblRatingHint;
        private Label lblCategory, lblAdded;
        private Label lblStatus;
        private Button btnBorrow, btnReturn, btnScore, btnAudio;
        private Label lblBorrowHeader;
        private ListBox lbMyBooks;

        private List<Book> _books = new List<Book>();
        private ImageList imgList;
        private Book _selectedBook = null;

        //篩選狀態
        private Button _activeFilterBtn = null;  // 記錄目前啟動中的按鈕
        private readonly Color ColBtnDefault = Color.Linen;  // btnPopular 原色
        private readonly Color ColBtnNewDefault = Color.Linen; // btnNew 原色
        private readonly Color ColBtnActive = Color.FromArgb(100, 170, 230);   // 啟動時淺藍色

        //user名稱
        private string _currentUser = "";

        //書本簡介音檔
        private string _audioDir;
        private bool _isPlaying = false;



        public Form1(string currentUser)
        {
            InitializeComponent();
            DataManager.Initialize();      
            _books = DataManager.LoadBooks();
            _currentUser = currentUser;
            InitFilterDropDown();   // 初始化下拉篩選
            InitDetailPanel();
            InitListView();
            RefreshBookGrid();
            InitBorrowPanel();
            _audioDir = Path.Combine(Application.StartupPath, "audio");
        }

        //=========================左側借閱清單============================
        private void InitBorrowPanel()
        {
            lbuserinfo.Text = "歡迎登入  " +  _currentUser;
            // 調整字體大小與置中（可選，讓畫面更美觀）
            lbuserinfo.Font = new Font("Microsoft JhengHei", 14, FontStyle.Bold);
            lbuserinfo.TextAlign = ContentAlignment.MiddleCenter;

            // 初始化時先設定基本文字（稍後會在 RefreshBorrowList 更新數量）
            lbborrow_hmb.Text = "您目前已經借閱：0 本書";

            lblBorrowHeader = new Label
            {
                Text = $"📋 我的借閱（{_currentUser}）",
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Tan,
            };

            lbMyBooks = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Microsoft JhengHei UI", 9f),
                BackColor = Color.Bisque,
                ItemHeight = 42,
                DrawMode = DrawMode.OwnerDrawFixed,
            };
            lbMyBooks.DrawItem += LbMyBooks_DrawItem;
            lbMyBooks.SelectedIndexChanged += LbMyBooks_SelectedIndexChanged;

            // ← 順序：先加 Fill，再加 Top
            pnlBorrowList.Controls.Add(lbMyBooks);
            pnlBorrowList.Controls.Add(lblBorrowHeader);

            RefreshBorrowList();
        }

        private void RefreshBorrowList()
        {
            lbMyBooks.Items.Clear();

            var myBooks = _books.Where(b => b.Status == "已借出"
                                         && b.Borrower == _currentUser)
                                .ToList();

            foreach (var book in myBooks)
                lbMyBooks.Items.Add(book);

            // ✨ 新增：動態更新 lbborrow_hmb 的文字與借閱數量
            lbborrow_hmb.Text = $"您目前已經借閱：{myBooks.Count} 本書";

            // 沒有借閱中的書就顯示提示
            //if (myBooks.Count == 0)
            //    lbMyBooks.Items.Add(null);  // 用 null 當佔位，DrawItem 處理
        }

        private void LbMyBooks_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = lbMyBooks.Items[e.Index] as Book;

            // 底色
            bool selected = (e.State & DrawItemState.Selected) != 0;
            e.Graphics.FillRectangle(
                selected ? new SolidBrush(Color.Wheat)
                         : new SolidBrush(Color.Wheat),
                e.Bounds);

            // 無借閱提示
            if (item == null)
            {
                e.Graphics.DrawString("（目前無借閱中的書）",
                    new Font("Microsoft JhengHei UI", 9f),
                    Brushes.Gray,
                    new PointF(e.Bounds.X + 8, e.Bounds.Y + 13));
                return;
            }

            // 書名（第一行）
            e.Graphics.DrawString(
                ShortenTitle(item.Title),
                new Font("Microsoft JhengHei UI", 9.5f, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(30, 50, 100)),
                new PointF(e.Bounds.X + 8, e.Bounds.Y + 4));

            // 應還日期（第二行）— 逾期紅字
            bool overdue = item.DueDate.HasValue && DateTime.Today > item.DueDate.Value;
            string dueText = item.DueDate.HasValue
                ? (overdue ? $"⚠ {item.DueDate.Value:yyyy-MM-dd}" : $"還 {item.DueDate.Value:yyyy-MM-dd}")
                : "應還日期：--";

            e.Graphics.DrawString(
                dueText,
                new Font("Microsoft JhengHei UI", 8.5f),
                overdue ? Brushes.Crimson : new SolidBrush(Color.FromArgb(80, 100, 140)),
                new PointF(e.Bounds.X + 8, e.Bounds.Y + 24));

            // 底部分隔線
            e.Graphics.DrawLine(
                new Pen(Color.FromArgb(200, 215, 240)),
                e.Bounds.Left, e.Bounds.Bottom - 1,
                e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void LbMyBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            var book = lbMyBooks.SelectedItem as Book;
            if (book == null) return;

            // 同步選取 ListView（可選）
            _selectedBook = book;
            UpdateDetailPanel(book);
        }

        //=====================================================

        // =======================ListVeiw=====================
        private void InitListView()
        {
            imgList = new ImageList
            {
                ImageSize = new Size(96, 128),   // 封面比例 3:4
                ColorDepth = ColorDepth.Depth32Bit
            };
            lvBooks.LargeImageList = imgList;

            // 點選書籍時更新右側面板
            lvBooks.SelectedIndexChanged += (s, e) =>
            {
                if (lvBooks.SelectedItems.Count == 0)
                {
                    _selectedBook = null;
                    UpdateDetailPanel(null);
                    return;
                }
                _selectedBook = lvBooks.SelectedItems[0].Tag as Book;
                UpdateDetailPanel(_selectedBook);
            };
        }

        private void RefreshBookGrid()
        {
            // 先取得要顯示的書單（依目前篩選條件）
            var selected = _clbCategory.CheckedItems
                                       .Cast<string>()
                                       .ToList();

            List<Book> toShow = selected.Count == 0
                ? new List<Book>(_books)
                : _books.Where(b => selected.Contains(b.Category)).ToList();

            // 清空
            lvBooks.Items.Clear();
            imgList.Images.Clear();

            foreach (var book in toShow)
            {
                // 載入封面，沒有就用預設圖
                Image cover = GetCoverImage(book);

                imgList.Images.Add(book.BookId, cover);

                var item = new ListViewItem
                {
                    Text = ShortenTitle(book.Title),
                    ImageKey = book.BookId,
                    Tag = book          // 把 Book 物件藏在 Tag 裡，之後取用
                };

                // 已借出 → 書名紅字
                if (book.Status == "已借出")
                    item.ForeColor = Color.FromArgb(200, 50, 50);

                lvBooks.Items.Add(item);
            }
        }

        // 書名太長就截斷
        private string ShortenTitle(string title)
        {
            return title.Length > 8 ? title.Substring(0, 7) + "…" : title;
        }

        // 取封面圖，沒有封面就畫一個預設底色
        private Image GetCoverImage(Book book)
        {
            // 依書號找圖片，支援 jpg / png / jpeg
            string[] exts = { ".jpg", ".png", ".jpeg" };
            foreach (var ext in exts)
            {
                string path = Path.Combine(DataManager.ImagesDir, book.BookId + ext);
                if (File.Exists(path))
                {
                    try { return Image.FromFile(path); }
                    catch { }
                }
            }

            // 找不到就畫預設封面
            var bmp = new Bitmap(96, 128);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(180, 200, 230));
                var font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold);
                var rect = new RectangleF(6, 30, 84, 80);
                var fmt = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                g.DrawString(book.Title, font, Brushes.White, rect, fmt);
            }
            return bmp;
        }

        //====================================================================
        private void InitFilterDropDown()
        {
            _clbCategory = new CheckedListBox
            {
                CheckOnClick = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Microsoft JhengHei UI", 10f),
                Width = 140,
            };
            _clbCategory.Items.Add("文學小說");
            _clbCategory.Items.Add("資訊科技");
            _clbCategory.Items.Add("商業理財");
            _clbCategory.Items.Add("藝術設計");
            _clbCategory.ItemCheck += ClbCategory_ItemCheck;

            var host = new ToolStripControlHost(_clbCategory)
            {
                Padding = new Padding(4)
            };
            _filterDropDown = new ToolStripDropDown();
            _filterDropDown.Items.Add(host);
        }

        private void InitDetailPanel()
        {

            int y = 10;

            // ① 頂部書名標題
            lblTitleBar = new Label
            {
                Text = "（未選擇書籍）",
                Location = new Point(12, y),
                Size = new Size(224, 22),
                Font = new Font("Microsoft JhengHei UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 70, 140),
                AutoEllipsis = true
            };
            pnlDetail.Controls.Add(lblTitleBar);
            y += 28;

            // ② 封面 PictureBox
            pbCover = new PictureBox
            {
                Location = new Point(30, y),
                Size = new Size(185, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlDetail.Controls.Add(pbCover);
            y += 160;

            // ③ 書籍資訊
            lblTitle = AddDetailLabel($"■ 書名：", y); y += 22;
            lblAuthor = AddDetailLabel($"■ 作者：", y); y += 22;

            // 簡介 Label
            lblDesc = AddDetailLabel("", y);
            lblDesc.Size = new Size(224, 60);      // 多行高度
            lblDesc.AutoSize = false;
            lblDesc.Font = new Font("Microsoft JhengHei UI", 8.5f);
            lblDesc.ForeColor = Color.FromArgb(80, 100, 130);
            y += 68;


            // 聲音按鈕（用圖示切換播放/停止）
            btnAudio = new Button
            {
                Name = "btnAudio",
                Text = "🔊 播放簡介",
                Location = new Point(12, y),
                Size = new Size(105, 28),
                BackColor = Color.FromArgb(60, 140, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAudio.FlatAppearance.BorderSize = 0;
            btnAudio.Click += BtnAudio_Click;
            pnlDetail.Controls.Add(btnAudio);
            y += 36;

            lblRating = new Label
            {
                Text = "☆☆☆☆☆",
                Location = new Point(12, y),
                Size = new Size(224, 22),
                Font = new Font("Microsoft JhengHei UI", 13f),
                ForeColor = Color.FromArgb(220, 160, 0)
            };
            pnlDetail.Controls.Add(lblRating);
            y += 22;

            lblRatingHint = new Label
            {
                Text = "Rating",
                Location = new Point(14, y),
                Size = new Size(224, 16),
                Font = new Font("Microsoft JhengHei UI", 8f),
                ForeColor = Color.Gray
            };
            pnlDetail.Controls.Add(lblRatingHint);
            y += 20;

            lblCategory = AddDetailLabel("■ 分類：", y); y += 22;
            lblAdded = AddDetailLabel("■ 上架時間：", y); y += 10;

            AddSeparator(y); y += 14;

            // ④ 狀態
            lblStatus = new Label
            {
                Text = "---",
                Location = new Point(12, y),
                Size = new Size(224, 28),
                Font = new Font("Microsoft JhengHei UI", 14f, FontStyle.Bold),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlDetail.Controls.Add(lblStatus);
            y += 34;

            AddSeparator(y); y += 14;




            // ⑥ 操作按鈕
            AddSectionTitle("操作控制", y); y += 26;

            btnBorrow = CreateBtn("辦理借閱", new Point(12, y), new Size(105, 32),
                                  Color.FromArgb(30, 160, 70));
            btnReturn = CreateBtn("歸還書籍", new Point(122, y), new Size(105, 32),
                                  Color.FromArgb(30, 120, 180));
            pnlDetail.Controls.Add(btnBorrow);
            pnlDetail.Controls.Add(btnReturn);
            y += 40;

            btnScore = CreateBtn("評分書籍", new Point(12, y), new Size(215, 32),
                                  Color.FromArgb(190, 50, 50));
            pnlDetail.Controls.Add(btnScore);

            // 事件
            btnBorrow.Click += BtnBorrow_Click;
            btnReturn.Click += BtnReturn_Click;
            btnScore.Click += BtnScore_Click;

            // 初始狀態全部 disable
            UpdateDetailPanel(null);


        }

        private void BtnDesc_Click(object sender, EventArgs e)
        {
            if (_selectedBook == null) return;

            MessageBox.Show(
                _selectedBook.Description,
                $"📄 {_selectedBook.Title} 簡介",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnAudio_Click(object sender, EventArgs e)
        {
            if (_selectedBook == null) return;

            string audioPath = Path.Combine(_audioDir, _selectedBook.BookId + ".mp3");

            if (!File.Exists(audioPath))
            {
                MessageBox.Show("此書尚無聲音簡介。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 切換播放 / 停止
            if (_isPlaying)
            {
                wmp.Ctlcontrols.stop();
                _isPlaying = false;

                // 找到按鈕還原文字
                var btn = pnlDetail.Controls.Find("btnAudio", false);
                if (btn.Length > 0) btn[0].Text = "🔊 播放簡介";
            }
            else
            {
                wmp.URL = audioPath;
                wmp.Ctlcontrols.play();
                _isPlaying = true;

                var btn = pnlDetail.Controls.Find("btnAudio", false);
                if (btn.Length > 0) btn[0].Text = "⏹ 停止";
            }
        }

        private Label AddDetailLabel(string text, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(12, y),
                Size = new Size(224, 22),
                Font = new Font("Microsoft JhengHei UI", 9.5f),
                ForeColor = Color.FromArgb(40, 55, 90),
                AutoEllipsis = true
            };
            pnlDetail.Controls.Add(lbl);
            return lbl;
        }

        private void AddSeparator(int y)
        {
            var sep = new Panel
            {
                Location = new Point(12, y),
                Size = new Size(224, 1),
                BackColor = Color.FromArgb(200, 215, 240)
            };
            pnlDetail.Controls.Add(sep);
        }

        

        private void AddSectionTitle(string text, int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(12, y),
                Size = new Size(224, 20),
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 70, 140)
            };
            pnlDetail.Controls.Add(lbl);
        }

        private Button CreateBtn(string text, Point loc, Size size, Color bg)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft JhengHei UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void UpdateDetailPanel(Book book)
        {
            // 切換書籍時停止音訊
            if (_isPlaying)
            {
                wmp.Ctlcontrols.stop();
                _isPlaying = false;
                btnAudio.Text = "🔊 播放簡介";

            }

            if (book == null)
            {
                lblTitleBar.Text = "（未選擇書籍）";
                pbCover.Image = null;
                pbCover.BackColor = Color.FromArgb(210, 222, 245);
                lblTitle.Text = "■ 書名：";
                lblAuthor.Text = "■ 作者：";
                lblDesc.Text = "";
                lblRating.Text = "☆☆☆☆☆";
                lblCategory.Text = "■ 分類：";
                lblAdded.Text = "■ 上架時間：";
                lblStatus.Text = "---";
                lblStatus.ForeColor = Color.Gray;
                btnBorrow.Enabled = false;
                btnReturn.Enabled = false;
                btnScore.Enabled = false;
                btnAudio.Enabled = false;
                return;
            }

            bool borrowed = book.Status == "已借出";
            bool overdue = borrowed && book.DueDate.HasValue
                            && DateTime.Today > book.DueDate.Value;

            lblTitleBar.Text = $"{book.Title}（{book.Author}）";
            lblTitle.Text = $"■ 書名：{book.Title}";
            lblAuthor.Text = $"■ 作者：{book.Author}";
            lblDesc.Text = string.IsNullOrEmpty(book.Description) ? "（暫無簡介）" : book.Description;
            lblRating.Text = new string('★', book.Rating)
                              + new string('☆', 5 - book.Rating);
            lblCategory.Text = $"■ 分類：{book.Category}";
            lblAdded.Text = $"■ 上架時間：{book.AddedDate:yyyy-MM-dd}";


            if (overdue)
            {
                lblStatus.Text = "⚠ 已逾期";
                lblStatus.ForeColor = Color.FromArgb(200, 50, 0);
            }
            else if (borrowed)
            {
                lblStatus.Text = "● 已借出";
                lblStatus.ForeColor = Color.FromArgb(200, 50, 50);
            }
            else
            {
                lblStatus.Text = "● 在館中";
                lblStatus.ForeColor = Color.FromArgb(30, 160, 70);
            }

            if (!borrowed)
            {
                // 在館中：可借閱，不能還
                btnBorrow.Enabled = true;
                btnReturn.Enabled = false;
            }
            else if (book.Borrower == _currentUser)
            {
                // 已借出且是自己：只能還
                btnBorrow.Enabled = false;
                btnReturn.Enabled = true;
            }
            else
            {
                // 已借出且是別人：兩個都不能按
                btnBorrow.Enabled = false;
                btnReturn.Enabled = false;
            }
            btnAudio.Enabled = true;
            btnScore.Enabled = true;

            //// 封面圖片
            var coverPath = GetCoverImage(book);
            try { pbCover.Image = coverPath; }
            catch { pbCover.Image = null; }
            
        }

        private void lvBooks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void BtnBorrow_Click(object sender, EventArgs e)
        {
            if (_selectedBook == null) return;

            var result = MessageBox.Show(
                $"確認借閱《{_selectedBook.Title}》？\n\n" +
                $"借閱人：{_currentUser}\n" +
                $"應還日期：{DateTime.Today.AddDays(14):yyyy-MM-dd}",
                "辦理借閱",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK) return;

            _selectedBook.Status = "已借出";
            _selectedBook.Borrower = _currentUser;        // ← 用登入帳號
            _selectedBook.DueDate = DateTime.Today.AddDays(14);

            DataManager.SaveBooks(_books);
            RefreshBookGrid();
            RefreshBorrowList();                          // ← 更新左側清單
            UpdateDetailPanel(_selectedBook);

            MessageBox.Show($"借閱成功！\n應還日期：{_selectedBook.DueDate:yyyy-MM-dd}",
                "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        


        private void BtnReturn_Click(object sender, EventArgs e) 
        {
            if (_selectedBook == null) return;

            // 計算逾期
            int overdueDays = 0;
            int fine = 0;
            if (_selectedBook.DueDate.HasValue)
            {
                overdueDays = Math.Max(0,
                    (int)(DateTime.Today - _selectedBook.DueDate.Value).TotalDays);
                fine = overdueDays * 5;
            }

            // 確認視窗
            string msg = overdueDays > 0
                ? $"《{_selectedBook.Title}》已逾期 {overdueDays} 天\n💰 罰金：{fine} 元\n\n確認還書並收取罰金？"
                : $"確認歸還《{_selectedBook.Title}》？";

            var result = MessageBox.Show(msg, "歸還書籍",
                MessageBoxButtons.OKCancel,
                overdueDays > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question);

            if (result != DialogResult.OK) return;

            _selectedBook.Status = "在館中";
            _selectedBook.Borrower = "";
            _selectedBook.DueDate = null;

            DataManager.SaveBooks(_books);
            RefreshBookGrid();
            RefreshBorrowList();                          // ← 更新左側清單
            UpdateDetailPanel(_selectedBook);

            MessageBox.Show("還書成功！書籍已歸還入館。",
                "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnScore_Click(object sender, EventArgs e)
        {
            if (_selectedBook == null) return;

            // 建立評分視窗
            var dlg = new Form
            {
                Text = "書籍評分",
                Size = new Size(300, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(245, 248, 255),
                Font = new Font("Microsoft JhengHei UI", 10f)
            };

            dlg.Controls.Add(new Label
            {
                Text = $"《{_selectedBook.Title}》評分",
                Location = new Point(20, 20),
                Size = new Size(240, 24),
                Font = new Font("Microsoft JhengHei UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 70, 140)
            });

            dlg.Controls.Add(new Label
            {
                Text = "選擇星級：",
                Location = new Point(20, 58),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 80, 120)
            });

            // 星級下拉
            var cmb = new ComboBox
            {
                Location = new Point(100, 55),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft JhengHei UI", 10f)
            };
            cmb.Items.AddRange(new string[]
            {
        "⭐ 1 星", "⭐⭐ 2 星", "⭐⭐⭐ 3 星",
        "⭐⭐⭐⭐ 4 星", "⭐⭐⭐⭐⭐ 5 星"
            });
            cmb.SelectedIndex = _selectedBook.Rating - 1;  // 預選目前評分
            dlg.Controls.Add(cmb);

            // 確認按鈕
            var btnOK = new Button
            {
                Text = "確認",
                Location = new Point(80, 110),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(40, 70, 140),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnOK.FlatAppearance.BorderSize = 0;
            dlg.Controls.Add(btnOK);

            var btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(170, 110),
                Size = new Size(80, 32),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            dlg.Controls.Add(btnCancel);
            dlg.AcceptButton = btnOK;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _selectedBook.Rating = cmb.SelectedIndex + 1;
                DataManager.SaveBooks(_books);
                UpdateDetailPanel(_selectedBook);   // 右側星星即時更新

                MessageBox.Show($"評分已更新為 {_selectedBook.Rating} 星！",
                    "評分完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // ========================TOP_BTN程式代碼============================


        // btnFilterCategory 的 Click 事件（Designer 雙擊產生）
        private void btnFilterCategory_Click(object sender, EventArgs e)
        {
            _filterDropDown.Show(btnFilterCategory,
                new Point(0, btnFilterCategory.Height));
        }

        private void ClbCategory_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 點分類篩選時，取消熱門/新書的啟動狀態
            if (_activeFilterBtn != null)
            {
                _activeFilterBtn.BackColor = Color.Linen;
                _activeFilterBtn = null;
            }

            int checkedCount = _clbCategory.CheckedItems.Count
                + (e.NewValue == CheckState.Checked ? 1 : -1);

            btnFilterCategory.Text = checkedCount == 0
                ? "▼  全部分類"
                : $"▼  分類篩選 ({checkedCount})";

            this.BeginInvoke((Action)(() => RefreshBookGrid()));
        }
        private void ToggleFilterBtn(Button clicked)
        {
            // 再點同一個 → 取消，恢復全部書單
            if (_activeFilterBtn == clicked)
            {
                clicked.BackColor = Color.Linen;
                _activeFilterBtn = null;
                RefreshBookGrid();
                return;
            }

            // 還原上一個按鈕顏色
            if (_activeFilterBtn != null)
                _activeFilterBtn.BackColor = Color.Linen;

            // 啟動新的
            clicked.BackColor = ColBtnActive;
            _activeFilterBtn = clicked;
        }

        private void btnPopular_Click(object sender, EventArgs e)
        {
            ToggleFilterBtn(btnPopular);

            if (_activeFilterBtn != btnPopular) return; // 已取消就直接返回

            var top5 = _books.OrderByDescending(b => b.Rating)
                             .Take(5)
                             .ToList();

            lvBooks.Items.Clear();
            imgList.Images.Clear();

            foreach (var book in top5)
            {
                Image cover = GetCoverImage(book);
                imgList.Images.Add(book.BookId, cover);

                var item = new ListViewItem
                {
                    Text = ShortenTitle(book.Title),
                    ImageKey = book.BookId,
                    Tag = book
                };
                if (book.Status == "已借出")
                    item.ForeColor = Color.FromArgb(200, 50, 50);
                lvBooks.Items.Add(item);
            }

            for (int i = 0; i < _clbCategory.Items.Count; i++)
                _clbCategory.SetItemChecked(i, false);
            btnFilterCategory.Text = "▼  全部分類";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ToggleFilterBtn(btnNew);

            if (_activeFilterBtn != btnNew) return;

            var newBooks = _books.Where(b => (DateTime.Today - b.AddedDate).TotalDays <= 30)
                                 .ToList();

            lvBooks.Items.Clear();
            imgList.Images.Clear();

            foreach (var book in newBooks)
            {
                Image cover = GetCoverImage(book);
                imgList.Images.Add(book.BookId, cover);

                var item = new ListViewItem
                {
                    Text = ShortenTitle(book.Title),
                    ImageKey = book.BookId,
                    Tag = book
                };
                if (book.Status == "已借出")
                    item.ForeColor = Color.FromArgb(200, 50, 50);
                lvBooks.Items.Add(item);
            }

            for (int i = 0; i < _clbCategory.Items.Count; i++)
                _clbCategory.SetItemChecked(i, false);
            btnFilterCategory.Text = "▼  全部分類";

            if (newBooks.Count == 0)
                MessageBox.Show("目前沒有 30 天內的新書。", "新書專區",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ====================================================================

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // 停止音訊
            if (_isPlaying)
            {
                wmp.Ctlcontrols.stop();
                _isPlaying = false;
            }

            // 關閉主視窗，重新跑登入流程
            this.Hide();

            using (var login = new LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    _currentUser = login.LoggedInUser;
                    _books = DataManager.LoadBooks();
                    RefreshBookGrid();
                    RefreshBorrowList();
                    lblBorrowHeader.Text = $"📋 我的借閱（{_currentUser}）";
                    lbuserinfo.Text = "歡迎登入  " + _currentUser;
                    UpdateDetailPanel(null);
                    this.Show();
                }
                else
                {
                    // 登入視窗直接關掉 → 結束程式
                    Application.Exit();
                }
            }
        }

    }
}
