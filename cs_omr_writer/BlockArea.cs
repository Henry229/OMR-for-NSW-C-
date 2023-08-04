using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CSedu.OMR
{
    public partial class BlockArea : UserControl 
    {
        public BlockType areaBlockType = BlockType.ANSWER;
        public MarkingDirection areaMarkingDirection = MarkingDirection.H;
        public int NumofQuestion = 0;
        public int NumofSelection = 0;
        public int SelectionStartingNum = 0;
        public int StartingQuestionNum = 0;
        public string GroupName = "";
        public List<String> Items = new List<string>();
        public ControlHandler mover = null;
        public frmEditor parentForm = null;

        public BlockArea()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(100, Color.Blue);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void BlockArea_Leave(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void BlockArea_Click(object sender, EventArgs e)
        {
            this.parentForm.BlockAreaToControl(this);
        }

    }
}
