using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Resources;

namespace CSedu.OMR
{
    public class ControlHandler
    {
        private int horizontalMidPoint;
        private int verticalMidPoint;
        private Point[] locations;
        Image bit = Image.FromFile("move.jpg");
        Image bit2 = Image.FromFile("x.jpg");
        public frmEditor parentForm = null;

        private Control activeControl = null;
        public Control ActiveControl
        {
            get { return activeControl; }
        }

        private Control activeControlParent = null;

        public struct boundControl
        {
            public Control mControl;
            public Control mParent;
            public boundControl(Control parent, Control control)
            {
                mControl = control;
                mParent = parent;
            }
        }

        private List<boundControl> _boundControls = new List<boundControl>();
        public List<boundControl> boundControls
        {
            get { return _boundControls; }
        }

        public void AddBoundControl( Control parent, Control child )
        {
            this._boundControls.Add( new boundControl(parent, child));
            this.activeControlParent = parent;
            this.activeControl = child;

            locations = getLocations(child);
            addDragHandles();
            paintresize();
            canDeselect = true;
        }


        private Rectangle _boundingRectangle;
        public Rectangle boundingRectangle
        {
            get { return _boundingRectangle;  }
            set { _boundingRectangle = value; }
        }

        private bool _canDeselect;
        public bool canDeselect
        {
            get { return _canDeselect;  }
            set { _canDeselect = value; }
        }


        public void bindControls()
        {
            for (int i = 0; i < _boundControls.Count; i++)
            {
                Control ctrl = _boundControls[i].mControl;
                Control parent = _boundControls[i].mParent;
                ctrl.MouseDown += ctrl_MouseDown;
                ctrl.Paint += ctrl_Paint;
                ctrl.Resize += ctrl_Resize;
                ctrl.Move += ctrl_Move;
            }
        }

        PictureBox[] pb = new PictureBox[10];
        public void deselectAll()
        {
            if (activeControl != null && canDeselect)
            {
                activeControl = null;
                //boundingRectangle = null;
                for (int i = 0; i < 10; i++)
                {
                    pb[i].Dispose();
                }

                activeControlParent.Invalidate();
            }
        }

        private void addDragHandles()
        {
            Cursor[] sizeCursor = { Cursors.SizeNWSE, Cursors.SizeNS, Cursors.SizeNESW, Cursors.SizeWE
                                  , Cursors.SizeNWSE, Cursors.SizeNS, Cursors.SizeNESW, Cursors.SizeWE };

            int[] smConstants = { HTTOPLEFT, HTTOP, HTTOPRIGHT, HTRIGHT, HTBOTTOMRIGHT
                                 , HTBOTTOM, HTBOTTOMLEFT, HTLEFT };

            for (int i = 0; i < 8; i++)
            {
                pb[i] = new PictureBox();
                pb[i].BackColor = Color.White;
                pb[i].Size = new Size(6,6);
                pb[i].BorderStyle = BorderStyle.FixedSingle;
                pb[i].Location = locations[i];
                pb[i].Cursor = sizeCursor[i];
                pb[i].Tag = smConstants[i];
                activeControlParent.Controls.Add(pb[i]);
                pb[i].BringToFront();
                pb[i].MouseDown += ControlHandler_MouseDown;
                pb[i].MouseMove += ControlHandler_MouseMove;
            }

            pb[8] = new PictureBox();
            pb[8].Size = new Size(15, 15);
            pb[8].BorderStyle = BorderStyle.None;
            pb[8].Location = locations[8];
            pb[8].Cursor = Cursors.SizeAll;
            pb[8].Image = bit;
            activeControlParent.Controls.Add(pb[8]);
            pb[8].BringToFront();
            pb[8].MouseDown +=ControlHandler_MouseDown8;
            pb[8].MouseMove +=ControlHandler_MouseMove8;


            pb[9] = new PictureBox();
            pb[9].Size = new Size(15, 15);
            pb[9].BorderStyle = BorderStyle.None;
            pb[9].Location = locations[9];
            pb[9].Cursor = Cursors.Arrow;
            pb[9].Image = bit2;
            activeControlParent.Controls.Add(pb[9]);
            pb[9].BringToFront();
            pb[9].Click += ControlHandler_Click9;


        }

