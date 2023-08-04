using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Resources;

namespace CSedu.OMR
{
    public partial class pnlDetailScore : UserControl
    {
        BindingSource scorebindingsource = new BindingSource();
        private MarkingSheet markingSheet = null;
        private int sheetIndex = -1;
        private string templeteType = "";
        private DataSet students = null;

        ResourceManager res_man;
        CultureInfo cult;

        /// <summary>
        /// 로드시 MarkingSheet의 정보로 컨트롤에 값을 넣어줌
        /// </summary>
        /// <param name="rslt">호출할 Marking sheet</param>
        /// <param name="index">해당시트의 index</param>
        /// <param name="tmpltype">템플릿유형 - Writing인지 아닌지</param>
        public void bindData(MarkingSheet rslt, int index, string tmpltype)
        {
            //TODO : data load to grdScore
            if (index >= 0 && rslt.Error != MarkingErrorType.BAD_IMAGE
                && rslt.Error != MarkingErrorType.NOT_PROCESSED
                && rslt.Error != MarkingErrorType.LOW_MEMORY
                )
            {
                this.markingSheet = rslt;
                this.sheetIndex = index;
                this.templeteType = tmpltype;
                string msg = @"There is/are one or more existing data. Do you want to delete?
(If No, Data will be appended)"; // res_man.GetString("Msg02", cult); 

                if (this.markingSheet.Error == MarkingErrorType.SEND_OK)
                {
                    MessageBox.Show(msg);
                }

                this.txtBranchNo.Text = markingSheet.BranchNo;
                this.txtStudentNo.Text = markingSheet.StudentID;
                if (this.templeteType == "WRITING")
                {
                    this.txtTotalScore.Text = markingSheet.totalScore;
                    this.txtTotalScore.Visible = false;    // 종합점수는 보여주지 않기로..
                    this.chbSubject.Visible = false;
                    this.label3.Visible = false;
                }
                else
                {
                    for (int i = 0; i < chbSubject.Items.Count; i++)
                    {
                        chbSubject.SetItemChecked(i, false);

                        for (int j = 0; j < markingSheet.TestSubs.Count; j++)
                        {
                            if (chbSubject.Items[i].ToString().ToUpper() == markingSheet.TestSubs[j].ToUpper())
                            {
                                chbSubject.SetItemChecked(i, true);
                            }
                        }
                    }

                    this.txtTotalScore.Visible = false;
                    this.chbSubject.Visible = true;
                    this.label3.Visible = true;
                    this.label3.Text = "Subjects";
                }

                this.cmbLevel.Text = markingSheet.TestLevel;
                this.txtTestNo.Text = markingSheet.TestNo;
                this.cmbTestType.Text = markingSheet.TestType;

                List<Score> temp = new List<Score>();

                for (int i = 0; i < this.markingSheet.Scores.Count; i++)
                {

                    Score temp2 = new Score();
                    temp2.Subject = this.markingSheet.Scores[i].Subject;
                    temp2.QuestionNo = this.markingSheet.Scores[i].QuestionNo;
                    temp2.Answer = this.markingSheet.Scores[i].Answer;
                    temp2.area = this.markingSheet.Scores[i].area;
                    temp2.selection = this.markingSheet.Scores[i].selection;
                    temp2.blockType = markingSheet.Scores[i].blockType;

                    temp.Add(temp2);
                }

                scorebindingsource.DataSource = temp;
                scorebindingsource.ResetBindings(true);

                this.BringToFront();
                this.Visible = true;
            }

        }


        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        /// <param name="studs">학생리스트정보</param>
        public void init_Control( DataSet studs) 
        {
            this.gridScore.DataSource = scorebindingsource;

            string[] testlevel1 = Enum.GetNames(typeof(TestLevel1));
            for (int i = 0; i < testlevel1.Length; i++)
            {
                this.cmbLevel.Items.Add(testlevel1[i]);
            }

            string[] testlevel2 = Enum.GetNames(typeof(TestLevel2));
            for (int i = 0; i < testlevel2.Length; i++)
            {
                this.cmbLevel.Items.Add(testlevel2[i]);
            }

            string[] testtype = Enum.GetNames(typeof(TestType));
            for (int i = 0; i < testtype.Length; i++)
            {
                this.cmbTestType.Items.Add(testtype[i]);
            }

            string[] testsubject = Enum.GetNames(typeof(TestSubject));
            for (int i = 0; i < testsubject.Length; i++)
            {
                this.chbSubject.Items.Add(testsubject[i].Replace('_', ' ') );
            }

            this.students = studs;

            cult = CultureInfo.CurrentCulture;

            res_man = new ResourceManager("CSedu.OMR.resource.msg", typeof(frmLogin).Assembly);
        }


