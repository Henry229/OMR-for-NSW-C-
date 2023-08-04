using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using AForge.Math.Geometry;
using System.IO;
using System.Xml;
using OMR;
using CSedu.OMR;

namespace OMR
{
    // 출처 http://www.codeproject.com/Articles/451169/Csharp-Optical-Marks-Recognition-OMR-Engine-1-0
    // 코드를 수정하여 이미지처리 엔진으로 사용


    public class OpticalReader
    {
        
        /// <summary>
        /// 필기구가 pencil 인 경우
        /// </summary>
        public double multicheck = 1.5;     /*  ( maxcell / currentcell ) <= multicheck 이면 중복체크로 처리  
                                      *  숫자가 높을수록 중복처리가 많아짐
                                      */
        public double notfillcheck = 1.7;    /*  ( maxcell / currentcell ) >= notfillcheck  이면 체크 안했다고 처리  
                                            숫자가 높을수록 체크처리가 많이됨 ??
                                        */
        /// <summary>
        /// black pen 인 경우
        /// </summary>
        public double black_ratio = 0.14;  

        AForge.Imaging.Filters.ContrastCorrection Contrast = new ContrastCorrection(0);
        AForge.Imaging.Filters.Invert invert = new Invert();
        AForge.Imaging.Filters.ExtractChannel extract_channel = new ExtractChannel(0);
        AForge.Imaging.Filters.Threshold thresh_hold = new Threshold(44);
        AForge.Imaging.Filters.BrightnessCorrection brightcorrection = new BrightnessCorrection(-50);

        ColorFiltering colorFilter = new ColorFiltering();

        public OpticalReader()
        {
            colorFilter.Red = new IntRange(0, 0);
            colorFilter.Green = new IntRange(0, 0);
            colorFilter.Blue = new IntRange(0, 0);
            colorFilter.FillOutsideRange = false;
        }



        public Bitmap ExtractOMRSheet(Bitmap basicImage, int fillint, int contint, Template tmpl, ref MarkingErrorType err)
        {
            System.Drawing.Image flattened = (System.Drawing.Image)flatten(basicImage, fillint, contint);

            Bitmap ret = ExtractPaperFromFlattened(new Bitmap(flattened), basicImage, 3, fillint, contint, tmpl, ref err);
            flattened.Dispose();
            GC.Collect();
            return ret;
        }
        /// <summary>
        /// Detects, wrapps and crops out OMR sheet from flattened camera/scanner image.
        /// Flattened image is got by using method,  private Bitmap flatten(Bitmap bmp, int fillint, int contint);         
        /// </summary>
        /// <param name="bitmap">Bitmap image to process</param>
        /// <param name="basicImage">Backup image in case extraction fails</param>
        /// <param name="minBlobWidHei">Pre-configured variable, to be queried from XML reader</param>
        /// <param name="fillint">Pre-configured int, to be queried from XML reader</param>
        /// <param name="contint">Pre-configured int, to be queried from XML reader</param>
        /// <param name="OMRSheets">Sheets XML File Address</param>
        /// <param name="tb">Textbox to give in'process details on</param>
        /// <param name="panel1">Panel to draw in-process changes on.</param>
        /// <param name="giveFB">True when In- Process Feedback is required.</param>
        /// <param name="sheet">Type of sheet from OMREnums</param>
        /// <returns>Cropped OMR sheet (if detected) from camera/scanner image.</returns>
        private Bitmap ExtractPaperFromFlattened(Bitmap bitmap, Bitmap basicImage, int minBlobWidHei, int fillint, int contint
            , Template tmpl, ref MarkingErrorType err)
        {

            // lock image, Bitmap itself takes much time to be processed
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);  

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = minBlobWidHei;  // both these variables have to be given when calling the
            blobCounter.MinWidth = minBlobWidHei;   // method, the can also be queried from the XML reader using OMREnums

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();  

            bitmap.UnlockBits(bitmapData);

            Graphics g = Graphics.FromImage(bitmap);
            Pen yellowPen = new Pen(Color.Yellow, 2);   // create pen in case image extraction failes and we need to preview the
            
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs2 = blobCounter.GetObjects(bitmap, false);

            //Detection of paper lies within the presence of crossmark printed on the corneres of printed sheet.
            // First, detect left edge.
            System.Drawing.Image compImg = System.Drawing.Image.FromFile("lc.jpg"); 
            // lc.jpg = Mirrored image sample as located on the corner of printed sheet
            UnmanagedImage compUMImg = UnmanagedImage.FromManagedImage((Bitmap)compImg);

