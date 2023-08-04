using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OMR;


namespace CSedu.OMR
{
    public partial class frmEditor : Form
    {
        List<Template> templetes = null;                    // OMR sheet 유형을 저장.
        Point mouseStart = new Point();

        bool isDrag = false;
        bool isLoading = false;

        
        ControlHandler mover;

        private void LoadXML()
        {
            templetes = TemplateManager.GetTemplates("sheets.xml");
            this.cmbTemplete.Items.Clear();

            foreach (Template t in templetes)
            {
                this.cmbTemplete.Items.Add(t.description);
            }

            this.cmbTemplete.SelectedIndex = 0;
        }

        private void BindXmlSheetToControl(int index)
        {
            Template tmplt =  this.templetes[index];
            this.txtCode.Text = tmplt.code;
            this.txtDecr.Text = tmplt.description;
            this.LoadBlocks(tmplt);
        }

        private void SaveBlockToFile()
        {
            TemplateManager.SaveTemplates("sheet.xml", this.templetes, new List<string>());
        }

        private void SaveBlock()
        {
            Template tmplt = new Template();

            tmplt.actualHeight = this.picbox.Height;
            tmplt.actualWidth = this.picbox.Width;
            tmplt.code = this.txtCode.Text;
            tmplt.description = this.txtDecr.Text;
            tmplt.numofBlocks = this.picbox.Controls.Count;
            tmplt.sheetSize = SheetSize.A4;

            tmplt.Blocks = new List<Block>();

            for (int i = 0; i < tmplt.numofBlocks; i++)
            {
                BlockArea area = (BlockArea)this.picbox.Controls[i];
                Block bl = new Block();
                bl.Type = area.areaBlockType;
                bl.X = area.Left;
                bl.Y = area.Top;
                bl.Width = area.Width;
                bl.Height = area.Height;
                bl.numberofQuestions = area.NumofQuestion;
                bl.numberofSelections = area.NumofSelection;
                bl.startingNumberofAnswer = area.SelectionStartingNum;
                bl.startingQuestion = area.StartingQuestionNum;
                bl.Direction = area.areaMarkingDirection;
                bl.Group = area.GroupName;
                bl.items = area.Items;
            }

            tmplt.tag = "Y";

            this.templetes[cmbTemplete.SelectedIndex] = tmplt;
        }

        private void LoadBlocks(Template tmplt)
        {
            if (picbox.BackgroundImage == null)
                return;

            mover.deselectAll();
            mover.boundControls.Clear();
            this.picbox.Controls.Clear();

            for (int i = 0; i < tmplt.Blocks.Count; i++)
            {
                BlockArea block = new BlockArea();

                block.Location = new Point( 
                    tmplt.Blocks[i].X
                    , tmplt.Blocks[i].Y 
                    ); 
                block.Size = new Size(
                    tmplt.Blocks[i].Width 
                    , tmplt.Blocks[i].Height
                    );
                block.areaBlockType = tmplt.Blocks[i].Type;
                block.areaMarkingDirection = tmplt.Blocks[i].Direction;
                block.GroupName = tmplt.Blocks[i].Group;
                block.StartingQuestionNum = tmplt.Blocks[i].startingQuestion;
                block.Items = tmplt.Blocks[i].items;
                block.NumofQuestion = tmplt.Blocks[i].numberofQuestions;
                block.NumofSelection = tmplt.Blocks[i].numberofSelections;

                block.mover = this.mover;
                block.parentForm = this;

                //block.Click += block_Click;

                picbox.Controls.Add(block);

                mover.AddBoundControl(this.picbox, block);
                mover.bindControls();

                mover.deselectAll();
            }
        }

        public void BlockAreaToControl(BlockArea area)
        {
            this.isLoading = true;

            this.cmbBlockType.Text = area.areaBlockType.ToString();
            this.cmbDirection.Text = area.areaMarkingDirection.ToString();
            this.txtGroupName.Text = area.GroupName;

            this.numStartingQNo.Value = area.StartingQuestionNum;
            this.numCountOfSelection.Value = area.NumofSelection;
            this.numCountOfQ.Value = area.NumofQuestion;

            this.txtSelectionItem.Text = "";
            for (int i =0; i < area.Items.Count; i++)
            {
                this.txtSelectionItem.Text += area.Items[i].ToString()
                    + (( i == area.Items.Count - 1 ) ? "" : "|");
            }

            this.isLoading = false;
        }

        public frmEditor()
        {
            InitializeComponent();
            mover = new ControlHandler();
            mover.parentForm = this;
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            isLoading = true;

            string[] blocktype = Enum.GetNames(typeof(BlockType));
            for (int i = 0; i < blocktype.Length; i++)
            {
                this.cmbBlockType.Items.Add(blocktype[i]);
            }

            string[] direction = Enum.GetNames(typeof(MarkingDirection));
            for (int i = 0; i < direction.Length; i++)
            {
                this.cmbDirection.Items.Add(direction[i]);
            }

            LoadXML();
            BindXmlSheetToControl(0);

            isLoading = false;
            //picbox.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void btnLoadImage_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            this.openFileDialog1.Filter = "JPEG|*.jpg";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bit = (Bitmap)System.Drawing.Image.FromFile(openFileDialog1.FileName);

                OpticalReader reader = new OpticalReader();
                Template tmplt = templetes[cmbTemplete.SelectedIndex];

                MarkingErrorType err = MarkingErrorType.NOT_PROCESSED;

                bit = reader.ExtractOMRSheet(bit, tmplt.backgroundFill
                                                , tmplt.startingContrast, tmplt, ref err);

                this.picbox.Tag = bit;

                Image thumbNail = new Bitmap((Bitmap)this.picbox.Tag, tmplt.actualWidth, tmplt.actualHeight);
                this.picbox.BackgroundImage = thumbNail;
                this.picbox.Width = thumbNail.Width;
                this.picbox.Height = thumbNail.Height;

                this.LoadBlocks(tmplt);
            }
        }


