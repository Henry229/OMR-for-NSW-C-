using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Resources;
using System.Net;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;

namespace CSedu.OMR
{
    public partial class frmLogin : Form
    {
        private int cntClick = 0;
        ResourceManager res_man;
        CultureInfo cult;
        Point mousePoint = new Point();
        bool isButton = false;

        public frmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 로그인폼 로드시 사용자환경의 언어로 리소스메니저를 호출한다(다국어처리용)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            cult = CultureInfo.CurrentCulture;

            //cult = new CultureInfo("ko-kr");
            res_man = new ResourceManager("CSedu.OMR.resource.msg", typeof(frmLogin).Assembly);

            //string msg = res_man.GetString("Label01", cult);  // If you have any problem to using this program. Call XXX
            string msg = "If you have any problem to using this program. Call XXX";
        }

        /// <summary>
        /// 로그인 버튼 처리시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 라벨을 7번 클릭하면 이스터에그(?)가 나타나서 DB접속 문자열을 편집할수 있다
            if (cntClick == 7)
            {
                this.Height = this.Height + 337;
            }
            else
            {
                if ( CheckID() )
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 암호화 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            this.txtDecrypt.Text = CSedu.OMR.CryptorEngine.Encrypt(this.txtEncrypt.Text, true);
        }

        /// <summary>
        /// 복호화 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.txtEncrypt.Text = CSedu.OMR.CryptorEngine.Decrypt(this.txtDecrypt.Text, true);
        }

        /// <summary>
        /// 프로그램 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 사용자 로그인정보 체크
        /// </summary>
        /// <returns></returns>
        private bool CheckID()
        {
            //return true;
            string sql = @" SELECT admin_id FROM [dbo].[admin] WHERE admin_id = '" + this.txtID.Text + "' and admin_pwd = '"
                + this.txtPassword.Text + "'";

            try
            {
                SQLResult rslt = DatabaseUtil.executeAdHocQuery(sql, SQLTransactionType.SELECT);
                if (rslt.err == SQLErrorType.Error)
                {
                    MessageBox.Show(rslt.SqlMessage);
                    return false;
                }
                else
                {
                    if (rslt.ds.Tables[0].Rows.Count < 1) // 쿼리결과가 없으면 계정이 없거나 다른정보
                    {
                        string msg = "Wrong Account Infomation. Try Again.";//res_man.GetString("Msg09", cult);  // "Wrong Account Infomation. Try Again."

                        dlgAlart alart = new dlgAlart();
                        alart.ShowDialog("Log-in Error", msg);

                        return false;
                    }
                    else
                    {
                        string ip = GetIPAddress();
                        string mac = GetMacAddress();
                        
                        // 로그인정보 저장
                        sql = @"exec omr..CheckLogin '" + rslt.ds.Tables[0].Rows[0][0].ToString() + "','" + ip + "','" 
                            + mac + "','" + DatabaseUtil.application +"'";

                        SQLResult rslt2 = DatabaseUtil.executeAdHocQuery(sql, SQLTransactionType.INSERT);

                        if (rslt2.ds.Tables[0].Rows[0][0].ToString() != "00")
                        {
                            dlgAlart alart = new dlgAlart();
                            alart.ShowDialog("Log-in Error", rslt2.ds.Tables[0].Rows[0][1].ToString());
                            return false;
                        }

                        DatabaseUtil.userid = rslt.ds.Tables[0].Rows[0][0].ToString();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                dlgAlart alart = new dlgAlart();
                alart.ShowDialog("Log-in Error", ex.GetType().ToString() + System.Environment.NewLine
                    + "ErrorMessage : " + ex.Message);
            }

            return false;
        }

        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }

        private string GetIPAddress()
        {

            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            Regex ipregex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            for (int i = 0; i < addr.Length; i++)
            {
                if (addr[i].ToString() != "" && ipregex.IsMatch(addr[i].ToString()))
                {
                    return addr[i].ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 폼을 꾸며주기위한 메서드 오버로드, 폼에 테두리를 그려준다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Rectangle rec = new Rectangle(new Point(0,0), this.Size);

            e.Graphics.DrawRectangle(new Pen(Brushes.Navy, 8.0f), rec);
        }

        /// <summary>
        /// 외부 라이브러리 Cbutton클릭 이벤트 - 키보드 엔터시 로그인버튼 자동으로 눌려지기 위해 구현
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void btnLogin_ClickButtonArea(object Sender, MouseEventArgs e)
        {
            btnLogin_Click(Sender, e);
        }

        /// <summary>
        /// 이스터에그용 클릭 카운트 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            cntClick++;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

    }
}
