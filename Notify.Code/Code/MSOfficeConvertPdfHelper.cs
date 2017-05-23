using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notify.Code.Code
{
    /// <summary>
    /// 将Office系列软件(Office,World)转换成PDF格式的文档和图片形式存放
    /// </summary>
    public static class MSOfficeConvertPdfHelper
    {
        /// <summary>
        /// 将Office(World,PPT)系列软件转化PDF
        /// </summary>
        /// <param name="sourcePath">源文件地址 (需要转化的World，PPT的文件地址)，包括文件</param>
        /// <param name="pdfPath">存放转换后PDF的地址</param>
        public static void OfficeConvertPdfInfo(string sourcePath, string pdfPath)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(pdfPath))
            {
                throw new ArgumentException("您没有传递需要转换的地址，请您检查");
            }
            //第一步：将world，ppt的后缀存放成集合
            string[] worldExt = { ".doc", ".docx" };
            string[] pptExt = { ".ppt", ".pptx" };

            //第二步：获取文件的后缀名称
            string fileExt = Path.GetExtension(sourcePath).ToLower();

            //第三步：将World,PPT进行转换
            if (worldExt.Contains(fileExt)) //将World转换成Pdf
            {
                WorldConvertPdf(sourcePath, pdfPath);
            }
            else if (pptExt.Contains(fileExt)) //将PPT转换成Pdf
            {
                PptConvertPdf(sourcePath, pdfPath);
            }
            else
            {
                throw new System.Exception("该格式的MS Office 暂时不能转换为PDF!");
            }
        }

        /// <summary>
        /// 将World文件转化成Pdf(提供给OfficeConvertPdf方法调用)(使用插件Aspose.Words.Dll进行转换)
        /// </summary>
        /// <param name="worldPath">源存放world文件地址</param>
        /// <param name="pdfPath">转换后存放Pdf文件的地址</param>
        private static void WorldConvertPdf(string worldPath, string pdfPath)
        {
            try
            {
                if (!File.Exists(worldPath))
                {
                    throw new System.Exception("不存在源文件，World不存在");
                }
                if (string.IsNullOrEmpty(pdfPath))
                {
                    throw new System.Exception("转换存放PDF的路径不能为空");
                }
                var document = new Document(worldPath);
                document.Save(pdfPath, SaveFormat.Pdf);
            }
            catch (System.Exception exception)
            {
                throw new System.Exception("World转换PDF出现错误了，错误原因：" + exception.Message);
            }
        }

        /// <summary>
        /// 将PPT文件转化成Pdf(提供给OfficeConvertPdf方法调用)(使用插件：Microsoft.Office.Interop.PowerPoint.Dll,Office.Dll)
        /// </summary>
        /// <param name="pptPath">源存放Ppt文件地址</param>
        /// <param name="pdfPath">转换后存放Pdf文件的地址</param>
        private static void PptConvertPdf(string pptPath, string pdfPath)
        {
            try
            {
                if (!File.Exists(pptPath))
                {
                    throw new System.Exception("不存在源文件，PPT不存在");
                }
                if (string.IsNullOrEmpty(pdfPath))
                {
                    throw new System.Exception("转换存放PDF的路径不能为空");
                }
                //进行转换
                var powerPoint = new PowerPoint.Application(); //插件：Microsoft.Office.Interop.PowerPoint.Dll
                var objPresentations = powerPoint.Presentations.Open(pptPath, MsoTriState.msoFalse, MsoTriState.msoFalse,
                    MsoTriState.msoTrue); //插件：Office.Dll
                //调用转换PPT转换Pdf的方法
                To_PptConvertPdf(objPresentations, pdfPath);

            }
            catch (System.Exception exception)
            {
                throw new System.Exception("PPT转换PDF出现错误了，错误原因：" + exception.Message);
            }
        }

        /// <summary>
        /// 将PPT转换成PDF的方法实现
        /// </summary>
        /// <param name="objPresentations">PPT的实例化对象</param>
        /// <param name="pdfPath">存放转换后的Pdf的路径</param>
        private static void To_PptConvertPdf(object objPresentations, string pdfPath)
        {
            var powerPoint = (PowerPoint.Presentation)objPresentations;
            powerPoint.SaveAs(pdfPath, PowerPoint.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
            powerPoint.Close();
        }

        /// <summary>
        /// 将Office转化成图片存放
        /// </summary>
        /// <param name="sourcePath">源Office路径</param>
        /// <param name="imagePath">转换成图片存放图片的路径</param>
        public static void OfficeConvertImagesInfo(string sourcePath, string imagePath)
        {
            var document = new Document(sourcePath);
            using (Stream stream = new MemoryStream())
            {
                document.Save(stream, SaveFormat.Jpeg);
                using (Image image = Image.FromStream(stream)) //原始图片
                {
                    using (Bitmap bitmap = new Bitmap(image, 400, 300))
                    {
                        bitmap.Save(imagePath + "/" + "test.jpg");
                    }
                }
            }
        }

        /// <summary>
        /// 将PPT转换成图片存放(插件：Aspose.Slides.dll)
        /// </summary>
        /// <param name="pptPath">源PPT文件路径</param>
        /// <param name="imagePath">存放转换后的图片路径</param>
        /// <returns>返回总共转换了多少张图片</returns>
        public static int PptConvertImagesInfo(string pptPath, string imagePath)
        {
            var presentation = new Presentation(pptPath);
            var endSliderCount = presentation.Slides.Count;
            //循环导出所有的图片信息
            for (int i = 0; i < endSliderCount + 1; i++)
            {
                Slide slide = presentation.GetSlideByPosition(i);
                if (slide == null) continue;
                using (var memoryStream = new MemoryStream())
                {
                    slide.GetThumbnail(new Size(400, 400)).Save(imagePath + "/" + i + ".jpg");
                }
            }
            return endSliderCount;
        }

        /// <summary>
        /// 将Pdf转换成图片存放在指定的文件夹下面并且返回转换了多少张图片(插件：O2S.Components.PDFRender4NETd)
        /// (使用：调用此方法传递参数，最后一个参数为模糊度(参考传递30))
        /// </summary>
        /// <param name="pdfInputPath">源PDF路径</param>
        /// <param name="imageOutpPutPath">PDF转化成图片之后存放的路径</param>
        /// <param name="imageFormat">转换的图片格式</param>
        /// <param name="difinition">传递参数模糊度</param>
        /// <returns>返回转换了多少张图片信息</returns>
        public static int PdfConvertImageInfo(string pdfInputPath, string imageOutpPutPath, ImageFormat imageFormat,
            float difinition)
        {
            try
            {
                //第一步：判断需要转换的PDF是否存在
                if (!File.Exists(pdfInputPath))
                {
                    throw new System.Exception("PDF的路径不存在");
                }
                //第二步：判断存放转换后图片的地址是否存在
                if (!Directory.Exists(imageOutpPutPath))
                {
                    throw new System.Exception("存放图片的路径不存在");
                }
                //第三步：进行转换(使用插件O2S.Components.PDFRender4NET)
                PDFFile pdfFile = PDFFile.Open(pdfInputPath);
                int pageCount = pdfFile.PageCount;
                for (int i = 0; i < pageCount; i++)
                {
                    Bitmap bitmap = pdfFile.GetPageImage(i, difinition);
                    bitmap.Save(Path.Combine(imageOutpPutPath, i + "." + imageFormat), imageFormat);
                    bitmap.Dispose();
                }
                pdfFile.Dispose();
                //第四步：返回PDF转换了多少张图片的个数
                return pageCount;
            }
            catch (System.Exception exception)
            {

                throw new System.Exception("PDF转换图片出现错误了，错误原因：" + exception.Message);
            }
        }
    }
}