        /// <summary>
        /// 이미지영역에 마우스클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseStart = new Point(e.X, e.Y);
            mover.deselectAll();

        }

        /// <summary>
        /// 이미지영역에서 마우스 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_MouseMove(object sender, MouseEventArgs e)
        {

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    int deltaX = this.mouseStart.X - e.X;
                    int deltaY = this.mouseStart.Y - e.Y;

                    this.pnlBody.AutoScrollPosition =
                        new Point(deltaX - pnlBody.AutoScrollPosition.X, deltaY - pnlBody.AutoScrollPosition.Y);
                }

        }

        private void pnlBlock_MouseDown(object sender, MouseEventArgs e)
        {
            this.isDrag = true;
        }

        private void pnlBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                pnlBlock.DoDragDrop("Block", DragDropEffects.Copy);
            }

            isDrag = false;
        }

        private void picbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void picbox_DragDrop(object sender, DragEventArgs e)
        {

            mover.deselectAll();

            BlockArea block = new BlockArea();

            block.Location =  picbox.PointToClient(new Point(e.X, e.Y));
            block.Size = new Size(100, 100);
            block.mover = this.mover;
            block.parentForm = this;

            //block.Click += block_Click;

            picbox.Controls.Add(block);

            mover.AddBoundControl(this.picbox, block);
            mover.bindControls();

        }


        private void blockProperties_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {

                Control ctrl = (Control)sender;
                BlockArea block = (BlockArea)mover.ActiveControl;

                if (block == null)
                    return;

                switch (ctrl.Name)
                {
                    case "cmbBlockType":
                        block.areaBlockType = (BlockType)Enum.Parse(typeof(BlockType), this.cmbBlockType.Text.ToString());
                        break;
                    case "cmbDirection":
                        block.areaMarkingDirection = (MarkingDirection)Enum.Parse(typeof(MarkingDirection), this.cmbDirection.Text.ToString());
                        break;
                    case "txtGroupName":
                        block.GroupName = txtGroupName.Text;
                        break;
                    case "numStartingQNo":
                        block.StartingQuestionNum = (int)this.numStartingQNo.Value;
                        break;
                    case "numCountOfQ":
                        block.NumofQuestion = (int)this.numCountOfQ.Value;
                        break;
                    case "numCountOfSelection":
                        block.NumofSelection = (int)this.numCountOfSelection.Value;
                        break;
                    case "txtSelectionItem":
                        block.Items.Clear();

                        string[] items = this.txtSelectionItem.Text.Split('|');

                        for (int i = 0; i < items.Length; i++)
                        {
                            block.Items.Add(items[i]);
                        }

                        break;
                }
            }
            else
            {
                txtSelectionItem.Update();
                numCountOfQ.Update();
                numCountOfSelection.Update();
                numStartingQNo.Update();
                txtGroupName.Update();
            }


        }

        private void blockProperties_DimensionChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {

                Control ctrl = (Control)sender;
                BlockArea block = (BlockArea)mover.ActiveControl;

                if (block == null)
                    return;

                switch (ctrl.Name)
                {
                    case "numTop":
                        block.Top = (int)this.numTop.Value;
                        break;
                    case "numLeft":
                        block.Left = (int)this.numLeft.Value;
                        break;
                    case "numWidth":
                        block.Width = (int)this.numWidth.Value;
                        break;
                    case "numHeight":
                        block.Height = (int)this.numHeight.Value;
                        break;
                }
            }

        }

        public void DimensionChanged()
        {
            this.isLoading = true;

            BlockArea block = (BlockArea)mover.ActiveControl;

            if (block == null)
                return;
            numTop.Value = block.Top;
            numLeft.Value = block.Left;
            numWidth.Value = block.Width;
            numHeight.Value = block.Height;
            numTop.Update();
            numLeft.Update();
            numHeight.Update();
            numWidth.Update();

            this.isLoading = false;

        }

        private void btnAdd_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            Template tmplt = new Template();
            tmplt.code = "CODE_HERE";
            tmplt.description = "Description Here";
            tmplt.Blocks = new List<Block>();

            this.templetes.Add(tmplt);

            cmbTemplete.Items.Add(tmplt.description);
            cmbTemplete.SelectedIndex = cmbTemplete.Items.Count - 1;

            this.txtCode.Text = tmplt.code;
            this.txtDecr.Text = tmplt.description;
            this.LoadBlocks(tmplt);
        }

        private void btnSave_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            SaveBlock();
        }

        private void txtDecr_TextChanged(object sender, EventArgs e)
        {
            int sel_index = this.cmbTemplete.SelectedIndex;
            this.cmbTemplete.Items.RemoveAt(sel_index);
            this.cmbTemplete.Items.Insert(sel_index, txtDecr.Text);
            this.cmbTemplete.SelectedIndex = sel_index;
        }

        private void cmbTemplete_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.txtCode.Text = templetes[cmbTemplete.SelectedIndex].code;
            this.txtDecr.Text = templetes[cmbTemplete.SelectedIndex].description;
            this.LoadBlocks(templetes[cmbTemplete.SelectedIndex]);
        }

        private void btnSaveAll_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            SaveBlockToFile();
        }







    }
}
