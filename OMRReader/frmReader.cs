using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

using OMR;
using CButtonLib;
using ExcelLibrary;
//using System.Windows.Media.Imaging;

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

using System.Globalization;
using System.Resources;

namespace CSedu.OMR
{
    public partial class frmReader : Form
    {
        List<MarkingSheet> sheets = new List<MarkingSheet>();  // 이미지를 읽어서 보관하는 변수. 모든 작업의 기본이 되는 리스트
        BindingSource resultbindingsource = new BindingSource();         // grdResultList 의 데이터소스 Binding
        List<Template> templetes = null;                    // OMR sheet 유형을 저장.
        DataSet students = null;                            // Database의 학생리스트를 가져와서 보관
        DataSet examinfo = null;
        DataSet[] qnInfo = null;

        Point mouseStart = new Point();                     // 이미지를 마우스로 컨트롤하기 위한 마우스위치변수
        pnlDetailScore detailScore = null;

        // 이미지 판정 처리 기준값
        public double multicheck = 1.5; 
        public double notfillcheck = 3.0; 
        public double black_ratio = 0.113;  

        ResourceManager res_man;
        CultureInfo cult;

        int cntScan = 0;

        public frmReader()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤 초기화
        /// </summary>
        private void InitControls()
        {
            detailScore = new pnlDetailScore();
            detailScore.Location = new Point( this.btnOpen.Left - 6, this.btnOpen.Top - 3 );
            detailScore.Width = 485;
            detailScore.Height = 720;
            detailScore.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            detailScore.init_Control( students );

            this.Controls.Add(detailScore);

            detailScore.Visible = false;
            this.grdSheetList.DataSource = resultbindingsource;

            this.Text += "(" + DatabaseUtil.userid + ")";
            this.Text += DatabaseUtil.application.ToUpper() == "TEST" ? " - TEST " : "";

            cult = CultureInfo.CurrentCulture;
            res_man = new ResourceManager("CSedu.OMR.resource.msg", typeof(frmLogin).Assembly);

            this.rdoPencil.Checked = true;
            System.IO.FileInfo confile = new System.IO.FileInfo("config.txt");

            try
            {
                //read file for config values

                if (confile.Exists)
                {


                    System.IO.StreamReader reader = confile.OpenText();
                    if (!reader.EndOfStream)
                    {
                        string strmulticheck = reader.ReadLine();
                        this.multicheck = Convert.ToDouble(strmulticheck);
                    }
                    if (!reader.EndOfStream)
                    {
                        string strnotfillcheck = reader.ReadLine();
                        this.notfillcheck = Convert.ToDouble(strnotfillcheck);
                    }
                    if (!reader.EndOfStream)
                    {
                        string strblackratio = reader.ReadLine();
                        this.black_ratio = Convert.ToDouble(strblackratio);
                    }
                    if (!reader.EndOfStream)
                    {
                        string strpentype = reader.ReadLine();
                        if (strpentype.Equals("0"))
                            rdoPen.Checked = true;
                        else
                            rdoPencil.Checked = true;
                    }

                    reader.Close();
                    reader.Dispose();
                }
            }
            catch { }

        }

        /// <summary>
        /// Database에서 StudentList Load
        /// </summary>
        private void GetStudents()
        {
            string sql = @"
select id_number as [StudentNo], stud_first_name as [FirstName], stud_last_name as [LastName], branch, grade
from member 
where isnull(id_number, '') <> ''
order by id_number";

            SQLResult rslt = DatabaseUtil.executeAdHocQuery(sql, SQLTransactionType.SELECT);
            if (rslt.err == SQLErrorType.Error)
            {
                string msg = "Can not access student infomation :"; // res_man.GetString("Msg01", cult);  //Can not access student infomation :
                MessageBox.Show(msg + rslt.SqlMessage);
            }
            else
            {
                this.students = rslt.ds;
            }
        }

        /// <summary>
        /// xml파일(시트템플릿) 로드
        /// </summary>
        private void LoadXML()
        {

            // Download from server  https://www.csonlineschool.com.au/upload/xml/sheets.xml

            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile("https://www.csonlineschool.com.au/upload/xml/sheets.xml", "sheets.xml");
            }

            templetes = TemplateManager.GetTemplates("sheets.xml");
            this.cmbTemplete.Items.Clear();

            foreach ( Template t in templetes )
            {
                this.cmbTemplete.Items.Add(t.description);
            }

            this.cmbTemplete.SelectedIndex = 0;
        }

