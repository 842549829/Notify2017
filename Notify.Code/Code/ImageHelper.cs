using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Notify.Code.Code
{
    /// <summary>
    /// ImageHelper
    /// </summary>	
    public class ImageHelper
    {
        /// <summary>
        /// Gets the reduced image.
        /// </summary>
        /// <param name="srouceImage">The srouce image.</param>
        /// <param name="smallHeight">Height of the small.</param>
        /// <param name="retInfo">The ret information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetReducedImage(string srouceImage, int smallHeight, ref string retInfo)
        {
            if (retInfo == null)
            {
                throw new ArgumentNullException(nameof(retInfo));
            }
            Image ReducedImage = null;
            Image ResourceImage = null;
            retInfo = string.Empty;
            try
            {
                string smallfilename = Path.GetFileNameWithoutExtension(srouceImage) + "_small" + Path.GetExtension(srouceImage);

                string targetFilePath = Path.GetDirectoryName(srouceImage) + Path.DirectorySeparatorChar + smallfilename;

                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                ResourceImage = Image.FromFile(srouceImage);

                int ImageHeight = smallHeight;

                int ImageWidth = Convert.ToInt32((float)smallHeight / ResourceImage.Height * ResourceImage.Width);

                Image.GetThumbnailImageAbort callb = ThumbnailCallback;

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                return true;
            }
            catch (System.Exception ex)
            {
                retInfo = ex.Message;
                return false;
            }
            finally
            {
                ReducedImage?.Dispose();
                ResourceImage?.Dispose();
            }
        }

        /// <summary>
        /// 生成图片缩略图
        /// </summary>
        /// <param name="srouceImage">原始图片路径</param>
        /// <param name="thumbnailHeight">缩略图自动key 新生成的图片路径后缀 value新图片的高度</param>
        /// <param name="retInfo">失败信息</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetReducedImage(string srouceImage, Dictionary<string, int> thumbnailHeight, ref string retInfo)
        {
            if (retInfo == null)
            {
                throw new ArgumentNullException(nameof(retInfo));
            }
            Image ReducedImage = null;
            Image ResourceImage = null;
            retInfo = string.Empty;
            try
            {
                string smallfilepath = Path.GetDirectoryName(srouceImage) + Path.DirectorySeparatorChar;

                string filename = Path.GetFileNameWithoutExtension(srouceImage);
                if (Directory.Exists(smallfilepath))
                {
                    Directory.CreateDirectory(smallfilepath);
                }

                ResourceImage = Image.FromFile(srouceImage);

                foreach (var item in thumbnailHeight)
                {
                    int ImageHeight = item.Value;

                    int ImageWidth = Convert.ToInt32((float)ImageHeight / ResourceImage.Height * ResourceImage.Width);

                    Image.GetThumbnailImageAbort callb = ThumbnailCallback;

                    ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                    ImageFormat imageFormat = ImageFormat.Jpeg;

                    switch (Path.GetExtension(srouceImage).ToLower())
                    {
                        case ".png":
                        {
                            imageFormat = ImageFormat.Png;
                            break;
                        }
                        case ".ico":
                        {
                            imageFormat = ImageFormat.Icon;
                            break;
                        }
                        case ".bmp":
                        {
                            imageFormat = ImageFormat.Bmp;
                            break;
                        }
                    }
                    ReducedImage.Save(smallfilepath + filename + "_" + item.Key + Path.GetExtension(srouceImage), imageFormat);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                retInfo = ex.Message;
                return false;
            }
            finally
            {
                ReducedImage?.Dispose();
                ResourceImage?.Dispose();
            }
        }

        /// <summary>
        /// 按比例生成缩略图
        /// </summary>
        /// <param name="srouceImage">原始图片路径 全路径</param>
        /// <param name="ThumbnaPercent">缩略图百分比 0.01-1</param>
        /// <param name="retInfo">返还信息</param>
        /// <returns></returns>
        public static bool GetReducedImageByPercent(string srouceImage, float ThumbnaPercent, ref string retInfo)
        {
            if (retInfo == null) throw new ArgumentNullException(nameof(retInfo));
            Image ReducedImage = null;
            Image ResourceImage = null;
            retInfo = string.Empty;
            try
            {
                string smallfilename = Path.GetFileNameWithoutExtension(srouceImage) + "_small" + Path.GetExtension(srouceImage);

                string targetFilePath = Path.GetDirectoryName(srouceImage) + Path.DirectorySeparatorChar + smallfilename;

                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                ResourceImage = Image.FromFile(srouceImage);

                int ImageHeight = Convert.ToInt32(ResourceImage.Height * ThumbnaPercent);

                int ImageWidth = Convert.ToInt32(ResourceImage.Width * ThumbnaPercent);

                Image.GetThumbnailImageAbort callb = ThumbnailCallback;

                ReducedImage = ResourceImage.GetThumbnailImage(ImageWidth, ImageHeight, callb, IntPtr.Zero);

                ReducedImage.Save(@targetFilePath, ImageFormat.Jpeg);

                return true;
            }
            catch (System.Exception ex)
            {
                retInfo = ex.Message;
                return false;
            }
            finally
            {
                ReducedImage?.Dispose();
                ResourceImage?.Dispose();
            }
        }

        /// <summary>
        /// 缩略图回调方法
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ThumbnailCallback()
        {
            return false;
        }
    }
}
