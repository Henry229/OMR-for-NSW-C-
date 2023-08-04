using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSedu.OMR
{
    public partial class dlgFullSheet : Form
    {
        public List<MarkingSheet> results = null;

        public dlgFullSheet()
        {
            InitializeComponent();
        }

        private void dlgFullSheet_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < results.Count; i++)
            {
                MarkingSheet rslt = results[i];

                if ( i == 0 )
                {
                    grdFullSheet.ColumnCount = rslt.Scores.Count;
                }

                grdFullSheet.Rows.Add();

                for (int j = 0; j < rslt.Scores.Count; j++)
                {
                    if (i == 0)  // 첫줄은 컬럼세팅도 한다
                    {
                        grdFullSheet.Columns[j].Name = rslt.Scores[j].Subject + " Q." + rslt.Scores[j].QuestionNo;
                    }

                    grdFullSheet.Rows[i].Cells[j].Value = rslt.Scores[j].Answer;
                }
            }
        }
    }
}