        /// <summary>
        /// 기존 로드된 이미지가 있는지 확인
        /// </summary>
        private void checkExistingData()
        {
            if (sheets.Count > 0)
            {
                string msg = "There is/are one or more existing data. Do you want to delete?"; // res_man.GetString("Msg02", cult);  //There is/are one or more existing data. Do you want to delete?
                if (MessageBox.Show(msg, "OMR Reader", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    sheets.Clear();
                }
            }
        }

        /// <summary>
        /// 그리드뷰의 선택된 라인이 변경되었을때
        /// </summary>
        /// <param name="ind">행번호</param>
        private void changedRow(int ind)
        {
            Bitmap bit = sheets[ind].sheet;

            this.picBox.Tag = bit;

            Image thumbNail = new Bitmap((Bitmap)this.picBox.Tag, this.panel2.Width, ((int)((double)bit.Height / (double)bit.Width * (double)this.panel2.Width)));
            this.picBox.Image = thumbNail;
            this.picBox.Width = thumbNail.Width ;
            this.picBox.Height = thumbNail.Height ;
        }

        /// <summary>
        /// 그리드뷰의 데이터 모두 지우기
        /// </summary>
        private void removeAll()
        {
            string msg = "All records will be deleted. Are your sure?"; // res_man.GetString("Msg03", cult);  //All records will be deleted. Are your sure?
            if (MessageBox.Show(msg, "Remove All", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                this.sheets.Clear();
                resultbindingsource.DataSource = sheets;
                resultbindingsource.ResetBindings(true);

                this.picBox.Image = null;

                this.cmbTemplete.Enabled = true;
            }
        }

        /// <summary>
        /// 그리드뷰의 선택된 행 지우기
        /// </summary>
        private void deleteRow()
        {
            string msg = "Selected records will be deleted. Are your sure?"; // res_man.GetString("Msg04", cult);  //Selected records will be deleted. Are your sure?
            if (MessageBox.Show(msg, "Delete Records", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {

                List<int> tempIndex = new List<int>();
                foreach (DataGridViewRow row in grdSheetList.SelectedRows)
                {
                    tempIndex.Add(row.Index);
                }

                tempIndex.Sort();

                for (int i = tempIndex.Count - 1; i >= 0; i--)
                {
                    sheets.RemoveAt(tempIndex[i]);
                }

                resultbindingsource.DataSource = sheets;
                resultbindingsource.ResetBindings(true);

                if (sheets.Count == 0)
                {
                    this.cmbTemplete.Enabled = true;
                }
            }

            MarkingSheet.Check_Header_Error(ref sheets, students, ref this.examinfo, cmbTemplete.Text.ToUpper());
        }

        /// <summary>
        /// 이미지 인식처리 수행
        /// </summary>
        private void Process()
        {
            if (sheets == null || sheets.Count == 0)
                return;

            this.cntScan = 0;
            this.timer1.Start();

            this.Cursor = Cursors.WaitCursor;

            
        }

        /// <summary>
        /// 처리된 이미지 전송
        /// </summary>
        private void SendData()
        {
            if (sheets == null || sheets.Count == 0)
                return;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                //DatabaseUtil.application = "PRODUCTION";

                bool answer = false;

                for ( int i = 0 ; i < sheets.Count; i++ )
                {
                    if (sheets[i].Error >= MarkingErrorType.NO_ANSWER )
                    {
                        grdSheetList.CurrentCell = grdSheetList.Rows[i].Cells[0];
                        grdSheetList.Refresh();

                        string msg = @"This Sheet remains ErrorType : XXX
Do you really want to send result score?"; // res_man.GetString("Msg06", cult); //"This Sheet remains ErrorType : " + sheets[i].Error.ToString()
                                //+ System.Environment.NewLine + "Do you really want to send result score?";

                        msg = msg.Replace("XXX", sheets[i].Error.ToString());




                        




                        if (sheets[i].Error != MarkingErrorType.SCORE_OK && !answer) // 에러가 있거나 패스된 경우
                        {
                            if (MessageBox.Show(msg, "Alert", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                answer = true;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        //bool isProcessed = sheets[i].checkMarking(this.cmbTemplete.Text.ToUpper().ToUpper());
                        // 이미 처리된 같은 학생이 있으면 물어본다.
                        //if (false)
                        //{
                        //    if (MessageBox.Show("The scores of this student are already sent." + System.Environment.NewLine
                        //        + "Are you sure send again?" + System.Environment.NewLine
                        //        + "Previous data will be deleted."
                        //    , "Scores are already sent", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        //    {
                        //        SQLResult sql = null;
                        //        sql = sheets[i].SendData(this.cmbTemplete.Text.ToUpper(), examinfo);

                        //        if (sql.err != SQLErrorType.OK)
                        //        {
                        //            MessageBox.Show(sql.SqlMessage);
                        //        }
                        //    }
                        //}
                        //else
                        {
                            SQLResult sql = null;
                            sql = sheets[i].SendData(this.cmbTemplete.Text.ToUpper(), examinfo);

                            if (sql.err != SQLErrorType.OK)
                            {
                                MessageBox.Show(sql.SqlMessage);
                            }
                        }
                    }
                } 

                // 완료되면 reload
                resultbindingsource.DataSource = sheets;
                resultbindingsource.ResetBindings(true);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
                
        }

        /// <summary>
        /// 데이터의 유효성 체크
        /// </summary>
        public void Check_Error()
        {
            MarkingSheet.Check_Header_Error(ref sheets, students, ref this.examinfo, cmbTemplete.Text.ToUpper());

            resultbindingsource.DataSource = sheets;
            resultbindingsource.ResetBindings(true);
        }

        /// <summary>
        /// MarkingSheet 배열의 특정순번의 값 변경
        /// </summary>
        /// <param name="index"> 변경될 Index</param>
        /// <param name="rslt">Replace될 MarkingSheet</param>
        public void ChangeResultList(int index, MarkingSheet rslt)
        {
            this.sheets[index] = rslt;
        }


        #region  UI Events


        private void frmReader_Load(object sender, EventArgs e)
        {
            // TODO : templete XML(sheet.xml) load

            // TODO : control clear
            sheets.Clear();
            LoadXML();
            GetStudents();
            InitControls();
 

        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemOpenFile_Click(object sender, EventArgs e)
        {

            this.dlgOpenFile.Filter = "jpeg|*.jpg|tiff|*.tif";
            this.dlgOpenFile.FileName = "";

            this.dlgOpenFile.Multiselect = true;

            this.Cursor = Cursors.WaitCursor;
            try
            {

                if (DialogResult.OK == this.dlgFolder.ShowDialog())
                {
                    checkExistingData();


                    ImageManager.GetAllImgFiles(dlgFolder.SelectedPath, ref sheets);

                    if (sheets.Count > 0)
                    {
                        resultbindingsource.DataSource = sheets;
                        resultbindingsource.ResetBindings(true);
                        changedRow(0);

                        // 데이터가 로드되면 마킹 용지종류를 변경할수 없다. 였다가 스캔하고나서 변경못하는걸로 변경 2014.09.18 from hugh
                        //this.cmbTemplete.Enabled = false;
                    }

                    return;
                }
            }
            catch 
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 폴더를 열어 이미지를 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemOpenFolder_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {

                if (DialogResult.OK == this.dlgFolder.ShowDialog())
                {
                    checkExistingData();

                    ImageManager.GetAllImgFiles(dlgFolder.SelectedPath, ref sheets);

                    if (sheets.Count > 0)
                    {
                        resultbindingsource.DataSource = sheets;
                        resultbindingsource.ResetBindings(true);
                        changedRow(0);

                        // 데이터가 로드되면 마킹 용지종류를 변경할수 없다. 
                        this.cmbTemplete.Enabled = false;
                    }

                    return;
                }
               
            }
            catch 
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        


        /// <summary>
        /// 확대버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMag_Click(object sender, EventArgs e)
        {
            if (sheets.Count < 1)
                return;

            double width = ((double)this.picBox.Width) * 1.1;
            double height = ((double)this.picBox.Height) * 1.1;
            Image thumbNail = new Bitmap((Bitmap)this.picBox.Tag, (int)width, (int)height);
            this.picBox.Image = thumbNail;
            this.picBox.Width = thumbNail.Width;
            this.picBox.Height = thumbNail.Height;
        }

        /// <summary>
        /// 축소버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMini_Click(object sender, EventArgs e)
        {
            if (sheets.Count < 1)
                return;

            if (this.picBox.Width > 300)
            {
                double width = ((double)this.picBox.Width ) - 100;
                double height = ((double)this.picBox.Height ) - 100.0 * (((double)this.picBox.Height) / ((double)this.picBox.Width));
                Image thumbNail = new Bitmap((Bitmap)this.picBox.Tag, (int)width, (int)height);
                this.picBox.Image = thumbNail;
                this.picBox.Width = thumbNail.Width;
                this.picBox.Height = thumbNail.Height;
            }
            else
            {
                string msg = "Minimum Size!!"; // res_man.GetString("Msg07", cult);
                MessageBox.Show(msg);
            }
            
        }

        /// <summary>
        /// 이미지영역에 마우스클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseStart = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 이미지영역에서 마우스 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int deltaX = this.mouseStart.X - e.X;
                int deltaY = this.mouseStart.Y - e.Y;

                this.panel2.AutoScrollPosition =
                    new Point(deltaX - panel2.AutoScrollPosition.X, deltaY - panel2.AutoScrollPosition.Y);
            }
        }

        #region  GridView Events

        private void grdResultList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            changedRow(e.RowIndex);
        }

        private void grdResultList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            this.detailScore.bindData(this.sheets[e.RowIndex], e.RowIndex, cmbTemplete.Text.ToUpper());
            this.detailScore.btnCancelCorrection.Select();

        }

        private void grdSheetList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.detailScore.bindData(this.sheets[this.grdSheetList.CurrentCell.RowIndex], this.grdSheetList.CurrentCell.RowIndex, cmbTemplete.Text.ToUpper());
                e.SuppressKeyPress = true;

                this.detailScore.btnCancelCorrection.Select();

            }
        }


        private void grdResultList_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            this.Cursor = Cursors.WaitCursor;

            ImageManager.GetAllImgFiles(files, ref sheets);

            if (sheets.Count > 0)
            {
                resultbindingsource.DataSource = sheets;
                resultbindingsource.ResetBindings(true);


                changedRow(0);

                // 데이터가 로드되면 마킹 용지종류를 변경할수 없다.  스캔후 변경못하는 방식으로 수정
                //this.cmbTemplete.Enabled = false;
            }

            this.Cursor = Cursors.Default;

            
        }

        private void grdResultList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void grdResultList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in this.grdSheetList.Rows)
            {
                if (row.Cells[1].Value.ToString() == MarkingErrorType.NO_ANSWER.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.MULTI_ANSWER .ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.MISSING_GUIDESCORE.ToString() )
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                }
                else if (row.Cells[1].Value.ToString() == MarkingErrorType.BAD_IMAGE.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.INSUFFICIENT_INFO.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.LOW_MEMORY.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.WRONG_STUDENTNO.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.SERVER_ERROR.ToString()
                    || row.Cells[1].Value.ToString() == MarkingErrorType.DIFFERENT_HEADER.ToString()
                    )
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                }
            }
        }


        #endregion

        /// <summary>
        /// Clear All버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.removeAll();
        }

        /// <summary>
        /// Delete Record버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.deleteRow();
        }

        /// <summary>
        /// Scan 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, EventArgs e)
        {
            // 스캔 완료하면 시트변경 못한다.
            this.cmbTemplete.Enabled = false;
            this.Process();
        }

        /// <summary>
        /// Send 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            this.SendData();
        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            this.deleteRow();
        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemClearAll_Click(object sender, EventArgs e)
        {
            this.removeAll();
        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemProcess_Click(object sender, EventArgs e)
        {
            this.Process();
        }


        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullSheet_Click(object sender, EventArgs e)
        {
            if (this.cmbTemplete.Text != "WRITING")
            {
                dlgFullSheet dlg = new dlgFullSheet();
                dlg.results = this.sheets;
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
            return;
        }

        /// <summary>
        /// Export Image 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMakeTiff_Click(object sender, EventArgs e)
        {
            if (this.cmbTemplete.Text.ToUpper() == "WRITING")
            {
                // 첫번째 학생아이디 받아서 저장폴더 선택 

                string strFirstStudent = "";
                string strTestNo = "";

                for (int i = 0; i < sheets.Count; i++)
                {
                    if (sheets[i].StudentID != null && sheets[i].Error >= MarkingErrorType.NO_ANSWER)
                    {
                        strFirstStudent = sheets[i].StudentID;
                        break;
                    }
                }


                //dlgSaveFile.FileName = strFirstStudent;



                // 학생번호 초기화
                strFirstStudent = "";
                strTestNo = "";

                string strDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments ) + "\\" + "OMRExport";
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(strDir);

                if (!dir.Exists)
                {
                    dir.Create();
                }

                strDir = strDir + "\\" + System.DateTime.Now.ToString("yyyyMMdd");
                dir = new System.IO.DirectoryInfo(strDir);

                if (!dir.Exists)
                {
                    dir.Create();
                }


                List<Bitmap> bits = new List<Bitmap>();

                for (int i = 0; i < sheets.Count; i++)
                {
                    if (sheets[i].StudentID != null && sheets[i].Error >= MarkingErrorType.NO_ANSWER)
                    {


                        if (bits.Count > 0)  // 진행중인 이미지파일이 있으면 파일처리
                        {
                            ImageManager.saveMultipage(bits, dir.FullName + "\\" + strTestNo + "" + strFirstStudent + ".tif", "TIFF");
                            bits.Clear();
                        }

                        // 첫번째 장에서는 학생번호 + 시험번호 가져온다
                        strFirstStudent = sheets[i].StudentID;
                        strTestNo = sheets[i].TestNo;

                    }
                    else if ( strFirstStudent != "" && ( sheets[i].Error <= MarkingErrorType.DIFFERENT_HEADER ) )
                    {
                        // 학생번호가 있고 다음장이 이미지 처리가 안된 경우는 파일저장대상임
                        bits.Add(sheets[i].sheet);
                    }
                    else
                    {
                        // 처리됐으나 학생번호 오류등인 경우 초기화하고 패스한다.
                        strFirstStudent = "";
                        bits.Clear();
                    }
                }

                //루프완료후 진행중인 이미지 파일이 있으면 파일을 닫는다.
                if (bits.Count > 0)  // 진행중인 이미지파일이 있으면 파일처리
                {
                    ImageManager.saveMultipage(bits, dir.FullName + "\\"  + strTestNo + "" + strFirstStudent + ".tif", "TIFF");
                    bits.Clear();
                }

                MessageBox.Show("Completed files exporting");
            }

        }

        /// <summary>
        /// 폼 꾸미기 이미지영역에 테두리를 그려준다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Brushes.Goldenrod, 7.5f), new Rectangle(this.panel2.Location, this.panel2.Size));
        }

        /// <summary>
        /// 이미지 스캔 처리시 메모리 오류가 나서 비동기로 처리하기 위해 타이머를 돌린다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( this.cntScan < sheets.Count)  // 시트보다 작으면 스캔 계속
            {
                try
                {
                    grdSheetList.CurrentCell = grdSheetList.Rows[this.cntScan].Cells[0];
                    grdSheetList.Refresh();

                    

                    sheets[cntScan].Process(this.templetes[cmbTemplete.SelectedIndex], this.rdoPen.Checked
                            , this.black_ratio, this.multicheck, this.notfillcheck);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cntScan++;
                }
            }
            else{  // 크면 스캔 종료
                timer1.Stop();
                MarkingSheet.Check_Header_Error(ref sheets, students, ref this.examinfo, cmbTemplete.Text.ToUpper());
                resultbindingsource.DataSource = sheets;
                resultbindingsource.ResetBindings(true);

                //string msg = res_man.GetString("Msg05", cult);  //scanning completed. Do you want to send data now?
                //if (MessageBox.Show(msg, "SCAN", MessageBoxButtons.YesNo)
                //    == DialogResult.Yes)
                //{
                //    this.SendData();
                //}

                this.Cursor = Cursors.Default;
                changedRow(this.grdSheetList.SelectedRows[0].Index);
            }

        }

        /// <summary>
        /// 시트 선택시 Writing일 경우 Export Image 버튼 활성화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTemplete_SelectionChangeCommitted(object sender, EventArgs e)
        {

            if (cmbTemplete.Items[cmbTemplete.SelectedIndex].ToString().ToUpper() == "WRITING")
            {
                this.btnTiff.Enabled = true;
            }
            else
            {
                this.btnTiff.Enabled = false;
            }
        }



        #endregion

        private void btnHelp_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            dlgAdjust dlg = new dlgAdjust();
            dlg.decBlackRatio = (decimal)this.black_ratio;
            dlg.decMultiCheck = (decimal)this.multicheck;
            dlg.decNotFillCheck = (decimal)this.notfillcheck;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.black_ratio = (double)dlg.decBlackRatio;
                this.multicheck = (double)dlg.decMultiCheck;
                this.notfillcheck = (double)dlg.decNotFillCheck;
            }
        }

        private void menuItemCopyFirst_Click(object sender, EventArgs e)
        {
            if (sheets == null || sheets.Count == 0)
                return;

            string branchNo = sheets[0].BranchNo;
            string testtype = sheets[0].TestType;
            string testno = sheets[0].TestNo;
            string testlevel = sheets[0].TestLevel;


            for (int i = 1; i < sheets.Count; i++)
            {
                sheets[i].BranchNo = branchNo;
                sheets[i].TestType = testtype;
                sheets[i].TestNo = testno;
                sheets[i].TestLevel = testlevel;

                List<string> templist = new List<string>();

                for(int j= 0; j < sheets[0].TestSubs.Count; j++)
                {
                    templist.Add(sheets[0].TestSubs[j]);
                }

                sheets[i].TestSubs = templist;



            }

            Check_Error();
        }

        private void btnExcel_ClickButtonArea(object Sender, MouseEventArgs e)
        {

        }

        private void ExportExcel_NPOI(string filename)
        {
            
            if (sheets == null || sheets.Count == 0)
                return;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                //DatabaseUtil.application = "PRODUCTION";

                bool answer = false;

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();


                //fill background
                ICellStyle style1 = hssfworkbook.CreateCellStyle();
                style1.FillPattern = FillPattern.SolidForeground;
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Orange.Index;


                ICellStyle styleheader = hssfworkbook.CreateCellStyle();
                styleheader.FillPattern = FillPattern.SolidForeground;
                styleheader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;

                short numFormat = hssfworkbook.CreateDataFormat().GetFormat("#,##0.00");


                var font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 16;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font.Underline = NPOI.SS.UserModel.FontUnderlineType.Double;

                ICellStyle styletitle = hssfworkbook.CreateCellStyle();
                styletitle.SetFont(font);




                ////create a entry of DocumentSummaryInformation
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "OmniAll";
                hssfworkbook.DocumentSummaryInformation = dsi;

                ////create a entry of SummaryInformation
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = "Test Result";
                hssfworkbook.SummaryInformation = si;


                var excelsheet_s = hssfworkbook.CreateSheet("Summary");

                excelsheet_s.CreateRow(0).CreateCell(0).SetCellValue("Test Results - Summary" );
                excelsheet_s.GetRow(0).GetCell(0).CellStyle = styletitle;

                excelsheet_s.CreateRow(1);

                var headerrow_s = excelsheet_s.CreateRow(2);
                headerrow_s.CreateCell(0).SetCellValue("Student ID");
                headerrow_s.GetCell(0).CellStyle = styleheader;
                headerrow_s.CreateCell(1).SetCellValue("Student Name");
                headerrow_s.GetCell(1).CellStyle = styleheader;

                for ( int i = 0; i < this.examinfo.Tables.Count; i++)
                {
                    headerrow_s.CreateCell(i+2).SetCellValue(this.examinfo.Tables[i].TableName);
                    headerrow_s.GetCell(i+2).CellStyle = styleheader;
                }


                
                for (int i = 0; i < this.examinfo.Tables.Count; i++)
                {
                    string worksheetname = sheets[0].TestType + sheets[0].TestNo + "(" + examinfo.Tables[i].TableName + ")";
                    var excelsheet = hssfworkbook.CreateSheet(worksheetname);
                    

                    // Stud_id / stud_name / marking (wrong == red)

                    excelsheet.CreateRow(0).CreateCell(0).SetCellValue("Test Results - " + worksheetname.Replace("_", " "));
                    excelsheet.GetRow(0).GetCell(0).CellStyle = styletitle;


                    excelsheet.CreateRow(1);

                    var headerrow = excelsheet.CreateRow(2);
                    headerrow.CreateCell(0).SetCellValue("Student ID");
                    headerrow.GetCell(0).CellStyle = styleheader;
                    headerrow.CreateCell(1).SetCellValue("Student Name");
                    headerrow.GetCell(1).CellStyle = styleheader;

                    for (int j = 1; j <= examinfo.Tables[i].Rows.Count; j++)
                    {
                        headerrow.CreateCell(j+1).SetCellValue(j.ToString());
                        headerrow.GetCell(j + 1).CellStyle = styleheader;
                    }

                    headerrow.CreateCell(examinfo.Tables[i].Rows.Count + 2).SetCellValue("Score");
                    headerrow.GetCell(examinfo.Tables[i].Rows.Count + 2).CellStyle = styleheader;

                    for (int j = 0; j < sheets.Count; j++)
                    {
                        if (sheets[j].Error >= MarkingErrorType.NO_ANSWER)
                        {
                            grdSheetList.CurrentCell = grdSheetList.Rows[j].Cells[0];
                            grdSheetList.Refresh();

                            string msg = @"This Sheet remains ErrorType : XXX
Do you really want to send result score?"; // res_man.GetString("Msg06", cult); //"This Sheet remains ErrorType : " + sheets[i].Error.ToString()
                                                                           //+ System.Environment.NewLine + "Do you really want to send result score?";

                            msg = msg.Replace("XXX", sheets[j].Error.ToString());

                            if (sheets[j].Error != MarkingErrorType.SCORE_OK && !answer) // 에러가 있거나 패스된 경우
                            {
                                if (MessageBox.Show(msg, "Alert", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    answer = true;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            var studentrow = excelsheet.CreateRow(j+3);

                            studentrow.CreateCell(0).SetCellValue(sheets[j].StudentID);
                            studentrow.CreateCell(1).SetCellValue(sheets[j].StudentName);
                           

                            var studentrow_s = i == 0 ?  excelsheet_s.CreateRow(j + 3) : excelsheet_s.GetRow(j + 3);
                            studentrow_s.CreateCell(0).SetCellValue(sheets[j].StudentID);
                            studentrow_s.CreateCell(1).SetCellValue(sheets[j].StudentName);


                            double cnt_correct = 0;

                            //시험 과목의 문제수 만큼 루프를 돌려서 점수를 넣고 정답이 아닌경우 붉은색 표시
                            for (int k = 0; k < examinfo.Tables[i].Rows.Count; k++)
                            {

                                if (sheets[j].Scores[k + i * 60].Answer != examinfo.Tables[i].Rows[k]["ANSWER"].ToString())
                                {
                                    studentrow.CreateCell(k+2)
                                        .SetCellValue(sheets[j].Scores[k + i * 60].Answer + "(" + examinfo.Tables[i].Rows[k]["ANSWER"].ToString() + ")");
                                    studentrow.GetCell(k + 2).CellStyle = style1;
             
                                }
                                else
                                {
                                    studentrow.CreateCell(k + 2).SetCellValue(sheets[j].Scores[k + i * 60].Answer );
                                    cnt_correct = cnt_correct + 1;
                                }
                            }


                            //점수
                            studentrow.CreateCell(examinfo.Tables[i].Rows.Count + 2).SetCellValue(cnt_correct / (double)examinfo.Tables[i].Rows.Count * 100.0);
                            studentrow.GetCell(examinfo.Tables[i].Rows.Count + 2).CellStyle.DataFormat = numFormat;


                            studentrow_s.CreateCell(i + 2).SetCellValue(cnt_correct / (double)examinfo.Tables[i].Rows.Count * 100.0);
                            studentrow_s.GetCell(i + 2).CellStyle.DataFormat = numFormat;

                        }
                    }

                    excelsheet.AutoSizeColumn(1);

                }

                excelsheet_s.AutoSizeColumn(1);

                //Write the stream data of workbook to the root directory
                System.IO.FileStream file = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                hssfworkbook.Write(file);
                file.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void ExportExcel(string file)
        {
            if (sheets == null || sheets.Count == 0)
                return;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                //DatabaseUtil.application = "PRODUCTION";

                bool answer = false;

                ExcelLibrary.SpreadSheet.Worksheet[] ws = new ExcelLibrary.SpreadSheet.Worksheet[this.examinfo.Tables.Count + 1];


                //to avoid format error
                for (int j = 0; j < 100; j++)
                    ws[this.examinfo.Tables.Count].Cells[j, 0] = new ExcelLibrary.SpreadSheet.Cell("");

                ExcelLibrary.SpreadSheet.Workbook wb = new ExcelLibrary.SpreadSheet.Workbook();
                ws[this.examinfo.Tables.Count] = new ExcelLibrary.SpreadSheet.Worksheet("Summary");
                ws[this.examinfo.Tables.Count].Cells[2, 0] = new ExcelLibrary.SpreadSheet.Cell("Student ID");
                ws[this.examinfo.Tables.Count].Cells[2, 1] = new ExcelLibrary.SpreadSheet.Cell("Student Name");

                for( int i = 0; i < examinfo.Tables.Count; i++ )
                {
                    ws[this.examinfo.Tables.Count].Cells[2, i + 2] = new ExcelLibrary.SpreadSheet.Cell(examinfo.Tables[i].TableName);
                }

                for ( int i = 0; i < ws.Length; i++)
                {
                    string worksheetname = sheets[0].TestType + sheets[0].TestNo + "(" + examinfo.Tables[i].TableName + ")";
                    ws[i] = new ExcelLibrary.SpreadSheet.Worksheet(worksheetname);

                    //to avoid format error
                    for (int j = 0; j < 100; j++)
                        ws[i].Cells[j, 0] = new ExcelLibrary.SpreadSheet.Cell("");

                    // Stud_id / stud_name / marking (wrong == red)

                    ws[i].Cells[0, 0] = new ExcelLibrary.SpreadSheet.Cell("Test Results - " + worksheetname);
                    ws[i].Cells[2, 0] = new ExcelLibrary.SpreadSheet.Cell("Student ID");
                    ws[i].Cells[2, 1] = new ExcelLibrary.SpreadSheet.Cell("Student Name");

                    for (int j = 1; j <= examinfo.Tables[i].Rows.Count; j++)
                    {
                        ws[i].Cells[2, j + 1] = new ExcelLibrary.SpreadSheet.Cell(j.ToString());
                    }

                    ws[i].Cells[2, examinfo.Tables[i].Rows.Count + 2] = new ExcelLibrary.SpreadSheet.Cell("Score");


                    for (int j = 0; j < sheets.Count; j++)
                    {
                        if (sheets[j].Error >= MarkingErrorType.NO_ANSWER)
                        {
                            grdSheetList.CurrentCell = grdSheetList.Rows[j].Cells[0];
                            grdSheetList.Refresh();

                            string msg = @"This Sheet remains ErrorType : XXX
Do you really want to send result score?"; // res_man.GetString("Msg06", cult); //"This Sheet remains ErrorType : " + sheets[i].Error.ToString()
                                                                           //+ System.Environment.NewLine + "Do you really want to send result score?";

                            msg = msg.Replace("XXX", sheets[j].Error.ToString());

                            if (sheets[j].Error != MarkingErrorType.SCORE_OK && !answer) // 에러가 있거나 패스된 경우
                            {
                                if (MessageBox.Show(msg, "Alert", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    answer = true;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            ws[i].Cells[j + 3, 0] = new ExcelLibrary.SpreadSheet.Cell(sheets[j].StudentID);
                            ws[i].Cells[j + 3, 1] = new ExcelLibrary.SpreadSheet.Cell(sheets[j].StudentName);
                            
                            double cnt_correct = 0;

                            //시험 과목의 문제수 만큼 루프를 돌려서 점수를 넣고 정답이 아닌경우 붉은색 표시
                            for (int k = 0; k < examinfo.Tables[i].Rows.Count; k++)
                            {
                                
                                if (sheets[j].Scores[k+i*60].Answer != examinfo.Tables[i].Rows[k]["ANSWER"].ToString())
                                {
                                    ws[i].Cells[j + 3, k + 2] = new ExcelLibrary.SpreadSheet.Cell(
                                        sheets[j].Scores[k + i*60].Answer + "(" + examinfo.Tables[i].Rows[k]["ANSWER"].ToString() + ")");
                                }
                                else
                                {
                                    ws[i].Cells[j + 3, k + 2] = new ExcelLibrary.SpreadSheet.Cell(sheets[j].Scores[k+i*60].Answer);
                                    cnt_correct = cnt_correct + 1;
                                }
                            }


                            //점수
                            ws[i].Cells[j + 3, examinfo.Tables[i].Rows.Count + 2] = new ExcelLibrary.SpreadSheet.Cell(cnt_correct/(double)examinfo.Tables[i].Rows.Count * 100.0);

                            ws[this.examinfo.Tables.Count].Cells[j+3, i + 2] = new ExcelLibrary.SpreadSheet.Cell(cnt_correct / (double)examinfo.Tables[i].Rows.Count * 100.0);



                        }
                    }
                    wb.Worksheets.Add(ws[i]);
                }

                wb.Worksheets.Add(ws[this.examinfo.Tables.Count]);

                
                wb.Save(file);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //임시로 여기에 엑셀 export를 넣는다

                if (dlgSaveFile.ShowDialog() == DialogResult.OK)
                {
                    //ExportExcel(dlgSaveFile.FileName);

                    ExportExcel_NPOI(dlgSaveFile.FileName);
                }



            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void frmReader_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                String penType = "0";
                if (rdoPen.Checked)
                    penType = "0";
                else
                    penType = "1";

                //read file for config values
                System.IO.FileInfo confile1 = new System.IO.FileInfo("config.txt");

                System.IO.StreamWriter writer1 = confile1.CreateText();
                writer1.WriteLine(this.multicheck.ToString());
                writer1.WriteLine(this.notfillcheck.ToString());
                writer1.WriteLine(this.black_ratio.ToString());
                writer1.WriteLine(penType);

                writer1.Flush();
                writer1.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
