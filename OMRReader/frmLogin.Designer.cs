namespace CSedu.OMR
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            CButtonLib.DesignerRectTracker designerRectTracker1 = new CButtonLib.DesignerRectTracker();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            CButtonLib.cBlendItems cBlendItems1 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker2 = new CButtonLib.DesignerRectTracker();
            CButtonLib.DesignerRectTracker designerRectTracker3 = new CButtonLib.DesignerRectTracker();
            CButtonLib.cBlendItems cBlendItems2 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker4 = new CButtonLib.DesignerRectTracker();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEncrypt = new System.Windows.Forms.TextBox();
            this.txtDecrypt = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnLogin = new CButtonLib.CButton();
            this.btnExit = new CButtonLib.CButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(205, 218);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(270, 23);
            this.txtID.TabIndex = 4;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(205, 283);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(270, 23);
            this.txtPassword.TabIndex = 5;
            // 
            // txtEncrypt
            // 
            this.txtEncrypt.Location = new System.Drawing.Point(179, 458);
            this.txtEncrypt.Name = "txtEncrypt";
            this.txtEncrypt.Size = new System.Drawing.Size(323, 22);
            this.txtEncrypt.TabIndex = 8;
            // 
            // txtDecrypt
            // 
            this.txtDecrypt.Location = new System.Drawing.Point(179, 484);
            this.txtDecrypt.Name = "txtDecrypt";
            this.txtDecrypt.Size = new System.Drawing.Size(323, 22);
            this.txtDecrypt.TabIndex = 9;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(348, 510);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 10;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(429, 510);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Decrypt";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BorderColor = System.Drawing.Color.Black;
            this.btnLogin.BorderShow = false;
            designerRectTracker1.IsActive = false;
            designerRectTracker1.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker1.TrackerRectangle")));
            this.btnLogin.CenterPtTracker = designerRectTracker1;
            cBlendItems1.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems1.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnLogin.ColorFillBlend = cBlendItems1;
            this.btnLogin.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnLogin.FocalPoints.CenterPtX = 1F;
            this.btnLogin.FocalPoints.CenterPtY = 0.625F;
            this.btnLogin.FocalPoints.FocusPtX = 0F;
            this.btnLogin.FocalPoints.FocusPtY = 0F;
            designerRectTracker2.IsActive = false;
            designerRectTracker2.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker2.TrackerRectangle")));
            this.btnLogin.FocusPtTracker = designerRectTracker2;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ImageIndex = 0;
            this.btnLogin.Location = new System.Drawing.Point(511, 231);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Shape = CButtonLib.CButton.eShape.Ellipse;
            this.btnLogin.Size = new System.Drawing.Size(64, 65);
            this.btnLogin.TabIndex = 22;
            this.btnLogin.Text = "OK";
            this.btnLogin.TextShadow = System.Drawing.Color.DimGray;
            this.btnLogin.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnLogin_ClickButtonArea);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BorderColor = System.Drawing.Color.Black;
            this.btnExit.BorderShow = false;
            designerRectTracker3.IsActive = false;
            designerRectTracker3.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker3.TrackerRectangle")));
            this.btnExit.CenterPtTracker = designerRectTracker3;
            cBlendItems2.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(80))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(69)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems2.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnExit.ColorFillBlend = cBlendItems2;
            this.btnExit.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnExit.FocalPoints.CenterPtX = 1F;
            this.btnExit.FocalPoints.CenterPtY = 0.625F;
            this.btnExit.FocalPoints.FocusPtX = 0F;
            this.btnExit.FocalPoints.FocusPtY = 0F;
            designerRectTracker4.IsActive = false;
            designerRectTracker4.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker4.TrackerRectangle")));
            this.btnExit.FocusPtTracker = designerRectTracker4;
            this.btnExit.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ImageIndex = 0;
            this.btnExit.Location = new System.Drawing.Point(608, 231);
            this.btnExit.Name = "btnExit";
            this.btnExit.Shape = CButtonLib.CButton.eShape.Ellipse;
            this.btnExit.Size = new System.Drawing.Size(64, 65);
            this.btnExit.TabIndex = 23;
            this.btnExit.Text = "Exit";
            this.btnExit.TextShadow = System.Drawing.Color.DimGray;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(709, 408);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "...";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(729, 419);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.txtDecrypt);
            this.Controls.Add(this.txtEncrypt);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtID);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogin";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmLogin_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEncrypt;
        private System.Windows.Forms.TextBox txtDecrypt;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button button1;
        internal CButtonLib.CButton btnLogin;
        internal CButtonLib.CButton btnExit;
        private System.Windows.Forms.Label label1;
    }
}