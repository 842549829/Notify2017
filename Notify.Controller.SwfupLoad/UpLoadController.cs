using System;
using System.Web;
using System.Web.Mvc;

namespace Notify.Controller.SwfupLoad
{
    /// <summary>
    /// 上传图片控制器
    /// </summary>
    public class UpLoadController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// 上传图片首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传图片首页
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult IndexView()
        {
            return View();
        }

        /// <summary>
        /// 上传图片处理
        /// </summary>
        /// <param name="fileName">图片流</param>
        /// <returns>结果</returns>
        public ActionResult UpLoading(HttpPostedFileBase fileName)
        {
            string path = string.Empty;
            if (fileName != null)
            {
                //创建图片新的名称
                string nameImg = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //获得上传图片的路径
                string strPath = fileName.FileName;
                //获得上传图片的类型(后缀名)
                string type = strPath.Substring(strPath.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
                if (ValidateImg(type))
                {
                    //拼写数据库保存的相对路径字符串
                    //拼写上传图片的路径
                    var uppath = Server.MapPath("~/Content/Images/UpImgs/");
                    uppath += nameImg + "." + type;
                    path = "/Content/Images/UpImgs/" + nameImg + "." + type;

                    //上传图片
                    fileName.SaveAs(uppath);
                }
            }
            return Content(path);
        }

        /// <summary>
        /// 验证上传图片类型
        /// </summary>
        /// <param name="imgName">图片</param>
        /// <returns>结果</returns>
        public bool ValidateImg(string imgName)
        {
            string[] imgType = { "gif", "jpg", "png", "bmp" };

            int i = 0;
            bool blean = false;

            //判断是否为Image类型文件
            while (i < imgType.Length)
            {
                if (imgName.Equals(imgType[i]))
                {
                    blean = true;
                    break;
                }
                if (i == (imgType.Length - 1))
                {
                    break;
                }
                i++;
            }
            return blean;
        }
    }
}