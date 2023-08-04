using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace CSedu.OMR
{
    public class TemplateManager
    {
        // OMR이미지인식 에 사용되는 변수 기본값, 이미지상태에 따라 변경될수도 있음.
        static private double minBlobRatio = 0.0001;
        static private double maxBlobRatio = 0.005;
        static private int startingContrast = 15;
        static private int backgroundFill = 0;
        static private int minBlobToDetect = 3;
        static private int imageMatch = 54;
        static private int treshValue = 44;


        /// <summary>
        /// 템플릿 시트에서 xml을 Parsing 하여 객체생성
        /// </summary>
        /// <param name="fileName">로드할 xml 파일명</param>
        /// <returns>리턴되는 템플릿 리스트</returns>
        static public List<Template> GetTemplates(string fileName)
        {
            List<Template> templetes = new List<Template>();

            XmlTextReader reader = new XmlTextReader(fileName);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            reader.Close();

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("OMRSheet");

            foreach (XmlNode node in nodes)
            {
                Template tmpt = new Template();
                tmpt.code = node.Attributes["Code"].Value;
                tmpt.description = node.Attributes["Descr"].Value;
                tmpt.sheetSize = (SheetSize)Enum.Parse(typeof(SheetSize), node.SelectSingleNode("./SheetSize").InnerText.ToString());
                tmpt.actualWidth = Int32.Parse(node.SelectSingleNode("./ActualWidth").InnerText);
                tmpt.actualHeight = Int32.Parse(node.SelectSingleNode("./ActualHeight").InnerText);
                tmpt.numofQuestions = Int32.Parse(node.SelectSingleNode("./NumofQuestion").InnerText);
                tmpt.minBlobRatio = TemplateManager.minBlobRatio;
                tmpt.maxBlobRatio = TemplateManager.maxBlobRatio;
                tmpt.startingContrast = TemplateManager.startingContrast;
                tmpt.backgroundFill = TemplateManager.backgroundFill;
                tmpt.minBlobToDetect = TemplateManager.minBlobToDetect;
                tmpt.imageMatch = TemplateManager.imageMatch;
                tmpt.treshValue = TemplateManager.treshValue;

                tmpt.numofBlocks = Int32.Parse(node.SelectSingleNode("./NumOfBlocks").InnerText);

                XmlNodeList children = node.SelectNodes("./Blocks/Block");
                List<Block> listBlock = new List<Block>();

                foreach (XmlNode child in children)
                {
                    Block bl = new Block();
                    bl.name = child.Attributes["Name"].Value;
                    bl.Type = (BlockType)Enum.Parse(typeof(BlockType), child.Attributes["BlockType"].Value);
                    bl.Direction = (MarkingDirection)Enum.Parse(typeof(MarkingDirection), child.Attributes["MarkingDirection"].Value);
                    bl.Group = child.Attributes["Group"] != null ? child.Attributes["Group"].Value : null;
                    bl.startingQuestion = child.Attributes["StartingQuestion"] != null ? Int32.Parse(child.Attributes["StartingQuestion"].Value) : 1;
                    bl.X = Int32.Parse(child.SelectSingleNode("./X").InnerText);
                    bl.Y = Int32.Parse(child.SelectSingleNode("./Y").InnerText);
                    bl.Width = Int32.Parse(child.SelectSingleNode("./Width").InnerText);
                    bl.Height = Int32.Parse(child.SelectSingleNode("./Height").InnerText);
                    bl.numberofQuestions = Int32.Parse(child.SelectSingleNode("./NumofQuestion").InnerText);
                    bl.numberofSelections = Int32.Parse(child.SelectSingleNode("./NumofSelection").InnerText);
                    bl.startingNumberofAnswer = Int32.Parse(child.SelectSingleNode("./SelectionStartingNumber").InnerText);   

                    XmlNodeList grandChildren = child.SelectNodes("./Items/Item");
                    bl.items = new List<string>();
                    bl.itemtype = (SelectionItemType)Enum.Parse(typeof(SelectionItemType), child.SelectSingleNode("./Items").Attributes["type"].Value);

                    foreach (XmlNode grandchild in grandChildren)
                    {
                        bl.items.Add(grandchild.InnerText);
                    }

                    listBlock.Add(bl);
                }

                tmpt.Blocks = listBlock;


                templetes.Add(tmpt);

            }

            return templetes;
        }


        /// <summary>
        /// templete 를 xml로 parsing
        /// </summary>
        /// <param name="fileName">로드할 xml 파일명</param>
        /// <returns>리턴되는 템플릿 리스트</returns>
        static public string SaveTemplates(string fileName, List<Template> templetes, List<string> deletelist)
        {
            XmlTextReader reader = new XmlTextReader(fileName);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            reader.Close();

            XmlNode root = xmlDoc.GetElementsByTagName("OMRSheets")[0];

            XmlNodeList sheets = xmlDoc.GetElementsByTagName("OMRSheet");
            foreach (XmlNode node in sheets)
            {
                string code = node.Attributes["Code"].Value;

                foreach ( string delcode in deletelist )
                {
                    if ( code == delcode )
                    {
                        root.RemoveChild(node);
                    }
                }
            }


            foreach ( Template tmplt in templetes)
            {
                if (tmplt.tag != "Y")
                    continue;
                XmlNode node = xmlDoc.CreateElement("OMRSheet");


                root.AppendChild(node);


            }


            XmlNodeList nodes = xmlDoc.GetElementsByTagName("OMRSheet");

            foreach (XmlNode node in nodes)
            {
                Template tmpt = new Template();
                tmpt.code = node.Attributes["Code"].Value;
                tmpt.description = node.Attributes["Descr"].Value;
                tmpt.sheetSize = (SheetSize)Enum.Parse(typeof(SheetSize), node.SelectSingleNode("./SheetSize").InnerText.ToString());
                tmpt.actualWidth = Int32.Parse(node.SelectSingleNode("./ActualWidth").InnerText);
                tmpt.actualHeight = Int32.Parse(node.SelectSingleNode("./ActualHeight").InnerText);
                tmpt.numofQuestions = Int32.Parse(node.SelectSingleNode("./NumofQuestion").InnerText);
                tmpt.minBlobRatio = TemplateManager.minBlobRatio;
                tmpt.maxBlobRatio = TemplateManager.maxBlobRatio;
                tmpt.startingContrast = TemplateManager.startingContrast;
                tmpt.backgroundFill = TemplateManager.backgroundFill;
                tmpt.minBlobToDetect = TemplateManager.minBlobToDetect;
                tmpt.imageMatch = TemplateManager.imageMatch;
                tmpt.treshValue = TemplateManager.treshValue;

                tmpt.numofBlocks = Int32.Parse(node.SelectSingleNode("./NumOfBlocks").InnerText);

                XmlNodeList children = node.SelectNodes("./Blocks/Block");
                List<Block> listBlock = new List<Block>();

                foreach (XmlNode child in children)
                {
                    Block bl = new Block();
                    bl.name = child.Attributes["Name"].Value;
                    bl.Type = (BlockType)Enum.Parse(typeof(BlockType), child.Attributes["BlockType"].Value);
                    bl.Direction = (MarkingDirection)Enum.Parse(typeof(MarkingDirection), child.Attributes["MarkingDirection"].Value);
                    bl.Group = child.Attributes["Group"] != null ? child.Attributes["Group"].Value : null;
                    bl.startingQuestion = child.Attributes["StartingQuestion"] != null ? Int32.Parse(child.Attributes["StartingQuestion"].Value) : 1;
                    bl.X = Int32.Parse(child.SelectSingleNode("./X").InnerText);
                    bl.Y = Int32.Parse(child.SelectSingleNode("./Y").InnerText);
                    bl.Width = Int32.Parse(child.SelectSingleNode("./Width").InnerText);
                    bl.Height = Int32.Parse(child.SelectSingleNode("./Height").InnerText);
                    bl.numberofQuestions = Int32.Parse(child.SelectSingleNode("./NumofQuestion").InnerText);
                    bl.numberofSelections = Int32.Parse(child.SelectSingleNode("./NumofSelection").InnerText);
                    bl.startingNumberofAnswer = Int32.Parse(child.SelectSingleNode("./SelectionStartingNumber").InnerText);

                    XmlNodeList grandChildren = child.SelectNodes("./Items/Item");
                    bl.items = new List<string>();
                    bl.itemtype = (SelectionItemType)Enum.Parse(typeof(SelectionItemType), child.SelectSingleNode("./Items").Attributes["type"].Value);

                    foreach (XmlNode grandchild in grandChildren)
                    {
                        bl.items.Add(grandchild.InnerText);
                    }

                    listBlock.Add(bl);
                }

                tmpt.Blocks = listBlock;


                templetes.Add(tmpt);

            }

            return "";
        }



    }

    /// <summary>
    /// OMRSheet 속성 클래스 
    /// </summary>
    public class Template
    {
        public string code;               // 고유코드
        public string description;         // Sheet설명
        public SheetSize sheetSize;       // SheetSize A3,A4...
        public int actualWidth;           // 실제 인식영역 너비 from LeftMark to RightMark 
        public int actualHeight;          // 실제 인식영역 높이 from TopMark to BottomMark
        public int numofQuestions;        // 전체 문항수 (실제 사용되지는 않으나 참고값임)
        public double minBlobRatio;       // 최대크기 마킹식별을 위해 인식하는 최대 검은색 부위의 비율(전체 용지크기 대비) 
        public double maxBlobRatio;       // 최소크기 마킹식별을 위해 인식하는 최소 검은색 부위의 비율(전체 용지크기 대비)
        public int startingContrast;      // Recognition 시작시 Contrast 인식실패시 조금씩 변경하여 반복함
        public int backgroundFill;        // Recognition 시작시 Fill Factor 인식실패시 조금씩 변경하여 반복함
        public int minBlobToDetect;       // 인식할 최소 검은색부위  이보다 적은 숫자가 인식되면 실패로 간주함
        public int imageMatch;            // 이미지가 동일한지 판별 기준값 작으면 얼추 비슷해도 인식될수도 있음
        public int treshValue;            // Recognition 시 사용되는 treshvalue 이미지처리에 사용되는 값이나 정확한 의미는 모름
        public int numofBlocks;           // Sheet에서 마킹을 인식한 Block의 수  (실제 사용되지는 않으나 참고값임)
        public List<Block> Blocks;        // Block 리스트

        public string tag;                // 에디터에서 변경여부 체크
    }

    /// <summary>
    /// OMR Sheet의 각 인식영역블럭 Class
    /// </summary>
    public class Block
    {
        public string name;                     // 블럭명
        public BlockType Type;                  // 블럭타입
        public MarkingDirection Direction;      // 마킹방향 가로 or 세로
        public int startingQuestion;            // 시작문항번호 (동일과목도 블럭이 달라질수 있으므로 사용)
        public string Group;                    // 동일과목이나 블럭이 다른경우 한그룹으로 묶기위함
        public int X;                           // 좌표값 X
        public int Y;                           // 좌표값 Y
        public int Width;                       // 너비
        public int Height;                      // 높이
        public int numberofQuestions;           // 문항수  - Block영역을 문항갯수로 쪼갤때 사용
        public int numberofSelections;          // 선택갯수  - Block영역을 객관식선택숫자로 쪼갤때 사용
        public int startingNumberofAnswer;      // 문항 시작번호
        public List<string> items;              // 선택옵션
        public SelectionItemType itemtype;      // 아이템유형
    }

    /// <summary>
    /// 시트사이즈 
    /// </summary>
    public enum SheetSize
    {
        A3, A4, A5, B3, B4, B5
    }

    /// <summary>
    /// 블럭유형
    /// </summary>
    public enum BlockType
    {
        STUDENT_NO=0, TEST_TYPE=1, SUBJECT=2, TEST_LEVEL1=3, TEST_LEVEL2 = 4, TEST_NO=5
            , BRANCH_NO=6, GUIDE=7, TOTALSCORE=8, ANSWER=99
    }

    /// <summary>
    /// 답안마킹방향
    /// </summary>
    public enum MarkingDirection
    {
        V, H
    }

    /// <summary>
    /// 시험유형
    /// </summary>
    public enum TestType
    {
        ENTRANCE, SELECTIVE_TRIAL_TEST, SCHOLARSHIP_TEST, OC_TRIAL_TEST, CLASS_TEST, DUMMY, BLENDED_OCTT, BLENDED_STT
    }

    /// <summary>
    /// 테스트과목
    /// </summary>
    public enum TestSubject
    {
        MATHS, THINKING, READING, VOCA, TEXTTYPE, EXTRATS, GAPRACTICE, SCIENCE, OTHERS
    }

    /// <summary>
    /// 테스트레벨1  - 시트의 테스트레벨이 한번에 인식될수 없어 블럭을 분리
    /// </summary>
    public enum TestLevel1
    {
        YEAR1, YEAR2, YEAR3, YEAR4, YEAR5, YEAR6 
    }

    /// <summary>
    /// 테스트레벨2  - 시트의 테스트레벨이 한번에 인식될수 없어 블럭을 분리
    /// </summary>
    public enum TestLevel2
    {
        YEAR7, YEAR8, YEAR9, YEAR10, YEAR11, YEAR12
    }

    /// <summary>
    /// OMR Processing Error Type
    /// </summary>
    public enum MarkingErrorType
    {
        NOT_PROCESSED, CHECK_ERROR, LOW_MEMORY, BAD_IMAGE, RECOG_OK, DIFFERENT_HEADER, INSUFFICIENT_INFO, WRONG_STUDENTNO, NO_ANSWER, MULTI_ANSWER
        , MISSING_GUIDESCORE, SCORE_OK, SERVER_ERROR, SEND_OK, ETC
    }

    /// <summary>
    /// Marking Guide Category
    /// </summary>
    public enum WritingGuide
    {
        CONTENT, CREATIVITY, STRUCTURE_ORGANIZATION, EXPRESSION_LANG, LANG_SPELLING, LANG_GRAMMAR, LANG_PUNCTUATION 
    }


    public enum SelectionItemType
    {
        num, combo, multi
    }

}
