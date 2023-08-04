using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

using OMR;
using System.Configuration;

namespace CSedu.OMR
{
    /// <summary>
    /// Data Class for DataGridView - grdResultList
    /// </summary>
    public class MarkingSheet
    {
        public string FileName { get; set; }
        public MarkingErrorType Error { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public List<Score> Scores;
        public string totalScore;
        public Bitmap sheet;

        public string TestType;
        public List<string> TestSubs = new List<string>();
        public string TestLevel;
        public string TestNo;
        public string BranchNo;

        public string testval;


        /// <summary>
        /// 마킹 에러 체크 - 시트별
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public void Check_Marking_Error(DataSet students, DataSet examinfo, string templete)
        {
            try
            {
                this.Error = MarkingErrorType.CHECK_ERROR;

                if (this.BranchNo == "" || this.TestNo == "" || this.TestType == "" || this.TestLevel == ""
                    || this.StudentID == "")
                {
                    this.Error = MarkingErrorType.INSUFFICIENT_INFO;
                    return;
                }

                if (templete != "WRITING" && (this.TestSubs == null || this.TestSubs.Count == 0))
                {
                    this.Error = MarkingErrorType.INSUFFICIENT_INFO;
                    return;
                }



                DataRow[] student = students.Tables[0].Select("[StudentNo]='" + this.StudentID + "'");
                if (student.Length == 0)
                {
                    this.Error = MarkingErrorType.WRONG_STUDENTNO;
                    return;
                }

                this.StudentName = student[0]["FirstName"].ToString() + ", " + student[0]["LastName"].ToString();


                //과목명 보정 변경 중간에 바뀌다 마는 경우 발생
                for (int i = 0; i < this.Scores.Count; i++)
                {
                    if (this.Scores[i].blockType == BlockType.ANSWER)
                    {
                        this.Scores[i].Subject = this.GetRealSubject(this.Scores[i].Subject);
                    }
                }


                for (int i = 0; i < this.Scores.Count; i++)
                {

                    if (this.Scores[i].blockType == BlockType.ANSWER)
                    {

                        for (int j = 0; j < examinfo.Tables.Count; j++)
                        {
                            //과목 앞 5자리가 같고 문제번호가 같으면 마킹되어야 함
                            if (this.Scores[i].Subject.ToUpper().Substring(0, this.Scores[i].Subject.Length > 5 ? 5 : this.Scores[i].Subject.Length)
                                == examinfo.Tables[j].TableName.ToUpper().Substring(0, examinfo.Tables[j].TableName.Length > 5 ? 5 : examinfo.Tables[j].TableName.Length)
                                && Int32.Parse(this.Scores[i].QuestionNo) <= Int32.Parse(examinfo.Tables[j].Rows[0][0].ToString()))
                            {
                                this.Scores[i].check = true;
                                break;
                            }
                            else
                            {
                                this.Scores[i].check = false;
                            }
                        }
                    }


                    switch (this.Scores[i].blockType)
                    {
                        case BlockType.ANSWER:


                            if (this.Scores[i].Answer == "" && this.Scores[i].check)
                            {
                                this.Error = MarkingErrorType.NO_ANSWER;
                            }
                            else if (this.Scores[i].Answer.Length > 1)
                            {
                                this.Error = MarkingErrorType.MULTI_ANSWER;
                            }
                            break;
                        case BlockType.GUIDE:
                        case BlockType.TOTALSCORE:
                            if (this.Scores[i].Answer == "")
                            {
                                this.Error = MarkingErrorType.MISSING_GUIDESCORE;
                            }
                            else if (this.Scores[i].Answer.Length > 1 && this.Scores[i].blockType == BlockType.GUIDE)
                            {
                                this.Error = MarkingErrorType.MULTI_ANSWER;
                            }
                            break;
                    }


                    if (this.Error != MarkingErrorType.CHECK_ERROR)
                        break;

                }

                if (this.Error == MarkingErrorType.CHECK_ERROR)
                    this.Error = MarkingErrorType.SCORE_OK;

                return;
            }
            catch (Exception ex)
            {
                this.Error = MarkingErrorType.INSUFFICIENT_INFO;
            }
        }


        /// <summary>
        /// 마킹 에러 체크 - 전체(헤더비교)
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        static public void Check_Header_Error(ref List<MarkingSheet> results, DataSet student, ref DataSet examinfo, string tmplt)
        {

            if (results == null || results.Count == 0)
                return;

            string branchno = results[0].BranchNo;
            string testLevel = results[0].TestLevel;
            string testNo = results[0].TestNo;
            string testType = results[0].TestType;
            List<string> testSubs = results[0].TestSubs;

            // 에러체크전에 시험정보 가져온다
            SQLResult sql = DatabaseUtil.getExamInfo(results[0]);
            examinfo = sql.ds;

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].Error < MarkingErrorType.RECOG_OK)
                    continue;
                results[i].Error = MarkingErrorType.CHECK_ERROR;

                if (results[i].BranchNo != branchno)
                    results[i].Error = MarkingErrorType.DIFFERENT_HEADER;
                if (results[i].TestLevel == null || testLevel == null || results[i].TestLevel.ToUpper() != testLevel.ToUpper())
                    results[i].Error = MarkingErrorType.DIFFERENT_HEADER;
                if (results[i].TestNo == null || testNo == null || results[i].TestNo != testNo)
                    results[i].Error = MarkingErrorType.DIFFERENT_HEADER;
                if (results[i].TestType == null || testType == null || results[i].TestType.ToUpper() != testType.ToUpper())
                    results[i].Error = MarkingErrorType.DIFFERENT_HEADER;

