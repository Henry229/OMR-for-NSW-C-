namespace CSedu.OMR
{
    partial class dlgAlart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgAlart));
            CButtonLib.cBlendItems cBlendItems1 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker2 = new CButtonLib.DesignerRectTracker();
            this.btnOK = new CButtonLib.CButton();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BorderColor = System.Drawing.Color.Black;
            designerRectTracker1.IsActive = false;
            designerRectTracker1.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker1.TrackerRectangle")));
            this.btnOK.CenterPtTracker = designerRectTracker1;
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
            this.btnOK.ColorFillBlend = cBlendItems1;
            this.btnOK.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnOK.FocalPoints.CenterPtX = 1F;
            this.btnOK.FocalPoints.CenterPtY = 0.625F;
            this.btnOK.FocalPoints.FocusPtX = 0F;
            this.btnOK.FocalPoints.FocusPtY = 0F;
            designerRectTracker2.IsActive = false;
            designerRectTracker2.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker2.TrackerRectangle")));
            this.btnOK.FocusPtTracker = designerRectTracker2;
            this.btnOK.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ImageIndex = 0;
            this.btnOK.Location = new System.Drawing.Point(265, 150);
            this.btnOK.Name = "btnOK";
            this.btnOK.Shape = CButtonLib.CButton.eShape.Ellipse;
            this.btnOK.Size = new System.Drawing.Size(58, 40);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "OK";
            this.btnOK.TextShadow = System.Drawing.Color.DimGray;
            this.btnOK.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnLogin_ClickButtonArea);
            // 
            // txtMsg
            // 
            this.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsg.Location = new System.Drawing.Point(25, 34);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtMsg.Size = new System.Drawing.Size(298, 110);
            this.txtMsg.TabIndex = 26;
            this.txtMsg.Text = "";
            // 
            // dlgAlart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 204);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgAlart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alert";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.dlgAlart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal CButtonLib.CButton btnOK;
        private System.Windows.Forms.RichTextBox txtMsg;
    }
}