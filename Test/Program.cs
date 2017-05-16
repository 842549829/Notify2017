using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Notify.Code.Cache;
using Notify.Code.Code;
using Notify.Code.Encrypt;
using Notify.Code.Extension;
using Notify.Code.Features;
using Notify.Mail;
using Notify.Model.Transfer;
using Notify.Service;
using Notify.Code.Lambda;

namespace Test
{

    class Program
    {
        static void Main(string[] args)
        {
            /** 分词测试 */
            /*
            var data = Participles.TestData;
            var participles = new Participles(data);
            var rel = participles.OrderByDifferenceDesc().ToList();
            */

            /** Des加密解密 */
            //var mi = Des.Encrypt("aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");
            //var je = Des.Decrypt(mi);

            /** SHA1加密 */
            //var mi = SHA1.Encrypt("aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");

            /** MD5加密 */
            //var mi16 = Md5.Encrypt16("aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");
            //var mi32 = Md5.Encrypt32("aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");

            /** AES加密解密 */
            //var mi = AES.Encrypt("aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");
            //var je = AES.Decrypt(mi);

            /** RAS加密解密 */
            //var key = RSA.CreateRSAKey();
            //var mi = RSA.Encrypt(key.PublicKey, "aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");
            //var je = RSA.Decrypt(key.PrivateKey, mi);

            /** RAS加密解密*/
            //var key = RSACustom.CreateRSAKey();
            //var mi = RSACustom.Encrypt(key.PublicKey, "aaaaaaaaaaaaaaaaaaaaaaaaxxxxxxxxxxxxxxxxxxxxwwwwwwwwwww");
            //var je = RSACustom.Decrypt(key.PrivateKey, mi); 

            /** StringToUniCode**/
            //var str = "中国打开";
            //var strval = StringExtension.StringToUniCode(str);

            /****/
            //var strunicode = @"\u4e2d\u56fd";
            //var strunicodeval = StringExtension.UnicodeToString(strunicode); 

            /*解析*/
            UserId uid = new UserId();
            uid.Where(userId => (userId.Id == "8" && userId.LoginCount > 5) || userId.Pws != null || userId.Id.Like("%aa") && userId.LoginCount.In(new int?[] { 4, 6, 8, 9 }) && userId.Id.NotIn(new string[] { "a", "b", "c", "d" }));
            var sql = uid.WhereStr;
            Console.ReadLine();
        }
    }
}
