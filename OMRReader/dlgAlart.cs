using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSedu.OMR
{
    public partial class dlgAlart : Form
    {
        public dlgAlart()
        {
            InitializeComponent();
        }

        private void dlgAlart_Load(object sender, EventArgs e)
        {

        }

        public void ShowDialog(string strTitle, string strMsg)
        {
            this.Text = strTitle;
            this.txtMsg.Text = strMsg;

            this.ShowDialog();
        }

        private void btnLogin_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