        void ControlHandler_Click9(object sender, EventArgs e)
        {

            this.activeControl.Dispose();
            activeControl = null;

            for (int i = 0; i < 10; i++)
            {
                pb[i].Dispose();
            }

            activeControlParent.Invalidate();
            paintresize();

        }




        private bool mouseOnHandle = false;
        private bool mouseOnMove = false;

        void ControlHandler_MouseMove8(object sender, MouseEventArgs e)
        {
            if (mouseOnMove)
            {
                ReleaseCapture();
                SendMessage(activeControl.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                if (GetCapture() == 0)
                {
                    mouseOnMove = false;
                    for (int i = 0; i < 10; i++)
                    {
                        pb[i].BringToFront();
                    }
                }
                paintresize();
            }
        }

        void ControlHandler_MouseDown8(object sender, MouseEventArgs e)
        {
            mouseOnMove = true;
        }

        void ControlHandler_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseOnHandle)
            {
                ReleaseCapture();
                SendMessage(activeControl.Handle, WM_NCLBUTTONDOWN, Int32.Parse(((PictureBox)sender).Tag.ToString()), 0);
                if (GetCapture() == 0)
                {
                    mouseOnHandle = false;
                    Application.DoEvents();
                }

                paintresize();
            }
        }

        void ControlHandler_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOnHandle = true;
        }

        private void paintresize()
        {

            locations = getLocations(activeControl);

            for (int i = 0; i < 10; i++)
            {
                pb[i].Location = locations[i];
            }


        }


        void ctrl_Move(object sender, EventArgs e)
        {
            drawBoundingRectangle();
            paintresize();
            
        }

        void ctrl_Resize(object sender, EventArgs e)
        {
            drawBoundingRectangle();
            paintresize();
        }

        void ctrl_Paint(object sender, PaintEventArgs e)
        {
            if (sender == activeControl)
            {
                drawBoundingRectangle();
            }
        }

        private Point[] getLocations(Control ctrl)
        {
            if (ctrl == null)
            {
                return locations;
            }
            horizontalMidPoint = (ctrl.Left + ctrl.Width / 2);
            verticalMidPoint = (ctrl.Top + ctrl.Height / 2);

            locations = new Point[10]
            {
                new Point(ctrl.Left - 3, ctrl.Top - 3)
                , new Point(horizontalMidPoint - 3, ctrl.Top - 3)
                , new Point(ctrl.Right - 2, ctrl.Top - 3)
                , new Point(ctrl.Right-2, verticalMidPoint - 3)
                , new Point(ctrl.Right-2, ctrl.Bottom-2)
                , new Point(horizontalMidPoint-3, ctrl.Bottom-2)
                , new Point(ctrl.Left-3, ctrl.Bottom-2)
                , new Point(ctrl.Left-3, verticalMidPoint-3)
                , new Point(ctrl.Left+9, ctrl.Top-10)
                , new Point(ctrl.Right+2, ctrl.Top+2)
            };

            return locations;
        }

        void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (activeControlParent != null)
            {
                activeControlParent.Invalidate(true);
            }


            Control newCtrl = (Control)sender;

            locations = getLocations(newCtrl);

            horizontalMidPoint = (newCtrl.Left + newCtrl.Width / 2);
            verticalMidPoint = (newCtrl.Top + newCtrl.Height / 2);


            if (activeControl != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    pb[i].Dispose();
                }
            }

            activeControl = newCtrl;


            for (int i = 0; i < _boundControls.Count; i++)
            {
                if (_boundControls[i].mControl == activeControl)
                {
                    activeControlParent = _boundControls[i].mParent;
                }
            }


            drawBoundingRectangle();
            addDragHandles();

        }


        private void drawBoundingRectangle()
        {
            Rectangle r = activeControl.Bounds;
            r.Inflate(1, 1);
            boundingRectangle = r;
            activeControlParent.Invalidate();
            this.parentForm.DimensionChanged();
        }


        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();


        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int GetCapture();

        private const int WM_NCLBUTTONDOWN = 0x00A1;//&HA1;
        private const int HTBORDER = 18;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTCAPTION = 2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;

    }


}
