using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace cs_omr_extraction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            String URLString = "https://www.csonlineschool.com.au/upload/xml/sheets.xml";
            XmlTextReader reader = new XmlTextReader(URLString);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);

                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        Console.Write(">");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }




            string path = System.Windows.Forms.Application.StartupPath;

            if (System.IO.Directory.Exists(path))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                foreach (var item in di.GetFiles())
                {
                    Console.WriteLine(item.Name);
                }
            }


        }
    }
}