                if (results[i].TestSubs == null || testSubs == null || results[i].TestSubs.Count != testSubs.Count)
                {
                    results[i].Error = MarkingErrorType.DIFFERENT_HEADER;
                }
                else
                {
                    for (int j = 0; j < testSubs.Count; j++)
                    {
                        if (testSubs[j].ToUpper() != results[i].TestSubs[j].ToUpper())
                            results[i].Error = MarkingErrorType.DIFFERENT_HEADER;
                    }
                }

                if (results[i].Error == MarkingErrorType.CHECK_ERROR)
                    results[i].Check_Marking_Error(student, examinfo, tmplt);
            }

        }


        /// <summary>
        /// 실제 과목명 가져온다   이걸 왜 했지? 없어도 될거 같은데....
        /// </summary>
        /// <param name="tmpsubject">현재 과목명</param>
        /// <returns>실제 과목명</returns>
        public string GetRealSubject(string tmpsubject)
        {
            tmpsubject = tmpsubject.ToUpper();
            string realSub = tmpsubject;
            for (int i = 0; i < TestSubs.Count; i++)
            {
                switch (TestSubs[i].ToUpper())
                {
                    case "GA":
                    case "READING":
                    case "VERBALREASONING":
                    case "ABSTRACTREASONING":
                        if (tmpsubject.ToUpper() == "VERBALREASONING" || tmpsubject.ToUpper() == "READING" || tmpsubject.ToUpper() == "ABSTRACTREASONING" || tmpsubject.ToUpper() == "GA")
                        {
                            realSub = TestSubs[i].ToUpper();
                            return realSub;
                        }
                        break;
                    case "MATHS":
                    case "NUMERICALREASONING":
                    case "TEXTTYPE":
                    case "THINKING":
                        if (tmpsubject.ToUpper() == "MATHS" || tmpsubject.ToUpper() == "NUMERICALREASONING" || tmpsubject.ToUpper() == "TEXTTYPE" || tmpsubject.ToUpper() == "THINKING")
                        {
                            realSub = TestSubs[i].ToUpper();
                            return realSub;
                        }

                        break;
                }
            }

            return realSub;
        }


        /// <summary>
        /// 마킹인식처리
        /// </summary>
        /// <param name="tmplt">로드된 템플릿 시트</param>
        /// <param name="blackpen">펜 타입 (blackpen or not)</param>
        public void Process(Template tmplt, bool blackpen, double dblackratio, double dmulticheck, double dnotfillcheck)
        {

            if (this.Error == MarkingErrorType.NOT_PROCESSED || this.Error == MarkingErrorType.LOW_MEMORY)
            {
                OpticalReader reader = new OpticalReader();
                reader.black_ratio = dblackratio;
                reader.multicheck = dmulticheck;
                reader.notfillcheck = dnotfillcheck;

                try
                {
                    MarkingErrorType err = this.Error;

                    this.sheet = reader.ExtractOMRSheet(this.sheet, tmplt.backgroundFill
                    , tmplt.startingContrast, tmplt, ref err);

                    this.Error = err;

                    if (this.Error == MarkingErrorType.CHECK_ERROR || this.Error == MarkingErrorType.BAD_IMAGE)
                    {
                        return;
                    }

                    reader.getScoreOfSheet(tmplt, this, blackpen);
                }
                catch (Exception ex)
                {
                    this.Error = MarkingErrorType.LOW_MEMORY;
                    return;
                }
            }
        }

        /// <summary>
        /// 마킹결과 전송
        /// </summary>
        /// <param name="strWriting">Writing 시험인지 여부</param>
        /// <param name="examInfo"> 시험정보(Multiple 참고값 처리용) </param>
        /// <returns></returns>
        public SQLResult SendData(string strWriting, DataSet examInfo)
        {

            string strToSendData = ConfigurationManager.AppSettings["ToSendData"].ToLower();

            SQLResult sql = null;
            if (strWriting == "WRITING")
            {
                if (strToSendData == "csonline" || strToSendData == "both")
                {
                    sql = DatabaseUtil.sendGuide(this);
                }

                if (sql == null || sql.err == SQLErrorType.OK)
                {
                    if (strToSendData == "tstm" || strToSendData == "both")
                    {
                        var _t = DatabaseUtil.getWritingSendInfoForApi(this, examInfo);

                        sql = _t.Item1;
                        //if (sql.err == SQLErrorType.OK)
                        //{
                        sql = DatabaseUtil.sendWritingScoreApi(this, examInfo, _t.Item2);
                        //}
                    }
                }
            }
            else
            {
                if (strToSendData == "csonline" || strToSendData == "both")
                {
                    sql = DatabaseUtil.sendScore(this, examInfo);
                }

                if (sql == null || sql.err == SQLErrorType.OK)
                {
                    if (strToSendData == "tstm" || strToSendData == "both")
                    {
                        var _t = DatabaseUtil.getSendInfoForApi(this, examInfo);

                        sql = _t.Item1;
                        //if (sql.err == SQLErrorType.OK)
                        //{
                        sql = DatabaseUtil.sendScoreApi(this, examInfo, _t.Item2);
                        //}
                    }
                }
            }

            if (sql.err == SQLErrorType.OK)
            {
                this.Error = MarkingErrorType.SEND_OK;
            }
            else
            {
                this.Error = MarkingErrorType.SERVER_ERROR;
            }
            return sql;
        }

        public bool checkMarking(string strWriting)
        {
            return DatabaseUtil.checkExamTaken(strWriting, this);
        }

    }

    /// <summary>
    /// Data Class for DataGridView - grdScore
    /// </summary>
    public class Score
    {
        public string Subject { get; set; }
        public string QuestionNo { get; set; }
        public string Answer { get; set; }
        public bool[] marking;
        public bool check = true;
        public BlockType blockType;
        public Bitmap area;                            // to show sliced image
        public int selection;
    }

}
