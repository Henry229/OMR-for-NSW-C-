using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

// WEB API
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSedu.OMR
{

    /// <summary>
    /// DB 접속 및 쿼리수행 관리 클래스
    /// </summary>
    public class DatabaseUtil
    {
        static string conn = "";
        static System.Collections.Specialized.NameValueCollection appSetting = ConfigurationManager.AppSettings;
        static public string application = appSetting.GetValues("application")[0].ToUpper();   // Target Database 지정
        static public string userid = "";

        public DatabaseUtil()
        {

        }


        /// <summary>
        /// Adhoc Query 수행
        /// </summary>
        /// <param name="command"></param>
        /// <param name="trantype"></param>
        /// <returns></returns>
        static public SQLResult executeAdHocQuery(string command, SQLTransactionType trantype)
        {
            SQLResult result = new SQLResult();

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlTransaction tran = null;
                SqlCommand comm = null;
                DataSet dset = new DataSet();
                try
                {

                    conn.Open();

                    if (trantype != SQLTransactionType.SELECT)
                    {
                        tran = conn.BeginTransaction();
                        comm = new SqlCommand(command, conn, tran);
                    }
                    else
                    {
                        comm = new SqlCommand(command, conn);
                    }
                    SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();


                    adapter.SelectCommand = comm;
                    adapter.Fill(dset, "Results");


                    if (trantype != SQLTransactionType.SELECT)
                    {
                        tran.Commit();
                    }

                    result.err = SQLErrorType.OK;
                    result.SqlMessage = "";
                    result.ds = dset;
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    result.err = SQLErrorType.Error;
                    result.SqlMessage = ex.Message;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (comm != null)
                        {
                            comm.Dispose();
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// 시험정보를 가져온다
        /// </summary>
        /// <param name="rslt">시험정보를 가져오기 위한 첫번째행의 MarkingSheet</param>
        /// <returns></returns>
        static public SQLResult getExamInfo(MarkingSheet rslt)
        {
            SQLResult result = null;

            string sql = "";

            if (rslt.TestLevel == null)
            {
                rslt.TestLevel = "1";
            }

            if (rslt.TestType == null)
            {
                rslt.TestType = "";
            }

            for (int i = 0; i < rslt.TestSubs.Count; i++)
            {
                sql += @"
exec PR_Get_Exam_Answer '" + rslt.TestType.Replace("_", " ") + "', '"
                         + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', '"
                         + rslt.TestSubs[i].ToUpper() + "', '"
                         + rslt.TestNo + "'";
                /*
                select c.qn_total, c.qn_num, c.answer, c.alt_answer, category_id, category, category_detail_id, category_detail
                from test_detail a
                join test_type b on a.test_idx = b.idx
                join test_qn c on a.idx = c.detail_idx
                where b.testtype = '" + rslt.TestType.Replace("_", " ") + @"'
                and  Substring(replace( replace( a.subject, ' ', ''), 'English', 'Reading'),1,5) like Substring('" + rslt.TestSubs[i].ToUpper() + @"',1,5) + '%'
                and a.grade = " + rslt.TestLevel.ToUpper().Replace("YEAR", "") + @" - Case when b.testtype = 'Selective Trial Test' and b.course = 'Regular' and " + rslt.TestLevel.ToUpper().Replace("YEAR", "") + @" = 6 Then 1 Else 0 END
                and a.test_no = " + rslt.TestNo + @"
                and a.myear =  Year(Getdate()) - Case when b.testtype = 'Selective Trial Test' and b.course = 'Regular' and " + rslt.TestLevel.ToUpper().Replace("YEAR", "") + @" = 6 Then 1 Else 0 END
                order by a.test_no 
                ";*/
            }

            result = executeAdHocQuery(sql, SQLTransactionType.SELECT);

            if (result.ds != null && result.ds.Tables != null)
            {

                for (int i = 0; i < result.ds.Tables.Count; i++)
                {
                    if (result.ds.Tables[i] != null && rslt.TestSubs != null && rslt.TestSubs.Count > i)
                    {
                        result.ds.Tables[i].TableName = rslt.TestSubs[i];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 시험정보를 가져온다
        /// </summary>
        /// <param name="rslt">시험정보를 가져오기 위한 첫번째행의 MarkingSheet</param>
        /// <returns></returns>
        static public bool checkExamTaken(string strWriting, MarkingSheet rslt)
        {

            string sql = @"
exec PR_Get_Exam_Info '" + rslt.StudentID + "', '" + rslt.TestType.Replace("_", " ") + "', '"
                         + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', '"
                         + (strWriting == "WRITING" ? "Essay Writing" : rslt.Scores[0].Subject == "NA" ? rslt.Scores[60].Subject : rslt.Scores[0].Subject) + "', '"
                         + rslt.TestNo + "'";
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand(sql, conn);
                SqlDataReader sdr = comm.ExecuteReader();
                string strIdx = "";

                if (sdr.Read())
                {
                    strIdx = sdr[0].ToString();

                    if (sdr.Read())
                    {
                        // 리턴이 두개 이상이면 에러 처리
                        sdr.Close();
                        throw new Exception("2 or more data exist in test_detail TABLE ");
                    }
                }
                else  // 없어도 에러처리
                {
                    sdr.Close();
                    throw new Exception("no data exist in test_detail TABLE ");
                }

                sdr.Close();

                if (strWriting == "WRITING")
                {
                    sql = " select detail_idx from essay_writing ";
                }
                else
                {
                    string table = getAnswerTable(rslt);
                    sql = " select top 1 detail_idx from " + table;
                }

                sql += " Where stud_id = '" + rslt.StudentID + @"' and detail_idx = " + strIdx;

                comm.CommandText = sql;
                object existing = comm.ExecuteScalar();

                if (existing == null || System.DBNull.Value == existing)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// 마킹결과 저장. executeAdHocQuery 메서드를 사용하려 했으나, 
        /// 트랜잭션관리 및 성능최적화를 위해 별도 메서드로 처리
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        static public SQLResult sendScore(MarkingSheet rslt, DataSet examInfo)
        {
            List<Score> scores = rslt.Scores;
            SQLResult result = new SQLResult();

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlTransaction tran = null;
                SqlCommand comm = null;
                DataSet dset = new DataSet();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    comm = new SqlCommand("", conn, tran);

                    string subject = "";
                    string idx = "";
                    string type_idx = "";
                    string sale_idx = "";
                    string stud_id = "";
                    string testset = "";


                    string sql = "";
                    DataRow[] dr = null;

                    for (int j = 0; j < scores.Count; j++)
                    {
                        // 과목마킹이 없으면 답안체크는 넘어간다

                        bool existsubject = false;
                        while (!existsubject && j < scores.Count)
                        {
                            for (int k = 0; k < rslt.TestSubs.Count; k++)
                            {
                                if (scores[j].Subject.ToUpper() == rslt.TestSubs[k].ToUpper())
                                {
                                    existsubject = true;
                                    break;
                                }
                            }

                            if (!existsubject)
                            {
                                j++;
                            }
                        }

                        if (j >= scores.Count)
                            break;


                        string table = getAnswerTable(rslt);

                        try
                        {
                            // subject가 바뀌면 다시 시험키를 가져온다
                            if (subject != scores[j].Subject)
                            {
                                sql = @"
exec PR_Get_Exam_Info '" + rslt.StudentID + "', '" + rslt.TestType.Replace("_", " ") + "', '"
                         + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', '" + scores[j].Subject + "', '"
                         + rslt.TestNo + "'";

                                subject = scores[j].Subject;
                                comm.CommandText = sql;
                                SqlDataReader sdr = comm.ExecuteReader();
                                if (sdr.Read())
                                {
                                    idx = sdr[0].ToString();
                                    type_idx = sdr[1].ToString();
                                    stud_id = sdr[2].ToString();
                                    sale_idx = sdr[3].ToString();
                                    testset = sdr[4].ToString();

                                    if (sdr.Read())
                                    {
                                        // 리턴이 두개 이상이면 에러 처리
                                        sdr.Close();
                                        throw new Exception("2 or more data exist in test_detail TABLE ");
                                    }

                                    sdr.Close();


                                }
                                else
                                {
                                    // 리턴이 하나도 없으면 에러처리
                                    sdr.Close();
                                    throw new Exception("no data exists in test_detail TABLE ");

                                }

                            }

                            dr = null;
                            for (int k = 0; k < examInfo.Tables.Count; k++)
                            {
                                if (scores[j].Subject.ToUpper().Substring(0, scores[j].Subject.Length > 5 ? 5 : scores[j].Subject.Length)
                                == examInfo.Tables[k].TableName.ToUpper().Substring(0, examInfo.Tables[k].TableName.Length > 5 ? 5 : examInfo.Tables[k].TableName.Length))
                                {
                                    //select c.qn_total, c.qn_num, c.answer, c.alt_answer, category_id, category, category_detail_id, category_detail
                                    dr = examInfo.Tables[k].Select(" qn_num = " + scores[j].QuestionNo);
                                }
                            }

                            if (dr == null || dr.Length == 0)
                                continue;


                            // 이하 기존에 csonlineschool에서 Answer 테이블에 넣던 방식
                                                        sql = @"

                            Insert into "+ table + @"(stud_id, detail_idx,sale_idx,test_num
                            ,answer,alt_answer,stud_answer,category,category_id
                            ,category_detail,category_detail_id,reg_date,end_flag,rev_date)  -- end_flag는 무조건 '1'


                            Select '" + stud_id + @"', " + idx + ", " + sale_idx + @", " + scores[j].QuestionNo + @" 
                            , '" + dr[0]["answer"] + "','" + dr[0]["alt_answer"] + "', '" + scores[j].Answer + "', '" + dr[0]["category"].ToString().Replace("'", "''") + "', '" + dr[0]["category_id"] + @"'
                            , '" + dr[0]["category_detail"].ToString().Replace("'", "''") + "', '" + dr[0]["category_detail_id"] + "', getdate(), 1, getdate()" ;
                                                        comm.CommandText = sql;
                                                        comm.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            result.err = SQLErrorType.Error;
                            throw ex;
                        }


                    }  // j loop end

                    tran.Rollback();

                    result.err = SQLErrorType.OK;
                    result.SqlMessage = "";
                    result.ds = dset;
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    result.err = SQLErrorType.Error;
                    result.SqlMessage = ex.Message;
                }
                finally
                {
                    if (conn != null)
                    {
                        comm.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
            return result;

        }


        /// <summary>
        /// Getting Testset ID, etc.
        /// </summary>
        /// <param name="rslt"></param>
        /// <param name="examInfo"></param>
        /// <returns></returns>
        static public Tuple<SQLResult, List<Dictionary<string, object>>> getSendInfoForApi(MarkingSheet rslt, DataSet examInfo)
        {
            List<Dictionary<string, object>> _info = new List<Dictionary<string, object>>();
            List<Score> scores = rslt.Scores;
            SQLResult result = new SQLResult();

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlCommand comm = null;
                DataSet dset = new DataSet();
                try
                {
                    conn.Open();
                    comm = new SqlCommand("", conn);

                    string subject = "";
                    string stud_id = "";
                    string testset = "";


                    string sql = "";
                    DataRow[] dr = null;

                    for (int j = 0; j < scores.Count; j++)
                    {
                        // 과목마킹이 없으면 답안체크는 넘어간다

                        bool existsubject = false;
                        while (!existsubject && j < scores.Count)
                        {
                            for (int k = 0; k < rslt.TestSubs.Count; k++)
                            {
                                if (scores[j].Subject.ToUpper() == rslt.TestSubs[k].ToUpper())
                                {
                                    existsubject = true;
                                    break;
                                }
                            }

                            if (!existsubject)
                            {
                                j++;
                            }
                        }

                        if (j >= scores.Count)
                            break;


                        string table = getAnswerTable(rslt);

                        try
                        {
                            // subject가 바뀌면 다시 시험키를 가져온다
                            if (subject != scores[j].Subject)
                            {
                                sql = @"
                        exec PR_Get_Exam_Info '" + rslt.StudentID + "', '" + rslt.TestType.Replace("_", " ") + "', '"
                         + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', '" + scores[j].Subject + "', '"
                         + rslt.TestNo + "'";

                                subject = scores[j].Subject;
                                comm.CommandText = sql;
                                SqlDataReader sdr = comm.ExecuteReader();
                                if (sdr.Read())
                                {
                                    stud_id = sdr[2].ToString();
                                    testset = sdr[4].ToString();

                                    var _dic = new Dictionary<string, object>();
                                    //_dic.Add("student_id", "admin");
                                    //_dic.Add("testset_guid", "fcc99c56-de0a-4c36-b003-9867e3ce3bc4");
                                    _dic.Add("student_id", stud_id);
                                    _dic.Add("testset_guid", testset);
                                    _dic.Add("subject", subject);
                                    _dic.Add("point", j);
                                    _info.Add(_dic);

                                    if (sdr.Read())
                                    {
                                        // 리턴이 두개 이상이면 에러 처리
                                        sdr.Close();
                                        throw new Exception("2 or more data exist in test_detail TABLE ");
                                    }

                                    sdr.Close();


                                }
                                else
                                {
                                    // 리턴이 하나도 없으면 에러처리
                                    sdr.Close();
                                    throw new Exception("no data exists in test_detail TABLE ");

                                }

                            }

                            dr = null;
                            for (int k = 0; k < examInfo.Tables.Count; k++)
                            {
                                if (scores[j].Subject.ToUpper().Substring(0, scores[j].Subject.Length > 5 ? 5 : scores[j].Subject.Length)
                                == examInfo.Tables[k].TableName.ToUpper().Substring(0, examInfo.Tables[k].TableName.Length > 5 ? 5 : examInfo.Tables[k].TableName.Length))
                                {
                                    //select c.qn_total, c.qn_num, c.answer, c.alt_answer, category_id, category, category_detail_id, category_detail
                                    dr = examInfo.Tables[k].Select(" qn_num = " + scores[j].QuestionNo);
                                }
                            }

                            if (dr == null || dr.Length == 0)
                                continue;

                        }
                        catch (Exception ex)
                        {
                            result.err = SQLErrorType.Error;
                            throw ex;
                        }


                    }  // j loop end

                    //student info
                    var pre_student_id = String.Empty;
                    for (var i = 0; i < _info.Count; i++)
                    {
                        if (!pre_student_id.Equals(_info[i]["student_id"].ToString()))
                        {
                            comm = new SqlCommand("SELECT stud_first_name, stud_last_name, branch FROM MEMBER WHERE stud_id = @student_id", conn);
                            comm.Parameters.AddWithValue("@student_id", _info[i]["student_id"].ToString());
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    _info[i].Add("stud_first_name", reader["stud_first_name"].ToString());
                                    _info[i].Add("stud_last_name", reader["stud_last_name"].ToString());
                                    _info[i].Add("branch", reader["branch"].ToString().Trim());
                                }
                            }
                        }
                        else
                        {
                            if (i > 0)
                            {
                                _info[i].Add("stud_first_name", _info[i-1]["stud_first_name"].ToString());
                                _info[i].Add("stud_last_name", _info[i-1]["stud_last_name"].ToString());
                                _info[i].Add("branch", _info[i-1]["branch"].ToString());
                            }
                        }
                        pre_student_id = _info[i]["student_id"].ToString();
                    }

                    result.err = SQLErrorType.OK;
                    result.SqlMessage = "";
                    result.ds = dset;
                }
                catch (Exception ex)
                {
                    result.err = SQLErrorType.Error;
                    result.SqlMessage = ex.Message;
                }
                finally
                {
                    if (conn != null)
                    {
                        comm.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
            var _t = new Tuple<SQLResult, List<Dictionary<string, object>>>(result, _info);
            return _t;
        }


        /// <summary>
        /// TSTM WEB API 호출
        /// </summary>
        /// <param name="rslt"></param>
        /// <param name="examInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        static public SQLResult sendScoreApi(MarkingSheet rslt, DataSet examInfo, List<Dictionary<string, object>> info)
        {
            List<Score> scores = rslt.Scores;

            var _result = Task.Run(async () => await InsertDataAsync(scores, examInfo, info)).Result;

            return _result;
        }

        static public async Task<SQLResult> InsertDataAsync(List<Score> scores, DataSet examInfo, List<Dictionary<string, object>> info)
        {
            var appSettings = ConfigurationManager.AppSettings;
            bool? _isSuccess = null;
            SQLResult _result = new SQLResult();
            var baseAddress = new Uri(appSettings.Get("TSTMBaseAddress"));

            Object[] arrayOfObjects = new Object[] { scores, examInfo, info };
            string json = JsonConvert.SerializeObject(arrayOfObjects);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var _CredentialBase64 = appSettings.Get("AuthenticationHeaderValue");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("KEY", String.Format("{0}", _CredentialBase64));
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync("api/omr/marking", httpContent);
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);
                        if (result["result"].ToString().Equals("success"))
                        {
                            _isSuccess = true;
                            _result.err = SQLErrorType.OK;
                            _result.SqlMessage = String.Empty;
                        }
                    }
                    else
                    {
                        _isSuccess = false;
                        _result.err = SQLErrorType.Error;
                        if (response.StatusCode.ToString().Equals("InternalServerError"))
                        {
                            _result.SqlMessage = "The error occurred while applying for request data";
                        }
                        else
                        {
                            JObject jsonObj = JObject.Parse(responseData);
                            _result.SqlMessage = jsonObj.GetValue("message").ToString();
                        }
                        
                    }
                }
            }

            if (_isSuccess.GetValueOrDefault(false))
            {
                _isSuccess = true;
                _result.err = SQLErrorType.OK;
                _result.SqlMessage = String.Empty;
            }

            return _result;
        }

        /// <summary>
        /// Getting Testset ID, etc.
        /// </summary>
        /// <param name="rslt"></param>
        /// <param name="examInfo"></param>
        /// <returns></returns>
        static public Tuple<SQLResult, List<Dictionary<string, object>>> getWritingSendInfoForApi(MarkingSheet rslt, DataSet examInfo)
        {
            List<Dictionary<string, object>> _info = new List<Dictionary<string, object>>();
            List<Score> scores = rslt.Scores;
            SQLResult result = new SQLResult();

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlCommand comm = null;
                DataSet dset = new DataSet();
                try
                {
                    conn.Open();
                    comm = new SqlCommand("", conn);

                    string subject = "";
                    string stud_id = "";
                    string testset = "";


                    string sql = "";
                    DataRow[] dr = null;

                    subject = scores[0].Subject;
                    sql = @"
exec PR_Get_Exam_Info '" + rslt.StudentID + "', '" + rslt.TestType.Replace("_", " ") + "', '"
             + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', 'Essay Writing', '"
             + rslt.TestNo + "'";

                    try
                    {
                        comm.CommandText = sql;
                        SqlDataReader sdr = comm.ExecuteReader();
                        if (sdr.Read())
                        {
                            stud_id = sdr[2].ToString();
                            testset = sdr[4].ToString();

                            if (sdr[3] == System.DBNull.Value)
                            {
                                // sale 테이블이 없을경우 이지만 이제는 없을경우 procedure에서 넣어줌
                                sdr.Close();
                                throw new Exception("no sale data for this student ");
                            }

                            if (sdr.Read())
                            {
                                // 리턴이 두개 이상이면 에러 처리
                                sdr.Close();
                                throw new Exception("2 or more data exist in test_detail TABLE ");
                            }

                            var _dic = new Dictionary<string, object>();
                            //_dic.Add("student_id", "admin");
                            //_dic.Add("testset_guid", "fcc99c56-de0a-4c36-b003-9867e3ce3bc4");
                            _dic.Add("student_id", stud_id);
                            _dic.Add("testset_guid", testset);
                            _dic.Add("subject", subject);
                           _info.Add(_dic);

                            sdr.Close();
                        }
                        else
                        {
                            // 리턴이 하나도 없으면 에러처리
                            sdr.Close();
                            throw new Exception("no data exists in test_detail TABLE ");

                        }
                    }
                    catch (Exception ex)
                    {
                        result.err = SQLErrorType.Error;
                        throw ex;
                    }

                    //student info
                    var pre_student_id = String.Empty;
                    for (var i = 0; i < _info.Count; i++)
                    {
                        if (!pre_student_id.Equals(_info[i]["student_id"].ToString()))
                        {
                            comm = new SqlCommand("SELECT stud_first_name, stud_last_name, branch FROM MEMBER WHERE stud_id = @student_id", conn);
                            comm.Parameters.AddWithValue("@student_id", _info[i]["student_id"].ToString());
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    _info[i].Add("stud_first_name", reader["stud_first_name"].ToString());
                                    _info[i].Add("stud_last_name", reader["stud_last_name"].ToString());
                                    _info[i].Add("branch", reader["branch"].ToString().Trim());
                                }
                            }
                        }
                        else
                        {
                            if (i > 0)
                            {
                                _info[i].Add("stud_first_name", _info[i - 1]["stud_first_name"].ToString());
                                _info[i].Add("stud_last_name", _info[i - 1]["stud_last_name"].ToString());
                                _info[i].Add("branch", _info[i - 1]["branch"].ToString());
                            }
                        }
                        pre_student_id = _info[i]["student_id"].ToString();
                    }

                    result.err = SQLErrorType.OK;
                    result.SqlMessage = "";
                    result.ds = dset;
                }
                catch (Exception ex)
                {
                    result.err = SQLErrorType.Error;
                    result.SqlMessage = ex.Message;
                }
                finally
                {
                    if (conn != null)
                    {
                        comm.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
            var _t = new Tuple<SQLResult, List<Dictionary<string, object>>>(result, _info);
            return _t;
        }

        /// <summary>
        /// TSTM WEB API 호출
        /// </summary>
        /// <param name="rslt"></param>
        /// <param name="examInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        static public SQLResult sendWritingScoreApi(MarkingSheet rslt, DataSet examInfo, List<Dictionary<string, object>> info)
        {
            List<Score> scores = rslt.Scores;

            var _result = Task.Run(async () => await InsertWritingDataAsync(scores, examInfo, info)).Result;

            return _result;
        }

        static public async Task<SQLResult> InsertWritingDataAsync(List<Score> scores, DataSet examInfo, List<Dictionary<string, object>> info)
        {
            var appSettings = ConfigurationManager.AppSettings;
            bool? _isSuccess = null;
            SQLResult _result = new SQLResult();
            var baseAddress = new Uri(appSettings.Get("TSTMBaseAddress"));

            Object[] arrayOfObjects = new Object[] { scores, examInfo, info };
            string json = JsonConvert.SerializeObject(arrayOfObjects);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return null;

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                var _CredentialBase64 = appSettings.Get("AuthenticationHeaderValue");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("KEY", String.Format("{0}", _CredentialBase64));
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync("api/omr/writing", httpContent);
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData);
                        if (result["result"].ToString().Equals("success"))
                        {
                            _isSuccess = true;
                            _result.err = SQLErrorType.OK;
                            _result.SqlMessage = String.Empty;
                        }
                    }
                    else
                    {
                        _isSuccess = false;
                        _result.err = SQLErrorType.Error;
                        if (response.StatusCode.ToString().Equals("InternalServerError"))
                        {
                            _result.SqlMessage = "The error occurred while applying for request data";
                        }
                        else
                        {
                            JObject jsonObj = JObject.Parse(responseData);
                            _result.SqlMessage = jsonObj.GetValue("message").ToString();
                        }

                    }
                }
            }

            if (_isSuccess.GetValueOrDefault(false))
            {
                _isSuccess = true;
                _result.err = SQLErrorType.OK;
                _result.SqlMessage = String.Empty;
            }

            return _result;
        }

        //static private Task<Uri> InsertData2()
        //{
        //    HttpContent content = new StringContent(json);
        //    HttpResponseMessage response = await client.PostAsync("https://tstm.csonlineschool.com.au/api/omr/marking", content);


        //    response.EnsureSuccessStatusCode();

        //    // return URI of the created resource.
        //    return response.Headers.Location;
        //}

        static private string getAnswerTable(MarkingSheet rslt)
        {
            if (rslt.TestType.ToUpper() == TestType.CLASS_TEST.ToString()
                || rslt.TestType.ToUpper() == TestType.ENTRANCE.ToString()
                )
            {
                return "answer";
            }
            /*
            else if (rslt.TestType.ToUpper() == TestType.SELECTIVE_TRIAL_TEST.ToString() && rslt.TestNo == "60")
            {
                return "answer_sp";
            }
            */
            else
            {
                return "answer_11";
            }
        }

        /// <summary>
        /// writing guide 결과 저장. executeAdHocQuery 메서드를 사용하려 했으나, 
        /// 트랜잭션관리 및 성능최적화를 위해 별도 메서드로 처리
        /// </summary>
        /// <param name="scores"></param>
        /// <returns></returns>
        static public SQLResult sendGuide(MarkingSheet rslt)
        {
            List<Score> scores = rslt.Scores;
            SQLResult result = new SQLResult();

            string tableName = "essay_writing";
            /*
            if (rslt.TestNo == "60" && rslt.TestType.ToUpper() == TestType.SELECTIVE_TRIAL_TEST.ToString())
            {
                tableName = "essay_writing_sp";
            }
            */

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                SqlTransaction tran = null;
                SqlCommand comm = null;
                DataSet dset = new DataSet();
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    comm = new SqlCommand("", conn, tran);

                    string idx = "";
                    string type_idx = "";
                    string sale_idx = "";
                    string stud_id = "";

                    string sql = "";


                    sql = @"
exec PR_Get_Exam_Info '" + rslt.StudentID + "', '" + rslt.TestType.Replace("_", " ") + "', '"
             + rslt.TestLevel.ToUpper().Replace("YEAR", "") + "', 'Essay Writing', '"
             + rslt.TestNo + "'";

                    try
                    {
                        comm.CommandText = sql;
                        SqlDataReader sdr = comm.ExecuteReader();
                        if (sdr.Read())
                        {

                            idx = sdr[0].ToString();
                            type_idx = sdr[1].ToString();
                            stud_id = sdr[2].ToString();
                            sale_idx = sdr[3].ToString();

                            if (sdr[3] == System.DBNull.Value)
                            {
                                // sale 테이블이 없을경우 이지만 이제는 없을경우 procedure에서 넣어줌
                                sdr.Close();
                                throw new Exception("no sale data for this student ");
                            }

                            if (sdr.Read())
                            {
                                // 리턴이 두개 이상이면 에러 처리
                                sdr.Close();
                                throw new Exception("2 or more data exist in test_detail TABLE ");
                            }

                            sdr.Close();
                        }
                        else
                        {
                            // 리턴이 하나도 없으면 에러처리
                            sdr.Close();
                            throw new Exception("no data exists in test_detail TABLE ");

                        }
                    }
                    catch (Exception ex)
                    {
                        result.err = SQLErrorType.Error;
                        throw ex;
                    }

                    string strScore = "";
                    strScore = scores[0].Answer + "," + scores[2].Answer + "," + scores[1].Answer + "," + scores[3].Answer
                         + ", null," + scores[4].Answer + "," + scores[5].Answer + "," + scores[6].Answer;
                    sql = @"
Delete " + tableName + @"
Where stud_id = '" + stud_id + @"' and detail_idx = " + idx + @"
insert into " + tableName + @" (  stud_id, detail_idx, sale_idx, stud_answer, test_num, end_flag
	, contentScore, structureScore, vocabularyScore, languageScore, sentencesScore, spellingScore, grammarScore, punctuationScore
	, content, structure, vocabulary, language1, sentences, spelling, grammar, punctuation
	, contentDescription, structureDescription, vocabularyDescription, languageDescription, sentencesDescription, spellingDescription, grammarDescription, punctuationDescription
	, totalDescription, start_date, end_date, d1, d2, d3, d4, d5, d6, d7, d8, score_end, DPFlag, rev_date )

SELECT  '" + stud_id + @"',  " + idx + @", " + sale_idx + @", '', 1 , 1
	, " + strScore + @" -- 8개 점수  Sentences = null
	, (select [description] from item where idx = 13)
    , (select [description] from item where idx = 15)
    , (select [description] from item where idx = 14)
    , (select [description] from item where idx = 16)
    , (select [description] from item where idx = -1)
    , (select [description] from item where idx = 17)
    , (select [description] from item where idx = 18)
    , (select [description] from item where idx = 19)
	, y.con_descr, y.str_descr, y.cre_descr, y.lan_descr, y.sen_descr, y.spl_descr, y.grm_descr, y.pun_descr   -- comment
	, null, getdate(), getdate(), y.con_idx, y.str_idx, y.cre_idx, y.lan_idx, y.sen_idx, y.spl_idx, y.grm_idx, y.pun_idx 
    , 1, 0, getdate() 
from 
(
    select max(x.con_idx) as con_idx, max(x.con_descr) as con_descr
    , max(x.str_idx ) as str_idx, max(x.str_descr ) as str_descr
    , max(x.cre_idx ) as cre_idx, max(x.cre_descr ) as cre_descr
    , max(x.lan_idx ) as lan_idx, max(x.lan_descr ) as lan_descr
    , null as sen_idx, null as sen_descr
    , max(x.spl_idx ) as spl_idx, max(x.spl_descr ) as spl_descr
    , max(x.grm_idx ) as grm_idx, max(x.grm_descr ) as grm_descr
    , max(x.pun_idx ) as pun_idx, max(x.pun_descr ) as pun_descr
    from 
    (
    select idx as con_idx, description as con_descr
    , null as str_idx, null as str_descr
    , null as cre_idx, null as cre_descr
    , null as lan_idx, null as lan_descr
    , null as sen_idx, null as sen_descr
    , null as spl_idx, null as spl_descr
    , null as grm_idx, null as grm_descr
    , null as pun_idx, null as pun_descr 
    from comment where item_idx = 13 and score = " + scores[0].Answer + @"
    union all 
    select null,null
    , idx as str_idx, description as str_descr
    , null, null, null, null, null, null, null, null, null, null, null, null
    from comment where item_idx = 15 and score = " + scores[2].Answer + @"
    union all 
    select null, null, null, null
    , idx as cre_idx, description as cre_descr
    , null, null, null, null, null, null, null, null, null, null
    from comment where item_idx = 14 and score = " + scores[1].Answer + @"
    union all 
    select null, null, null, null, null, null
    , idx as lan_idx, description as lan_descr
    , null, null, null, null, null, null, null, null
    from comment where item_idx = 16 and score = " + scores[3].Answer + @"
    union all 
    select null, null, null, null, null, null, null, null, null, null
    , idx as spl_idx, description as spl_descr
    , null, null, null, null
    from comment where item_idx = 17 and score = " + scores[4].Answer + @" 
    union all 
    select null, null, null, null, null, null, null, null, null, null, null, null
    , idx as grm_idx, description as grm_descr
    , null, null
    from comment where item_idx = 18 and score = " + scores[5].Answer + @" 
    union all 
    select null, null, null, null, null, null, null, null, null, null, null, null, null, null
    , idx as pun_idx, description as pun_descr
    from comment where item_idx = 19 and score = " + scores[6].Answer + @" 
    ) x
) y ";

                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();

                    tran.Commit();

                    result.err = SQLErrorType.OK;
                    result.SqlMessage = "";
                    result.ds = dset;
                }
                catch (Exception ex)
                {
                    if (tran != null)
                        tran.Rollback();
                    result.err = SQLErrorType.Error;
                    result.SqlMessage = ex.Message;
                }
                finally
                {
                    if (conn != null)
                    {
                        comm.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// 프로시저 수행메서드. 현재 프로시저를 사용하지 않아서 구현안함
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="param"></param>
        /// <param name="trantype"></param>
        /// <returns></returns>
        static public SQLResult executeProcedure(string proc, SqlParameterCollection param, SQLTransactionType trantype)
        {
            return new SQLResult();
        }

        /// <summary>
        /// 접속문자열 가져오기
        /// </summary>
        /// <returns></returns>
        static private string getConnectionString()
        {

            System.Collections.Specialized.NameValueCollection appSetting = ConfigurationManager.AppSettings;
            application = appSetting.GetValues("application")[0].ToUpper();

            if (conn == "")
            {
                ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

                string tempconn = "";

                if (settings != null)
                {
                    foreach (ConnectionStringSettings cs in settings)
                    {
                        if (application == "TEST" && cs.Name.ToUpper() == "TEST")
                        {
                            tempconn = cs.ConnectionString;
                        }
                        else if (application == "PRODUCTION" && cs.Name.ToUpper() == "PRODUCTION")
                        {
                            tempconn = cs.ConnectionString;
                        }
                    }
                }

                conn = DecryptConnectionString(tempconn);
            }

            return conn;
        }

        static private string DecryptConnectionString(string tempconn)
        {
            return CSedu.OMR.CryptorEngine.Decrypt(tempconn, true);
        }

    }


    /// <summary>
    /// 쿼리결과 리턴 클래스
    /// </summary>
    public class SQLResult
    {
        public SQLErrorType err;
        public string SqlMessage;
        public DataSet ds;

        public SQLResult()
        {

        }

        public SQLResult(SQLErrorType errtype, string msg)
        {
            this.err = errtype;
            this.SqlMessage = msg;
        }

    }

    public enum SQLErrorType
    {
        Error, OK
    }

    public enum SQLTransactionType
    {
        SELECT, INSERT, DELETE, UPDATE
    }

}
