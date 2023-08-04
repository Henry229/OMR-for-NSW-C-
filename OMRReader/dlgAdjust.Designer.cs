namespace CSedu.OMR
{
    partial class dlgAdjust
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgAdjust));
            CButtonLib.cBlendItems cBlendItems1 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker2 = new CButtonLib.DesignerRectTracker();
            CButtonLib.DesignerRectTracker designerRectTracker3 = new CButtonLib.DesignerRectTracker();
            CButtonLib.cBlendItems cBlendItems2 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker4 = new CButtonLib.DesignerRectTracker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numBlackRatio = new System.Windows.Forms.NumericUpDown();
            this.numMultiCheck = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numNotFillCheck = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new CButtonLib.CButton();
            this.btnCancel = new CButtonLib.CButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numBlackRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNotFillCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "BLACK PEN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "PENCIL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Black Ratio";
            // 
            // numBlackRatio
            // 
            this.numBlackRatio.DecimalPlaces = 3;
            this.numBlackRatio.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBlackRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numBlackRatio.Location = new System.Drawing.Point(127, 49);
            this.numBlackRatio.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            196608});
            this.numBlackRatio.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            196608});
            this.numBlackRatio.Name = "numBlackRatio";
            this.numBlackRatio.Size = new System.Drawing.Size(74, 20);
            this.numBlackRatio.TabIndex = 7;
            this.numBlackRatio.Value = new decimal(new int[] {
            45,
            0,
            0,
            196608});
            // 
            // numMultiCheck
            // 
            this.numMultiCheck.DecimalPlaces = 1;
            this.numMultiCheck.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numMultiCheck.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numMultiCheck.Location = new System.Drawing.Point(127, 131);
            this.numMultiCheck.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.numMultiCheck.Name = "numMultiCheck";
            this.numMultiCheck.Size = new System.Drawing.Size(74, 20);
            this.numMultiCheck.TabIndex = 9;
            this.numMultiCheck.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Multi Check";
            // 
            // numNotFillCheck
            // 
            this.numNotFillCheck.DecimalPlaces = 1;
            this.numNotFillCheck.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numNotFillCheck.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numNotFillCheck.Location = new System.Drawing.Point(127, 157);
            this.numNotFillCheck.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            this.numNotFillCheck.Name = "numNotFillCheck";
            this.numNotFillCheck.Size = new System.Drawing.Size(74, 20);
            this.numNotFillCheck.TabIndex = 11;
            this.numNotFillCheck.Value = new decimal(new int[] {
            17,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 14);
            this.label5.TabIndex = 10;
            this.label5.Text = "NotFill Check";
            // 
            // btnSave
            // 
            this.btnSave.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker1.IsActive = false;
            designerRectTracker1.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker1.TrackerRectangle")));
            this.btnSave.CenterPtTracker = designerRectTracker1;
            cBlendItems1.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems1.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnSave.ColorFillBlend = cBlendItems1;
            this.btnSave.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnSave.FocalPoints.CenterPtX = 1F;
            this.btnSave.FocalPoints.CenterPtY = 0.625F;
            this.btnSave.FocalPoints.FocusPtX = 0F;
            this.btnSave.FocalPoints.FocusPtY = 0F;
            designerRectTracker2.IsActive = false;
            designerRectTracker2.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker2.TrackerRectangle")));
            this.btnSave.FocusPtTracker = designerRectTracker2;
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.ImageIndex = 0;
            this.btnSave.Location = new System.Drawing.Point(90, 189);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 26);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "SAVE";
            this.btnSave.TextShadow = System.Drawing.Color.DimGray;
            this.btnSave.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnSave_ClickButtonArea);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker3.IsActive = false;
            designerRectTracker3.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker3.TrackerRectangle")));
            this.btnCancel.CenterPtTracker = designerRectTracker3;
            cBlendItems2.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems2.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnCancel.ColorFillBlend = cBlendItems2;
            this.btnCancel.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnCancel.FocalPoints.CenterPtX = 1F;
            this.btnCancel.FocalPoints.CenterPtY = 0.625F;
            this.btnCancel.FocalPoints.FocusPtX = 0F;
            this.btnCancel.FocalPoints.FocusPtY = 0F;
            designerRectTracker4.IsActive = false;
            designerRectTracker4.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker4.TrackerRectangle")));
            this.btnCancel.FocusPtTracker = designerRectTracker4;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageIndex = 0;
            this.btnCancel.Location = new System.Drawing.Point(148, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 26);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextShadow = System.Drawing.Color.DimGray;
            this.btnCancel.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnCancel_ClickButtonArea);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 14);
            this.label6.TabIndex = 26;
            this.label6.Text = "This setting is for specific condition";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(210, 14);
            this.label7.TabIndex = 27;
            this.label7.Text = "Don\'t change values under normal condition";
            // 
            // dlgAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 264);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numNotFillCheck);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numMultiCheck);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numBlackRatio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "dlgAdjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setting Properties";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numBlackRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNotFillCheck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBlackRatio;
        private System.Windows.Forms.NumericUpDown numMultiCheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numNotFillCheck;
        private System.Windows.Forms.Label label5;
        internal CButtonLib.CButton btnSave;
        internal CButtonLib.CButton btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}