        public pnlDetailScore()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 데이터 바이드 할때, Value에 따라 셀색상등 변경해줌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridScore_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in this.gridScore.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[2].Value.ToString() == "")
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                }
                else if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Length > 1)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        /// <summary>
        /// 셀을 변경할때, 이미지 영역에 마킹한 이미지를 보여줌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridScore_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.picScore.Image = ((List<Score>)scorebindingsource.DataSource)[e.RowIndex].area;
        }


        /// <summary>
        /// 수정완료후 OK 눌렀을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOKCorrection_Click(object sender, EventArgs e)
        {
            // TODO : data save to resultset & clear grdScore

            List<Score> scores = (List<Score>)this.scorebindingsource.DataSource;

            this.markingSheet.StudentID = this.txtStudentNo.Text;
            this.markingSheet.TestType = this.cmbTestType.SelectedIndex < 0 ? "" : this.cmbTestType.Items[this.cmbTestType.SelectedIndex].ToString();
            this.markingSheet.TestLevel = this.cmbLevel.SelectedIndex < 0 ? "" : this.cmbLevel.Items[this.cmbLevel.SelectedIndex].ToString();
            this.markingSheet.TestNo = this.txtTestNo.Text;
            this.markingSheet.BranchNo = this.txtBranchNo.Text;
            this.markingSheet.totalScore = this.txtTotalScore.Text;

            this.markingSheet.TestSubs = new List<string>();
            if (this.templeteType != "WRITING")
            {
                CheckedListBox.CheckedIndexCollection chkitems = chbSubject.CheckedIndices;
                this.markingSheet.TestSubs.Clear();

                for (int i = 0; i < chkitems.Count; i++)
                {
                    this.markingSheet.TestSubs.Add(chbSubject.Items[Int32.Parse(chkitems[i].ToString())].ToString());
                }
                this.chbSubject.ClearSelected();
            }

            this.markingSheet.Scores = scores;

            ((frmReader)this.Parent).Check_Error();
            ((frmReader)this.Parent).ChangeResultList(this.sheetIndex, this.markingSheet);

            this.templeteType = "";
            this.sheetIndex = -1;
            this.markingSheet = null;

            scorebindingsource.ResetBindings(true);

            this.Visible = false;
        }

        /// <summary>
        /// 수정하지 않고 취소 눌렀을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelCorrection_Click(object sender, EventArgs e)
        {
            // TODO : clear grdScore

            scorebindingsource.ResetBindings(true);
            this.Visible = false;
        }

        /// <summary>
        /// 학생팝업버튼 눌렀을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStudentpop_Click(object sender, EventArgs e)
        {
            dlgStudent pop = new dlgStudent();
            pop.students = this.students;
            if (pop.ShowDialog() == DialogResult.OK)
                this.txtStudentNo.Text = pop.StudentID;

        }

        private void pnlScore_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picScore_MouseClick(object sender, MouseEventArgs e)
        {

            int width = picScore.Image.Width;
            int selection = ((List<Score>)scorebindingsource.DataSource)[this.gridScore.CurrentRow.Index].selection;

            decimal number = Convert.ToDecimal(e.X) / (Convert.ToDecimal(width) / Convert.ToDecimal(selection));

            string strnumber = Convert.ToInt32(Math.Ceiling(number)).ToString();

            if (this.templeteType == "WRITING")
            {
                strnumber = (Int32.Parse(strnumber) - 1).ToString();
            }


            string curanswer = ((List<Score>)scorebindingsource.DataSource)[this.gridScore.CurrentRow.Index].Answer;

            if (curanswer.Contains(strnumber))  // 포함되어있으면 제거
            {
                curanswer = curanswer.Replace(strnumber, "");
            }
            else  // 포함 안되어 있으면 추가
            {
                curanswer = curanswer + strnumber;  // 숫자를 순서에 맞게 배열할지는 고민해보자
            }

            ((List<Score>)scorebindingsource.DataSource)[this.gridScore.CurrentRow.Index].Answer = curanswer;

            this.gridScore.CurrentRow.Cells["Answer"].Value = curanswer;

        }

        private void gridScore_Scroll(object sender, ScrollEventArgs e)
        {
            if ( ((Form)this.Parent).WindowState != FormWindowState.Maximized )
                this.gridScore.Invalidate();
        }

    }
}
