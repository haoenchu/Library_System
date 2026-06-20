namespace Windowsproject
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnPopular = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.btnFilterCategory = new System.Windows.Forms.Button();
            this.lvBooks = new System.Windows.Forms.ListView();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.wmp = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlBorrowList = new System.Windows.Forms.Panel();
            this.lbuserinfo = new System.Windows.Forms.Label();
            this.lbborrow_hmb = new System.Windows.Forms.Label();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pnlToolbar.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wmp)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPopular
            // 
            this.btnPopular.BackColor = System.Drawing.Color.Linen;
            this.btnPopular.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPopular.Location = new System.Drawing.Point(984, 6);
            this.btnPopular.Name = "btnPopular";
            this.btnPopular.Size = new System.Drawing.Size(175, 58);
            this.btnPopular.TabIndex = 1;
            this.btnPopular.Text = "熱門排行";
            this.btnPopular.UseVisualStyleBackColor = false;
            this.btnPopular.Click += new System.EventHandler(this.btnPopular_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Linen;
            this.btnNew.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnNew.Location = new System.Drawing.Point(1160, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(175, 58);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "新書推薦";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.BurlyWood;
            this.pnlToolbar.Controls.Add(this.btnQuit);
            this.pnlToolbar.Controls.Add(this.lbborrow_hmb);
            this.pnlToolbar.Controls.Add(this.lbuserinfo);
            this.pnlToolbar.Controls.Add(this.btnFilterCategory);
            this.pnlToolbar.Controls.Add(this.btnNew);
            this.pnlToolbar.Controls.Add(this.btnPopular);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1338, 67);
            this.pnlToolbar.TabIndex = 3;
            // 
            // btnFilterCategory
            // 
            this.btnFilterCategory.BackColor = System.Drawing.Color.Linen;
            this.btnFilterCategory.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnFilterCategory.Location = new System.Drawing.Point(3, 6);
            this.btnFilterCategory.Name = "btnFilterCategory";
            this.btnFilterCategory.Size = new System.Drawing.Size(221, 58);
            this.btnFilterCategory.TabIndex = 6;
            this.btnFilterCategory.Text = "分類篩選";
            this.btnFilterCategory.UseVisualStyleBackColor = false;
            this.btnFilterCategory.Click += new System.EventHandler(this.btnFilterCategory_Click);
            // 
            // lvBooks
            // 
            this.lvBooks.BackColor = System.Drawing.Color.AntiqueWhite;
            this.lvBooks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvBooks.HideSelection = false;
            this.lvBooks.Location = new System.Drawing.Point(230, 67);
            this.lvBooks.MultiSelect = false;
            this.lvBooks.Name = "lvBooks";
            this.lvBooks.Size = new System.Drawing.Size(748, 666);
            this.lvBooks.TabIndex = 0;
            this.lvBooks.UseCompatibleStateImageBehavior = false;
            this.lvBooks.SelectedIndexChanged += new System.EventHandler(this.lvBooks_SelectedIndexChanged);
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.Color.Bisque;
            this.pnlDetail.Controls.Add(this.wmp);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetail.Location = new System.Drawing.Point(984, 67);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(354, 666);
            this.pnlDetail.TabIndex = 4;
            // 
            // wmp
            // 
            this.wmp.Enabled = true;
            this.wmp.Location = new System.Drawing.Point(149, 325);
            this.wmp.Name = "wmp";
            this.wmp.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmp.OcxState")));
            this.wmp.Size = new System.Drawing.Size(193, 87);
            this.wmp.TabIndex = 6;
            this.wmp.Visible = false;
            // 
            // pnlBorrowList
            // 
            this.pnlBorrowList.BackColor = System.Drawing.Color.Bisque;
            this.pnlBorrowList.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBorrowList.Location = new System.Drawing.Point(0, 67);
            this.pnlBorrowList.Name = "pnlBorrowList";
            this.pnlBorrowList.Size = new System.Drawing.Size(224, 666);
            this.pnlBorrowList.TabIndex = 5;
            // 
            // lbuserinfo
            // 
            this.lbuserinfo.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbuserinfo.Location = new System.Drawing.Point(243, 20);
            this.lbuserinfo.Name = "lbuserinfo";
            this.lbuserinfo.Size = new System.Drawing.Size(235, 33);
            this.lbuserinfo.TabIndex = 7;
            this.lbuserinfo.Text = "歡迎，名字";
            // 
            // lbborrow_hmb
            // 
            this.lbborrow_hmb.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbborrow_hmb.Location = new System.Drawing.Point(496, 25);
            this.lbborrow_hmb.Name = "lbborrow_hmb";
            this.lbborrow_hmb.Size = new System.Drawing.Size(385, 25);
            this.lbborrow_hmb.TabIndex = 8;
            this.lbborrow_hmb.Text = "how many books is borrowed";
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Linen;
            this.btnQuit.Font = new System.Drawing.Font("微軟正黑體", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnQuit.Location = new System.Drawing.Point(887, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(91, 44);
            this.btnQuit.TabIndex = 9;
            this.btnQuit.Text = "登出";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1338, 733);
            this.Controls.Add(this.pnlBorrowList);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.lvBooks);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlToolbar.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wmp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnPopular;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.ListView lvBooks;
        private System.Windows.Forms.Button btnFilterCategory;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlBorrowList;
        private AxWMPLib.AxWindowsMediaPlayer wmp;
        private System.Windows.Forms.Label lbuserinfo;
        private System.Windows.Forms.Label lbborrow_hmb;
        private System.Windows.Forms.Button btnQuit;
    }
}

