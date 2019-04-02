namespace PhotoFinder
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearchPhoto = new System.Windows.Forms.Button();
            this.photoListView = new System.Windows.Forms.ListView();
            this.tbKeyword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearchPhoto
            // 
            this.btnSearchPhoto.Location = new System.Drawing.Point(456, 13);
            this.btnSearchPhoto.Name = "btnSearchPhoto";
            this.btnSearchPhoto.Size = new System.Drawing.Size(98, 39);
            this.btnSearchPhoto.TabIndex = 0;
            this.btnSearchPhoto.Text = "사진 검색";
            this.btnSearchPhoto.UseVisualStyleBackColor = true;
            this.btnSearchPhoto.Click += new System.EventHandler(this.btnSearchPhoto_Click);
            // 
            // photoListView
            // 
            this.photoListView.Location = new System.Drawing.Point(30, 66);
            this.photoListView.Name = "photoListView";
            this.photoListView.Size = new System.Drawing.Size(773, 488);
            this.photoListView.TabIndex = 1;
            this.photoListView.UseCompatibleStateImageBehavior = false;
            this.photoListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.photoListView_MouseClick);
            // 
            // tbKeyword
            // 
            this.tbKeyword.Location = new System.Drawing.Point(249, 22);
            this.tbKeyword.Name = "tbKeyword";
            this.tbKeyword.Size = new System.Drawing.Size(189, 21);
            this.tbKeyword.TabIndex = 2;
            this.tbKeyword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbKeyword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "찾고 싶은 사진의 키워드를 입력해주세요 :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbKeyword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSearchPhoto);
            this.groupBox1.Location = new System.Drawing.Point(30, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(567, 57);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "검색";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 568);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.photoListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PhotoFinder";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearchPhoto;
        private System.Windows.Forms.ListView photoListView;
        private System.Windows.Forms.TextBox tbKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

