using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using AForge.Math.Geometry;

using OMR;
using System.Runtime.InteropServices;

namespace CSedu.OMR
{
    public class ImageManager
    {

        
        /// <summary>
        /// // get each image from tif file.
        /// </summary>
        /// <param name="file">로드할 파일</param>
        /// <param name="images">로드하여 리턴할 이미지</param>
        /// <returns>로드하여 리턴할 이미지 파라미터와 중복처리되고 있음</returns>
        static public List<MarkingSheet> GetAllPages(string file, ref List<MarkingSheet> images)
        {
            Bitmap bitmap = null;
            try
            {
                bitmap = (Bitmap)System.Drawing.Image.FromFile(file);
            }
            catch 
            {
                // Not image file then just return;
                return images;
            }
            int count = bitmap.GetFrameCount(FrameDimension.Page);

            for (int idx = 0; idx < count; idx++)
            {
                // save each frame to a bytestream
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);

                MarkingSheet rslt = new MarkingSheet();
                Bitmap bt = (Bitmap)System.Drawing.Image.FromStream(byteStream);

                char tempch = '\\';
                string[] filepath = file.Split(tempch);

                rslt.FileName = filepath[filepath.Length-1];
                rslt.sheet = bt;
                rslt.StudentID = "";

                images.Add(rslt);
            }

            return images;
        }

        
        /// <summary>
        /// // get each image from selected folder.
        /// </summary>
        /// <param name="folder">로드할 폴더</param>
        /// <param name="images">로드하여 리턴할 리스트</param>
        /// <returns>로드하여 리턴할 리스트</returns>
        static public List<MarkingSheet> GetAllImgFiles(string folder, ref List<MarkingSheet> images)
        {
            System.IO.DirectoryInfo dirInfo = new DirectoryInfo(folder);

            System.IO.FileInfo[] files = dirInfo.GetFiles();

            foreach (FileInfo file in files)
            {
                try
                {
                    GetAllPages(file.FullName, ref images);
                }
                catch(Exception ex){ 
                    // do nothing 
                }
            }

            return images;
        }

        

        /// <summary>
        /// // get each image from selected files.
        /// </summary>
        /// <param name="files">로드할 파일 리스트</param>
        /// <param name="images">로드하여 리턴할 리스트</param>
        /// <returns>로드하여 리턴할 리스트</returns>
        static public List<MarkingSheet> GetAllImgFiles(string[] files, ref List<MarkingSheet> images)
        {

            foreach (string file in files)
            {
                try
                {
                    GetAllPages(file, ref images);
                }
                catch (Exception ex)
                {
                    // do nothing 
                }
            }

            return images;
        }

        /// <summary>
        /// Save Images to TIFF file format
        /// </summary>
        /// <param name="bmp">처리할 비트맵 이미지</param>
        /// <param name="location">저장할 위치</param>
        /// <param name="type">저장시 코덱유형</param>
        /// <returns></returns>
        static public bool saveMultipage(List<Bitmap> bmp, string location, string type)
        {

            if (bmp != null)
            {
                long compressionvalue = (long)EncoderValue.CompressionLZW;
                long colordepth = 8L;   // image color depth is 8bit. adjust this value to change image quality.
                bool greyscale = false;
                bool resize = true;

                try
                {
                    ImageCodecInfo codecInfo = getCodecForstring(type);

                    if (bmp.Count == 1)
                    {
                        EncoderParameters iparams = new EncoderParameters(2);
                        System.Drawing.Imaging.Encoder iparam = System.Drawing.Imaging.Encoder.Compression;
                        System.Drawing.Imaging.Encoder iparam2 = System.Drawing.Imaging.Encoder.ColorDepth;
                        EncoderParameter iparamPara = new EncoderParameter(iparam, compressionvalue);
                        iparams.Param[0] = iparamPara;
                        iparams.Param[1] = new EncoderParameter(iparam2, colordepth);

                        if (greyscale)
                        {
                            bmp[0] = MakeGrayscale(bmp[0]);
                        }

                        if (resize)
                        {
                            bmp[0].SetResolution(100, 100);
                        }

                        bmp[0].Save(location, codecInfo, iparams);
                    }
                    else if (bmp.Count > 1)
                    {

                        System.Drawing.Imaging.Encoder saveEncoder;
                        System.Drawing.Imaging.Encoder compressionEncoder;
                        System.Drawing.Imaging.Encoder colorDepth;
                        EncoderParameter SaveEncodeParam;
                        EncoderParameter CompressionEncodeParam;
                        EncoderParameters EncoderParams = new EncoderParameters(3);

                        saveEncoder = System.Drawing.Imaging.Encoder.SaveFlag;
                        compressionEncoder = System.Drawing.Imaging.Encoder.Compression;
                        colorDepth = System.Drawing.Imaging.Encoder.ColorDepth;

                        // Save the first page (frame).
                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.MultiFrame);
                        CompressionEncodeParam = new EncoderParameter(compressionEncoder, compressionvalue);
                        EncoderParams.Param[0] = CompressionEncodeParam;
                        EncoderParams.Param[1] = SaveEncodeParam;
                        EncoderParams.Param[2] = new EncoderParameter(colorDepth, colordepth);

                        File.Delete(location);

                        if (greyscale)
                        {
                            bmp[0] = MakeGrayscale(bmp[0]);
                        }
                        if (resize)
                        {
                            bmp[0].SetResolution(100, 100);
                        }


                        bmp[0].Save(location, codecInfo, EncoderParams);

                        for (int i = 1; i < bmp.Count; i++)
                        {
                            if (bmp[i] == null)
                                break;

                            SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.FrameDimensionPage);
                            CompressionEncodeParam = new EncoderParameter(compressionEncoder, compressionvalue);
                            EncoderParams.Param[0] = CompressionEncodeParam;
                            EncoderParams.Param[1] = SaveEncodeParam;
                            EncoderParams.Param[2] = new EncoderParameter(colorDepth, colordepth);

                            if (greyscale)
                            {
                                bmp[i] = MakeGrayscale(bmp[i]);
                            }
                            if (resize)
                            {
                                bmp[i].SetResolution(100, 100);
                            }

                            bmp[0].SaveAdd(bmp[i], EncoderParams);

                        }

                        EncoderParams = new EncoderParameters(1);
                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.Flush);
                        EncoderParams.Param[0] = SaveEncodeParam;
                        bmp[0].SaveAdd(EncoderParams);
                    }
                    return true;


                }
                catch (System.Exception ee)
                {
                    throw new Exception(ee.Message + "  Error in making tif file");
                }
            }
            else
                return false;
        }

        /// <summary>
        /// 이미지코덱정보를 가져오는 메서드
        /// </summary>
        /// <param name="type">코덱유형 문자열</param>
        /// <returns>리턴되는 코덱유형</returns>
        static private ImageCodecInfo getCodecForstring(string type)
        {
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < info.Length; i++)
            {
                string EnumName = type.ToString();
                if (info[i].FormatDescription.Equals(EnumName))
                {
                    return info[i];
                }
            }

            return null;

        }

        /// <summary>
        /// To reduce file size, make grayscale image.
        /// </summary>
        /// <param name="original">원본이미지</param>
        /// <returns>변환된 이미지</returns>
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
          {
             new float[] {.3f, .3f, .3f, 0, 0},
             new float[] {.59f, .59f, .59f, 0, 0},
             new float[] {.11f, .11f, .11f, 0, 0},
             new float[] {0, 0, 0, 1, 0},
             new float[] {0, 0, 0, 0, 1}
          });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

    }

}
