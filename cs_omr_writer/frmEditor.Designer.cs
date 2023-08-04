namespace CSedu.OMR
{
    partial class frmEditor
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
            CButtonLib.DesignerRectTracker designerRectTracker3 = new CButtonLib.DesignerRectTracker();
            CButtonLib.cBlendItems cBlendItems2 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker4 = new CButtonLib.DesignerRectTracker();
            CButtonLib.DesignerRectTracker designerRectTracker5 = new CButtonLib.DesignerRectTracker();
            CButtonLib.cBlendItems cBlendItems3 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker6 = new CButtonLib.DesignerRectTracker();
            CButtonLib.DesignerRectTracker designerRectTracker7 = new CButtonLib.DesignerRectTracker();
            CButtonLib.cBlendItems cBlendItems4 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker8 = new CButtonLib.DesignerRectTracker();
            CButtonLib.DesignerRectTracker designerRectTracker1 = new CButtonLib.DesignerRectTracker();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditor));
            CButtonLib.cBlendItems cBlendItems1 = new CButtonLib.cBlendItems();
            CButtonLib.DesignerRectTracker designerRectTracker2 = new CButtonLib.DesignerRectTracker();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlBlock = new System.Windows.Forms.Panel();
            this.btnLoadImage = new CButtonLib.CButton();
            this.btnAdd = new CButtonLib.CButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new CButtonLib.CButton();
            this.cmbTemplete = new System.Windows.Forms.ComboBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.numLeft = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.numTop = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSelectionItem = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.numCountOfSelection = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numCountOfQ = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numStartingQNo = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBlockType = new System.Windows.Forms.ComboBox();
            this.txtDecr = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.picbox = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSaveAll = new CButtonLib.CButton();
            this.pnlHeader.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountOfSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountOfQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartingQNo)).BeginInit();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnSaveAll);
            this.pnlHeader.Controls.Add(this.pnlBlock);
            this.pnlHeader.Controls.Add(this.btnLoadImage);
            this.pnlHeader.Controls.Add(this.btnAdd);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.cmbTemplete);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(958, 50);
            this.pnlHeader.TabIndex = 1;
            // 
            // pnlBlock
            // 
            this.pnlBlock.AllowDrop = true;
            this.pnlBlock.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pnlBlock.Location = new System.Drawing.Point(898, 9);
            this.pnlBlock.Name = "pnlBlock";
            this.pnlBlock.Size = new System.Drawing.Size(48, 35);
            this.pnlBlock.TabIndex = 22;
            this.pnlBlock.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlBlock_MouseDown);
            this.pnlBlock.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlBlock_MouseMove);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker3.IsActive = false;
            designerRectTracker3.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker3.TrackerRectangle")));
            this.btnLoadImage.CenterPtTracker = designerRectTracker3;
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
            this.btnLoadImage.ColorFillBlend = cBlendItems2;
            this.btnLoadImage.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnLoadImage.FocalPoints.CenterPtX = 1F;
            this.btnLoadImage.FocalPoints.CenterPtY = 0.625F;
            this.btnLoadImage.FocalPoints.FocusPtX = 0F;
            this.btnLoadImage.FocalPoints.FocusPtY = 0F;
            designerRectTracker4.IsActive = false;
            designerRectTracker4.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker4.TrackerRectangle")));
            this.btnLoadImage.FocusPtTracker = designerRectTracker4;
            this.btnLoadImage.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadImage.ForeColor = System.Drawing.Color.Black;
            this.btnLoadImage.ImageIndex = 0;
            this.btnLoadImage.Location = new System.Drawing.Point(437, 4);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(84, 40);
            this.btnLoadImage.TabIndex = 21;
            this.btnLoadImage.Text = "LOAD IMAGE";
            this.btnLoadImage.TextShadow = System.Drawing.Color.DimGray;
            this.btnLoadImage.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnLoadImage_ClickButtonArea);
            // 
            // btnAdd
            // 
            this.btnAdd.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker5.IsActive = false;
            designerRectTracker5.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker5.TrackerRectangle")));
            this.btnAdd.CenterPtTracker = designerRectTracker5;
            cBlendItems3.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems3.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnAdd.ColorFillBlend = cBlendItems3;
            this.btnAdd.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnAdd.FocalPoints.CenterPtX = 1F;
            this.btnAdd.FocalPoints.CenterPtY = 0.625F;
            this.btnAdd.FocalPoints.FocusPtX = 0F;
            this.btnAdd.FocalPoints.FocusPtY = 0F;
            designerRectTracker6.IsActive = false;
            designerRectTracker6.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker6.TrackerRectangle")));
            this.btnAdd.FocusPtTracker = designerRectTracker6;
            this.btnAdd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.ImageIndex = 0;
            this.btnAdd.Location = new System.Drawing.Point(527, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 40);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "ADD            NEW SHEET";
            this.btnAdd.TextShadow = System.Drawing.Color.DimGray;
            this.btnAdd.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnAdd_ClickButtonArea);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "SHEET TYPE";
            // 
            // btnSave
            // 
            this.btnSave.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker7.IsActive = false;
            designerRectTracker7.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker7.TrackerRectangle")));
            this.btnSave.CenterPtTracker = designerRectTracker7;
            cBlendItems4.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(69)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(80))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems4.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnSave.ColorFillBlend = cBlendItems4;
            this.btnSave.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnSave.FocalPoints.CenterPtX = 1F;
            this.btnSave.FocalPoints.CenterPtY = 0.625F;
            this.btnSave.FocalPoints.FocusPtX = 0F;
            this.btnSave.FocalPoints.FocusPtY = 0F;
            designerRectTracker8.IsActive = false;
            designerRectTracker8.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker8.TrackerRectangle")));
            this.btnSave.FocusPtTracker = designerRectTracker8;
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageIndex = 0;
            this.btnSave.Location = new System.Drawing.Point(216, 33);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 20);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "SAVE";
            this.btnSave.TextShadow = System.Drawing.Color.DimGray;
            this.btnSave.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnSave_ClickButtonArea);
            // 
            // cmbTemplete
            // 
            this.cmbTemplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTemplete.FormattingEnabled = true;
            this.cmbTemplete.Location = new System.Drawing.Point(133, 11);
            this.cmbTemplete.Name = "cmbTemplete";
            this.cmbTemplete.Size = new System.Drawing.Size(298, 28);
            this.cmbTemplete.TabIndex = 2;
            this.cmbTemplete.SelectionChangeCommitted += new System.EventHandler(this.cmbTemplete_SelectionChangeCommitted);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.numHeight);
            this.pnlLeft.Controls.Add(this.label17);
            this.pnlLeft.Controls.Add(this.numWidth);
            this.pnlLeft.Controls.Add(this.label14);
            this.pnlLeft.Controls.Add(this.btnSave);
            this.pnlLeft.Controls.Add(this.numLeft);
            this.pnlLeft.Controls.Add(this.label15);
            this.pnlLeft.Controls.Add(this.numTop);
            this.pnlLeft.Controls.Add(this.label16);
            this.pnlLeft.Controls.Add(this.label13);
            this.pnlLeft.Controls.Add(this.txtSelectionItem);
            this.pnlLeft.Controls.Add(this.label12);
            this.pnlLeft.Controls.Add(this.numCountOfSelection);
            this.pnlLeft.Controls.Add(this.label11);
            this.pnlLeft.Controls.Add(this.numCountOfQ);
            this.pnlLeft.Controls.Add(this.label10);
            this.pnlLeft.Controls.Add(this.numStartingQNo);
            this.pnlLeft.Controls.Add(this.label9);
            this.pnlLeft.Controls.Add(this.txtGroupName);
            this.pnlLeft.Controls.Add(this.label8);
            this.pnlLeft.Controls.Add(this.cmbDirection);
            this.pnlLeft.Controls.Add(this.label7);
            this.pnlLeft.Controls.Add(this.label6);
            this.pnlLeft.Controls.Add(this.label5);
            this.pnlLeft.Controls.Add(this.label4);
            this.pnlLeft.Controls.Add(this.cmbBlockType);
            this.pnlLeft.Controls.Add(this.txtDecr);
            this.pnlLeft.Controls.Add(this.txtCode);
            this.pnlLeft.Controls.Add(this.label2);
            this.pnlLeft.Controls.Add(this.label3);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 50);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(291, 633);
            this.pnlLeft.TabIndex = 2;
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(109, 410);
            this.numHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(94, 20);
            this.numHeight.TabIndex = 30;
            this.numHeight.ValueChanged += new System.EventHandler(this.blockProperties_DimensionChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 412);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 29;
            this.label17.Text = "Height";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(109, 384);
            this.numWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(94, 20);
            this.numWidth.TabIndex = 28;
            this.numWidth.ValueChanged += new System.EventHandler(this.blockProperties_DimensionChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 386);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "Width";
            // 
            // numLeft
            // 
            this.numLeft.Location = new System.Drawing.Point(109, 358);
            this.numLeft.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numLeft.Name = "numLeft";
            this.numLeft.Size = new System.Drawing.Size(94, 20);
            this.numLeft.TabIndex = 26;
            this.numLeft.ValueChanged += new System.EventHandler(this.blockProperties_DimensionChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 360);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "Left";
            // 
            // numTop
            // 
            this.numTop.Location = new System.Drawing.Point(109, 333);
            this.numTop.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTop.Name = "numTop";
            this.numTop.Size = new System.Drawing.Size(94, 20);
            this.numTop.TabIndex = 24;
            this.numTop.ValueChanged += new System.EventHandler(this.blockProperties_DimensionChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(13, 335);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(26, 13);
            this.label16.TabIndex = 23;
            this.label16.Text = "Top";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(19, 288);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 12);
            this.label13.TabIndex = 22;
            this.label13.Text = "(seperator \'|\')";
            // 
            // txtSelectionItem
            // 
            this.txtSelectionItem.Location = new System.Drawing.Point(109, 272);
            this.txtSelectionItem.Name = "txtSelectionItem";
            this.txtSelectionItem.Size = new System.Drawing.Size(176, 20);
            this.txtSelectionItem.TabIndex = 21;
            this.txtSelectionItem.TextChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 275);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Selection Items";
            // 
            // numCountOfSelection
            // 
            this.numCountOfSelection.Location = new System.Drawing.Point(109, 247);
            this.numCountOfSelection.Name = "numCountOfSelection";
            this.numCountOfSelection.Size = new System.Drawing.Size(94, 20);
            this.numCountOfSelection.TabIndex = 19;
            this.numCountOfSelection.ValueChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 249);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Count of Selection";
            // 
            // numCountOfQ
            // 
            this.numCountOfQ.Location = new System.Drawing.Point(109, 221);
            this.numCountOfQ.Name = "numCountOfQ";
            this.numCountOfQ.Size = new System.Drawing.Size(94, 20);
            this.numCountOfQ.TabIndex = 17;
            this.numCountOfQ.ValueChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 223);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Count of Question";
            // 
            // numStartingQNo
            // 
            this.numStartingQNo.Location = new System.Drawing.Point(109, 196);
            this.numStartingQNo.Name = "numStartingQNo";
            this.numStartingQNo.Size = new System.Drawing.Size(94, 20);
            this.numStartingQNo.TabIndex = 15;
            this.numStartingQNo.ValueChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Starting Q\' No";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(109, 170);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(94, 20);
            this.txtGroupName.TabIndex = 13;
            this.txtGroupName.TextChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 173);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Group Name";
            // 
            // cmbDirection
            // 
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Location = new System.Drawing.Point(109, 143);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(121, 21);
            this.cmbDirection.TabIndex = 11;
            this.cmbDirection.SelectedIndexChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Marking Direction";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Block Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 19);
            this.label5.TabIndex = 8;
            this.label5.Text = "BLOCK INFOMATION";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "SHEET INFOMATION";
            // 
            // cmbBlockType
            // 
            this.cmbBlockType.FormattingEnabled = true;
            this.cmbBlockType.Location = new System.Drawing.Point(109, 116);
            this.cmbBlockType.Name = "cmbBlockType";
            this.cmbBlockType.Size = new System.Drawing.Size(121, 21);
            this.cmbBlockType.TabIndex = 6;
            this.cmbBlockType.SelectedIndexChanged += new System.EventHandler(this.blockProperties_ValueChanged);
            // 
            // txtDecr
            // 
            this.txtDecr.Location = new System.Drawing.Point(76, 56);
            this.txtDecr.Name = "txtDecr";
            this.txtDecr.Size = new System.Drawing.Size(199, 20);
            this.txtDecr.TabIndex = 5;
            this.txtDecr.TextChanged += new System.EventHandler(this.txtDecr_TextChanged);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(76, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(94, 20);
            this.txtCode.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CODE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description";
            // 
            // pnlBody
            // 
            this.pnlBody.AllowDrop = true;
            this.pnlBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBody.AutoScroll = true;
            this.pnlBody.Controls.Add(this.picbox);
            this.pnlBody.Location = new System.Drawing.Point(291, 50);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(667, 633);
            this.pnlBody.TabIndex = 3;
            this.pnlBody.DragDrop += new System.Windows.Forms.DragEventHandler(this.picbox_DragDrop);
            this.pnlBody.DragEnter += new System.Windows.Forms.DragEventHandler(this.picbox_DragEnter);
            // 
            // picbox
            // 
            this.picbox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picbox.Location = new System.Drawing.Point(6, 6);
            this.picbox.Name = "picbox";
            this.picbox.Size = new System.Drawing.Size(658, 624);
            this.picbox.TabIndex = 0;
            this.picbox.TabStop = false;
            this.picbox.DragDrop += new System.Windows.Forms.DragEventHandler(this.picbox_DragDrop);
            this.picbox.DragEnter += new System.Windows.Forms.DragEventHandler(this.picbox_DragEnter);
            this.picbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_MouseDown);
            this.picbox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_MouseMove);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.BorderColor = System.Drawing.Color.DarkGray;
            designerRectTracker1.IsActive = false;
            designerRectTracker1.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker1.TrackerRectangle")));
            this.btnSaveAll.CenterPtTracker = designerRectTracker1;
            cBlendItems1.iColor = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(69)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(80))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))))};
            cBlendItems1.iPoint = new float[] {
        0F,
        0.2191358F,
        0.8487654F,
        1F};
            this.btnSaveAll.ColorFillBlend = cBlendItems1;
            this.btnSaveAll.ColorFillSolid = System.Drawing.Color.DimGray;
            this.btnSaveAll.FocalPoints.CenterPtX = 1F;
            this.btnSaveAll.FocalPoints.CenterPtY = 0.625F;
            this.btnSaveAll.FocalPoints.FocusPtX = 0F;
            this.btnSaveAll.FocalPoints.FocusPtY = 0F;
            designerRectTracker2.IsActive = false;
            designerRectTracker2.TrackerRectangle = ((System.Drawing.RectangleF)(resources.GetObject("designerRectTracker2.TrackerRectangle")));
            this.btnSaveAll.FocusPtTracker = designerRectTracker2;
            this.btnSaveAll.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAll.ImageIndex = 0;
            this.btnSaveAll.Location = new System.Drawing.Point(633, 4);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(79, 40);
            this.btnSaveAll.TabIndex = 31;
            this.btnSaveAll.Text = "SAVE";
            this.btnSaveAll.TextShadow = System.Drawing.Color.DimGray;
            this.btnSaveAll.ClickButtonArea += new CButtonLib.CButton.ClickButtonAreaEventHandler(this.btnSaveAll_ClickButtonArea);
            // 
            // frmEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 683);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlHeader);
            this.Name = "frmEditor";
            this.Text = "frmEditor";
            this.Load += new System.EventHandler(this.frmEditor_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountOfSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCountOfQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartingQNo)).EndInit();
            this.pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTemplete;
        private System.Windows.Forms.Panel pnlLeft;
        internal CButtonLib.CButton btnAdd;
        internal CButtonLib.CButton btnSave;
        private System.Windows.Forms.TextBox txtDecr;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlBody;
        internal CButtonLib.CButton btnLoadImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox picbox;
        private System.Windows.Forms.Panel pnlBlock;
        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBlockType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSelectionItem;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numCountOfSelection;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numCountOfQ;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numStartingQNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numLeft;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numTop;
        private System.Windows.Forms.Label label16;
        internal CButtonLib.CButton btnSaveAll;
    }
}