using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSedu.OMR
{
    public partial class dlgAdjust : Form
    {
        public decimal decBlackRatio
        {
            get { return this.numBlackRatio.Value; }
            set { this.numBlackRatio.Value = value; }
        }

        public decimal decMultiCheck
        {
            get { return this.numMultiCheck.Value; }
            set { this.numMultiCheck.Value = value; }
        }

        public decimal decNotFillCheck
        {
            get { return this.numNotFillCheck.Value; }
            set { this.numNotFillCheck.Value = value; }
        }


        public dlgAdjust()
        {
            InitializeComponent();
        }

        private void btnCancel_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
