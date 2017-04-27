using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Notify.Code.IdentityCard
{
    /// <summary>
    /// 验证类
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// 验证码
        /// </summary>
        private readonly char[] verifyCodeMapping = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

        /// <summary>
        /// 地区代码
        /// </summary>
        private string areaCode;

        /// <summary>
        /// 生日
        /// </summary>
        private string birthCode;

        /// <summary>
        /// 随机
        /// </summary>
        private string randomCode;

        /// <summary>
        /// 性别
        /// </summary>
        private char sexCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class. 
        /// </summary>
        /// <param name="id">Id</param>
        public Validator(string id)
        {
            this.Id = id;
            this.Success = false;
            this.ErrorMessage = string.Empty;
            this.Area = null;
            this.Birth = BirthDate.Empty;
            this.Sex = Sex.Male;
        }

        /// <summary>
        /// 身份证ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        ///  区域信息
        /// </summary>
        public AreaInformation Area { get; private set; }

        /// <summary>
        ///  生日
        /// </summary>
        public BirthDate Birth { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; private set; }

        /// <summary>
        /// 执行比较结果
        /// </summary>
        /// <returns>结果</returns>
        public bool Execute()
        {
            string msg = this.CheckAndParse();
            if (string.IsNullOrWhiteSpace(msg))
            {
                this.ErrorMessage = string.Empty;
                this.Success = true;
            }
            else
            {
                this.ErrorMessage = msg;
                this.Success = false;
            }
            return this.Success;
        }

        /// <summary>
        /// IdentityCard18
        /// </summary>
        public string IdentityCard18
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Id))
                {
                    return "身份证号码不能为空";
                }
                if (this.Success && this.Id.Length == 15)
                {
                    return this.ToCardInfo18();
                }
                return this.Id;
            }
        }

        /// <summary>
        /// ToCardInfo18
        /// </summary>
        /// <returns>结果</returns>
        private string ToCardInfo18()
        {
            string bodyCode = GetBodyCode(this.areaCode, "19" + this.birthCode, this.randomCode, this.sexCode);
            char verifyCode = this.GetVerifyCode(bodyCode);
            return bodyCode + verifyCode;
        }

        /// <summary>
        /// 获取bodyCode
        /// </summary>
        /// <param name="areaCode">areaCode</param>
        /// <param name="birthCode">birthCode</param>
        /// <param name="randomCode">randomCode</param>
        /// <param name="sexCode">sexCode</param>
        /// <returns></returns>
        private static string GetBodyCode(string areaCode, string birthCode, string randomCode, char sexCode)
        {
            return areaCode + birthCode + randomCode + sexCode.ToString();
        }

        /// <summary>
        /// 检查身份证基本信息
        /// </summary>
        /// <returns>结果</returns>
        private string CheckAndParse()
        {
            if (string.IsNullOrWhiteSpace(this.Id))
            {
                return "身份证号码不能为空";
            }
            if (this.Id.Length == 15)
            {
                return this.ParseCardInfo15();
            }
            if (this.Id.Length == 18)
            {
                return this.ParseCardInfo18();
            }
            return "身份证号码必须为15位或18位";
        }

        /// <summary>
        /// 18位身份证
        /// </summary>
        /// <returns>结果</returns>
        private string ParseCardInfo18()
        {
            const string CardIdParttern = @"(\d{6})(\d{4})(\d{2})(\d{2})(\d{2})(\d{1})([\d,x,X]{1})";
            Match match = Regex.Match(this.Id, CardIdParttern);
            if (match.Success)
            {
                this.areaCode = match.Groups[1].Value;
                string year = match.Groups[2].Value;
                string month = match.Groups[3].Value;
                string day = match.Groups[4].Value;
                this.birthCode = year + month + day;
                this.randomCode = match.Groups[5].Value;
                this.sexCode = char.Parse(match.Groups[6].Value);
                string verifyCode = match.Groups[7].Value.ToUpper();

                if (this.ValidateVerifyCode(this.Id.Substring(0, 17), char.Parse(verifyCode)))
                {
                    try
                    {
                        this.Birth = BirthDate.GetBirthDate(year, month, day);
                        this.Area = AreaCodeMapping.GetArea(this.areaCode);
                        Sex = GetSex(this.sexCode);
                    }
                    catch (System.Exception ex)
                    {
                        return ex.Message;
                    }
                    return string.Empty;
                }
            }
            return "身份证号码格式错误";
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="bodyCode">bodyCode</param>
        /// <param name="verifyCode">verifyCode</param>
        /// <returns>结果</returns>
        private bool ValidateVerifyCode(string bodyCode, char verifyCode)
        {
            char calculatedVerifyCode = this.GetVerifyCode(bodyCode);
            return calculatedVerifyCode == verifyCode;
        }

        /// <summary>
        /// 15位身份证
        /// </summary>
        /// <returns>结果</returns>
        private string ParseCardInfo15()
        {
            const string CardIdParttern = @"(\d{6})(\d{2})(\d{2})(\d{2})(\d{2})(\d{1})";
            Match match = Regex.Match(this.Id, CardIdParttern);
            if (match.Success)
            {
                this.areaCode = match.Groups[1].Value;
                string year = match.Groups[2].Value;
                string month = match.Groups[3].Value;
                string day = match.Groups[4].Value;
                this.birthCode = year + month + day;
                this.randomCode = match.Groups[5].Value;
                this.sexCode = char.Parse(match.Groups[6].Value);

                try
                {
                    this.Area = AreaCodeMapping.GetArea(this.areaCode);
                    this.Birth = BirthDate.GetBirthDate(year, month, day);
                    Sex = GetSex(this.sexCode);
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
                return string.Empty;
            }
            return "身份证号码格式错误";
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="bodyCode">bodyCode</param>
        /// <returns>结果</returns>
        private char GetVerifyCode(string bodyCode)
        {
            char[] bodyCodeArray = bodyCode.ToCharArray();
            ////int sum = 0;
            ////for (int index = 0; index < bodyCodeArray.Length; index++)
            ////{
            ////    sum += int.Parse(bodyCodeArray[index].ToString()) * GetWeight(index);
            ////}
            ////return this.verifyCodeMapping[sum % 11];
            int sum = bodyCodeArray.Select((t, index) => int.Parse(t.ToString()) * GetWeight(index)).Sum();
            return this.verifyCodeMapping[sum % 11];
        }

        /// <summary>
        /// GetWeight
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>index</returns>
        private static int GetWeight(int index)
        {
            return (1 << (17 - index)) % 11;
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="sexCode">性别代码</param>
        /// <returns>性别</returns>
        private static Sex GetSex(char sexCode)
        {
            return ((int)sexCode) % 2 == 0 ? Sex.Female : Sex.Male;
        }
    }

    /// <summary>
    /// 生日
    /// </summary>
    public struct BirthDate
    {
        /// <summary>
        /// 年
        /// </summary>
        private readonly string year;

        /// <summary>
        /// 月
        /// </summary>
        private readonly string month;

        /// <summary>
        /// 日
        /// </summary>
        private readonly string day;

        /// <summary>
        /// 默认
        /// </summary>
        public static BirthDate Empty
        {
            get { return new BirthDate("00", "00", "00"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BirthDate"/> struct. 
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public BirthDate(string year, string month, string day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        /// <summary>
        /// 获取生日
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>结果</returns>
        public static BirthDate GetBirthDate(string year, string month, string day)
        {
            DateTime date;
            if (DateTime.TryParse(string.Format("{0}-{1}-{2}", year, month, day), out date))
            {
                return new BirthDate(year, month, day);
            }
            throw new System.Exception("日期不存在");
        }

        /// <summary>
        /// 年 
        /// </summary>
        public string Year
        {
            get { return this.year; }
        }

        /// <summary>
        /// 年
        /// </summary>
        public string Month
        {
            get { return this.month; }
        }

        /// <summary>
        /// 日
        /// </summary>
        public string Day
        {
            get { return this.day; }
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>结果</returns>
        public override string ToString()
        {
            return string.Format("{0}年{1}月{2}日", this.year, this.month, this.day);
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <param name="separator">separator</param>
        /// <returns>结果</returns>
        public string ToString(string separator)
        {
            return string.Format("{1}{0}{2}{0}{3}", separator, this.year, this.month, this.day);
        }
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男
        /// </summary>
        Male,

        /// <summary>
        /// 女
        /// </summary>
        Female
    }
}