            // this helps filtering out much smaller and much larger blobs depending upon the size of image.
            // can be queried from XML Reader
            double minbr = tmpl.minBlobRatio;
            double maxbr = tmpl.maxBlobRatio;
            
            List<IntPoint> quad = new List<IntPoint>(); // Store sheet corner locations (if anyone is detected )

            try
            {
                foreach (Blob blob in blobs2)
                {
                    if (
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) > minbr &&
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) < maxbr &&
                            blob.Rectangle.X < (bitmap.Width) / 4) // filters oout very small or very larg blobs
                    {
                        if ((double)blob.Rectangle.Width / blob.Rectangle.Height < 1.4 &&
                            (double)blob.Rectangle.Width / blob.Rectangle.Height > .6) // filters out blobs having insanely wrong aspect ratio
                        {
                            compUMImg = UnmanagedImage.FromManagedImage(ImageUtilities.ResizeImage(compImg, blob.Rectangle.Width, blob.Rectangle.Height));
                            if (isSame(blob.Image, compUMImg))
                            {
                                g.DrawRectangle(yellowPen, blob.Rectangle);
                                quad.Add(new IntPoint((int)blob.CenterOfGravity.X, (int)blob.CenterOfGravity.Y));
                            }
                        }
                    }
                }
            }
            catch (ArgumentException) 
            { 
                err = MarkingErrorType.BAD_IMAGE;
                g.Dispose();
                bitmap.Dispose();
                return basicImage;
            }

            try
            { // Sort out the list in right sequence, UpperLeft,LowerLeft,LowerRight,upperRight
                if ( quad.Count > 1 && quad[0].Y > quad[1].Y)
                {
                    IntPoint tp = quad[0];
                    quad[0] = quad[1];
                    quad[1] = tp;
                }
            }
            catch
            {
            }
            compImg = System.Drawing.Image.FromFile("rc.jpg");
            compUMImg = UnmanagedImage.FromManagedImage((Bitmap)compImg);

            try
            {
                foreach (Blob blob in blobs2)
                {
                    if (
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) > minbr &&
                        ((double)blob.Area) / ((double)bitmap.Width * bitmap.Height) < maxbr &&
                        blob.Rectangle.X > (bitmap.Width * 3) / 4)
                    {
                        if ((double)blob.Rectangle.Width / blob.Rectangle.Height < 1.4 &&
                        (double)blob.Rectangle.Width / blob.Rectangle.Height > .6)
                        {
                            compUMImg = UnmanagedImage.FromManagedImage(ImageUtilities.ResizeImage(compImg, blob.Rectangle.Width, blob.Rectangle.Height));
                            if (isSame(blob.Image, compUMImg))
                            {
                                g.DrawRectangle(yellowPen, blob.Rectangle);
                                quad.Add(new IntPoint((int)blob.CenterOfGravity.X, (int)blob.CenterOfGravity.Y));
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                err = MarkingErrorType.BAD_IMAGE;
                g.Dispose();
                bitmap.Dispose();

                return basicImage;
            }

            try
            {
                if (quad.Count > 3 && quad[2].Y < quad[3].Y)
                {
                    IntPoint tp = quad[2];
                    quad[2] = quad[3];
                    quad[3] = tp;
                }
            }
            catch
            {
                err = MarkingErrorType.BAD_IMAGE;

                g.Dispose();
                bitmap.Dispose();
                return basicImage;
            }

            yellowPen.Dispose();
            g.Dispose();
            //Again, filter out if wrong blobs pretended to our blobs.
            if (quad.Count == 4)
            {
                if (((double)quad[1].Y - (double)quad[0].Y) / ((double)quad[2].Y - (double)quad[3].Y) < .75 ||
                    ((double)quad[1].Y - (double)quad[0].Y) / ((double)quad[2].Y - (double)quad[3].Y) > 1.25)
                    quad.Clear(); // clear if, both edges have insanely wrong lengths
                else if (quad[0].X > bitmap.Width / 2 || quad[1].X > bitmap.Width / 2 || quad[2].X < bitmap.Width / 2 || quad[3].X < bitmap.Width / 2)
                    quad.Clear(); // clear if, sides appear to be "wrong sided"
            }
            if (quad.Count != 4)// sheet not detected, reccurrsive call.
            {
                if (Math.Abs(contint) <= 80)//try altering the contrast correction on both sides of numberline
                {
                    contint = contint + 10; //contint >= 0 ? contint + 10 : contint;
                    //contint = contint * -1;
                    GC.Collect();
                    return ExtractOMRSheet(basicImage, fillint, contint, tmpl, ref err);
                }
                else // contrast correction yeilded no result
                {
                    err = MarkingErrorType.BAD_IMAGE;

                    g.Dispose();
                    bitmap.Dispose();
                    return basicImage;
                }
            }
            else // sheet found
            {
                IntPoint tp2 = quad[3];
                quad[3] = quad[1];
                quad[1] = tp2;
                //sort the edges for wrap operation
                QuadrilateralTransformation wrap = new QuadrilateralTransformation(quad);
                wrap.UseInterpolation = false; //perspective wrap only, no binary.
                Rectangle sr = new Rectangle(0, 0, tmpl.actualWidth, tmpl.actualHeight); 
                wrap.AutomaticSizeCalculaton = false;
                wrap.NewWidth = sr.Width;
                wrap.NewHeight = sr.Height;
                //wrap.Apply(basicImage).Save("LastImg.jpg", ImageFormat.Jpeg); // creat file backup for future use.

                err = MarkingErrorType.RECOG_OK;

                g.Dispose();
                bitmap.Dispose();
                return wrap.Apply(basicImage); // wrap
            }
        }



        private Bitmap flatten(Bitmap bmp, int fillint, int contint)
        {
            // step 1 - turn background to black

            colorFilter.ApplyInPlace(bmp);
            Contrast.Factor = contint;

            Bitmap bmp1 = brightcorrection.Apply(bmp);
            Bitmap bmp2 = Contrast.Apply(bmp1);
            Bitmap bmp3 = extract_channel.Apply(bmp2);
            Bitmap bmp4 = thresh_hold.Apply(bmp3);
            Bitmap bmp5 = invert.Apply(bmp4);

            

            bmp1.Dispose();
            bmp2.Dispose();
            bmp3.Dispose();
            bmp4.Dispose();

            return bmp5;
        }



        private bool isSame(UnmanagedImage img1, UnmanagedImage img2)
        {
            int count = 0, tcount = img2.Width * img2.Height;
            for (int y = 0; y < img1.Height; y++)
                for (int x = 0; x < img1.Width; x++)
                {
                    Color c1 = img1.GetPixel(x, y), c2 = img2.GetPixel(x, y);
                    if ((c1.R + c1.G + c1.B) / 3 > (c2.R + c2.G + c2.B) / 3 - 10 &&
                        (c1.R + c1.G + c1.B) / 3 < (c2.R + c2.G + c2.B) / 3 + 10)
                        count++;
                }
            return (count * 100) / tcount >= 54;
        }
 
        /// <summary>
        /// Reads all the selected options on paper in one call
        /// </summary>
        /// <param name="image">Exctracted Sheet.</param>
        /// <param name="sheet">Type of printed sheet</param>
        /// <param name="OMRSheetFile">XML sheet address</param>
        /// <returns></returns>
        public void getScoreOfSheet(Template tmpl, MarkingSheet rslt, bool blackpen)
        {
            System.Drawing.Image image = rslt.sheet;

            //number of blocks depend upon type of sheet selected
            Rectangle[] Blocks = new Rectangle[tmpl.numofBlocks];
            rslt.Scores = new List<Score>();

            // Read block location from XML file for selected sheet
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new Rectangle(tmpl.Blocks[i].X, tmpl.Blocks[i].Y, tmpl.Blocks[i].Width, tmpl.Blocks[i].Height);
            }

            Graphics g = Graphics.FromImage(image);
            g.DrawRectangles(Pens.Red, Blocks);

            // slice the blocks into lines inside them and record as bitmap
            for (int i = 0; i < tmpl.numofBlocks; i++)
            {
                List<Rectangle> rects = new List<Rectangle>();

                Bitmap[] bmps = SliceOMarkBlock(image, Blocks[i], tmpl.Blocks[i].numberofQuestions
                    , tmpl.Blocks[i].Direction);

                Score score = new Score();

                for (int j = 0; j < bmps.Length; j++)
                {
                    bool[] result = rateSlice(bmps[j], tmpl.Blocks[i].numberofSelections, tmpl.Blocks[i].Direction, blackpen);

                    string strAnswer = "";
                    for (int k = 0; k < result.Length; k++)
                    {
                        if (result[k])
                        {
                            strAnswer += (k + tmpl.Blocks[i].startingNumberofAnswer);
                        }
                    }


                    if (tmpl.Blocks[i].Type == BlockType.ANSWER || tmpl.Blocks[i].Type == BlockType.GUIDE )
                    {
                        score = new Score();
                        score.Answer = "";
                        score.QuestionNo = (tmpl.Blocks[i].startingQuestion + j).ToString();

                        if (tmpl.Blocks[i].Type == BlockType.GUIDE)
                        {
                            score.QuestionNo += ". " + Enum.GetNames(typeof(WritingGuide))[Int32.Parse(score.QuestionNo)];
                        }

                        string tmpsubject = tmpl.Blocks[i].Group != null ? tmpl.Blocks[i].Group : tmpl.Blocks[i].name;
                        score.Subject = rslt.GetRealSubject(tmpsubject);

                        score.blockType = tmpl.Blocks[i].Type;
                        score.area = bmps[j];
                        score.Answer = strAnswer;
                        score.marking = result;
                        score.selection = tmpl.Blocks[i].numberofSelections;

                        rslt.Scores.Add(score);

                    }
                    else
                    {
                        
                        switch (tmpl.Blocks[i].Type)
                        {
                            case BlockType.STUDENT_NO :
                                rslt.StudentID += strAnswer;
                                break;
                            case BlockType.TEST_TYPE :
                                if (rslt.TestType == null && strAnswer != "")
                                {
                                    rslt.TestType += tmpl.Blocks[i].items[Int32.Parse(strAnswer.Substring(0,1))];
                                    if (rslt.TestType == null)
                                    {
                                        rslt.TestType = tmpl.Blocks[i].items[0];
                                    }
                                }
                                break;
                            case BlockType.SUBJECT :
                                for (int k = 0; k < result.Length; k++)
                                {
                                    if (result[k])
                                    {
                                        rslt.TestSubs.Add(tmpl.Blocks[i].items[k]);
                                    }
                                }
                                break;
                            case BlockType.TEST_LEVEL1 :
                            case BlockType.TEST_LEVEL2 :
                                if (strAnswer != "" && rslt.TestLevel == null ) 
                                {
                                    rslt.TestLevel += tmpl.Blocks[i].items[Int32.Parse(strAnswer.Substring(0,1))];

                                    if (rslt.TestLevel == null)
                                    {
                                        rslt.TestLevel = tmpl.Blocks[i].items[0];
                                    }
                                }
                                break;
                            case BlockType.TEST_NO :
                                rslt.TestNo += strAnswer;
                                break;
                            case BlockType.BRANCH_NO :
                                rslt.BranchNo += strAnswer;
                                break;
                            case BlockType.TOTALSCORE:
                                rslt.totalScore += strAnswer;
                                break;

                        }

                    }

                }
            }
            return;
        }


        /// <summary>
        /// returns ant int representation of selected option
        /// </summary>
        /// <param name="slice">Sliced sinlge line in choices block</param>
        /// <param name="OMCount"></param>
        /// <returns></returns>
        private bool[] rateSlice(Bitmap slice, int OMCount, MarkingDirection dir, bool blackpen)
        {
            Rectangle[] cropRects = new Rectangle[OMCount];
            Bitmap[] marks = new Bitmap[OMCount];
            bool[] result = new bool[OMCount];

            for (int i = 0; i < OMCount; i++)
            {
                result[i] = false;
            }

            //sub-devide line into option 
            for (int i = 0; i < OMCount; i++)
            {
                if (dir == MarkingDirection.H)
                {
                    cropRects[i] = new Rectangle(i * slice.Width / OMCount, 0, slice.Width / OMCount, slice.Height);
                }
                else
                {
                    cropRects[i] = new Rectangle(0, i * slice.Height / OMCount, slice.Width, slice.Height / OMCount);
                }
            }

            int crsr = 0;
            foreach (Rectangle cropRect in cropRects)
            {
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(slice, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
                marks[crsr] = target;
                crsr++;
            }
            List<long> fullInks = new List<long>();

            //get marking level
            foreach (Bitmap mark in marks)
            {
                fullInks.Add(InkDarkness(mark, blackpen));
            }



            //if (blackpen)
            {

                // 이미지 상대비교하는 방식에서 절대비교하는 방식으로 수정
                // 4% 이상 검은색이 있으면 체크된것으로 변경

                for (int i = 0; i < fullInks.Count; i++)
                {
                    if (black_ratio * 255.0 < fullInks[i])
                    {
                        result[i] = true;
                    }
                    else
                    {
                        result[i] = false;
                    }
                }
            }

            //else
            //{

            //    int indofMx = -1;
            //    long maxD = 0;

            //    //get maximum ink level
            //    for (int i = 0; i < OMCount; i++)
            //    {
            //        if (fullInks[i] > maxD)
            //        {
            //            maxD = fullInks[i];
            //            indofMx = i;
            //        }
            //    }

            //    result[indofMx] = true;

            //    bool parallelExist = false;
            //    bool[] paralleleach = new bool[OMCount]; 
            //    for (int i = 0; i < OMCount; i++)
            //    {
            //        if (i != indofMx)
            //        {
            //            if ((double)fullInks[indofMx] / (double)fullInks[i] <= multicheck) //both ink levels are nearly the same
            //            {
            //                result[i] = true;

            //                if (tpe) fpe = true;
            //                if (spe) tpe = true;
            //                if (parallelExist) spe = true;
            //                parallelExist = true;
            //            }
            //        }
            //    }
            //    int negScore = parallelExist ? -1 : 0;
            //    negScore = spe ? -2 : negScore;
            //    negScore = tpe ? -3 : negScore;
            //    negScore = fpe ? -4 : negScore;

            //    if (!parallelExist)
            //        return result;

            //    //check if multiple options were selected
            //    bool atleastOneUnfilled = false;
            //    for (int i = 0; i < OMCount; i++)
            //    {
            //        if (i != indofMx)
            //        {
            //            if ((double)fullInks[indofMx] / (double)fullInks[i] >= notfillcheck)
            //                atleastOneUnfilled = true;
            //        }
            //    }
            //    if (atleastOneUnfilled)
            //        return result;

            //    for (int i = 0; i < OMCount; i++)
            //    {
            //        result[i] = false;
            //    }
            //}


            return result;
        }



        private long InkDarkness(Bitmap OMark, bool blackpen)
        {
            //int darkestC = 255, lightestC = 0;

            // Red는 제외

            UnmanagedImage mark = UnmanagedImage.FromManagedImage(OMark);

            int dc = 0;


            for (int y = 0; y < OMark.Height; y++)
                for (int x = 0; x < OMark.Width; x++)
                {
                    Color c = mark.GetPixel(x, y);
                    if (HitTest(x-8, y-6, OMark.Width / 2.0, OMark.Height / 2.0))
                    {
                        if (blackpen)
                        {
                            dc += (255 + 255 - c.G - c.B) / 2;
                            //if ((c.G + c.B) < 64)
                            //{ dc += 64; }   // 255

                            //if ((c.G + c.B) < 128)
                            //{ dc += 64; }   // 255

                            //if ((c.G + c.B) < 192)
                            //{ dc += 64; }

                            //if ((c.G + c.B) < 256)
                            //{ dc += 64; }
                        }
                        else
                        {
                            if ((c.G + c.B) < 128)
                            { dc += 64; }   // 255

                            if ((c.G + c.B) < 192)
                            { dc += 64; }   // 255

                            if ((c.G + c.B) < 256)
                            { dc += 64; }

                            if ((c.G + c.B) < 448)
                            { dc += 64; }
                        }
                    }
                }

            return dc / (long)(    ((long)OMark.Height/(long)2-(long)1) * ((long)OMark.Width/(long)2-(long)3) * (long)3.141592);
        }

        private Bitmap[] SliceOMarkBlock(System.Drawing.Image fullSheet, Rectangle slicer, int slices
            , MarkingDirection dir )
        {
            List<Rectangle> cropRects = new List<Rectangle>();
            Bitmap[] bmps = new Bitmap[slices];

            int remain = dir == MarkingDirection.H ? slicer.Height : slicer.Width;

            for (int i = 0; i < slices; i++)
            {
                if (dir == MarkingDirection.H)
                {
                    cropRects.Add(new Rectangle(slicer.X, slicer.Y + (slicer.Height - remain) , slicer.Width, remain / (slices - i) ));
                    remain = remain - remain / (slices - i); 
                }
                else
                {
                    cropRects.Add(new Rectangle(slicer.X + (slicer.Width - remain), slicer.Y, remain / (slices - i), slicer.Height));
                    remain = remain - remain / (slices - i);
                }
            }

            Bitmap src = (Bitmap)fullSheet;


            // draw rect to check
            Graphics gg = Graphics.FromImage(src);
            foreach (Rectangle rect in cropRects)
            {
                gg.DrawRectangle( new Pen(Color.White, 7), rect);
            }

            int crsr = 0;
            foreach (Rectangle cropRect in cropRects)
            {
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
                bmps[crsr] = target;
                crsr++;
            }
            return bmps;
        }


        /// <summary>
        /// HitTest for Ellipse Area 
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <returns></returns>
        private bool HitTest(double pointX, double pointY, double centerX, double centerY)
        {
            double normalizedX = pointX - centerX;
            double normalizedY = pointY - centerY;

            return ((double)(normalizedX * normalizedX)
                     / (centerX * centerX)) + ((double)(normalizedY * normalizedY) / (centerY * centerY))
                <= 1.0;
        }

        
    }
}
