using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
    /// <summary>
    /// 分词
    /// </summary>
    public class Participles
    {
        /// <summary>
        /// 词组
        /// </summary>
        private readonly List<string> words;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="words">单词</param>
        public Participles(List<string> words)
        {
            this.words = words;
        }

        /// <summary>
        /// 分词
        /// </summary>
        /// <returns>分词结果</returns>
        public IEnumerable<Word> Split()
        {
            return this.words.Select(item => new Word(item));
        }

        /// <summary>
        /// 根据差值排序(升序)
        /// </summary>
        /// <returns>结果</returns>
        public IEnumerable<string> OrderByDifferenceAsc()
        {
            return this.Split().OrderBy(item => item.Difference).Select(item => item.OriginalWord);
        }

        /// <summary>
        /// 根据差值排序(降序)
        /// </summary>
        /// <returns>结果</returns>
        public IEnumerable<string> OrderByDifferenceDesc()
        {
            return this.Split().OrderByDescending(item => item.Difference).Select(item => item.OriginalWord);
        }

        /// <summary>
        /// 测试数据
        /// </summary>
        public static List<string> TestData => new List<string>
        {
            "满68元减30元",
            "满11元减10元",
            "满5元减3元",
            "满99元减30元",
            "满7元减5元",
            "满19元减10元",
            "满49元减40元",
            "满69元减40元"
        };
    }

    /// <summary>
    /// 单词
    /// </summary>
    public class Word
    {
        /// <summary>
        /// 单词
        /// </summary>
        private readonly string word;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="word">单词</param>
        public Word(string word)
        {
            this.word = word;
            this.RegexSplit();
        }

        /// <summary>
        /// 正则分词
        /// </summary>
        public void RegexSplit()
        {
            const string pattern = @"^满(?<max>\d+)元减(?<min>\d+)元$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(this.word);
            this.OriginalWord = this.word;
            this.Max = Convert.ToDecimal(match.Groups["max"].Value);
            this.Min = Convert.ToDecimal(match.Groups["min"].Value);
        }

        /// <summary>
        /// 原词
        /// </summary>
        public string OriginalWord { get; private set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Max { get; private set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Min { get; private set; }

        /// <summary>
        /// 差值
        /// </summary>
        public decimal Difference => this.Max - this.Min;
    }
}