using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Support;
using PanGu;

namespace Participles
{
    /// <summary>
    /// SearchEngine
    /// </summary>	
    public class SearchEngine
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="name">关键字</param>
        /// <returns>Id集合</returns>
        public List<string> SearchHotel(string name)
        {
            List<QueryParameter> paras = new EquatableList<QueryParameter>
            {
                new QueryParameter
                {
                    ParameterName = "keyword",
                    ParameterValue = name
                }
            };
            return SearchHotel(paras);
        }

        /// <summary>
        /// 搜索酒店信息
        /// </summary>
        /// <param name="paras">搜索参数列表</param>
        /// <returns>List{System.String}.</returns>
        public List<string> SearchHotel(List<QueryParameter> paras)
        {
            string luceneSql = GetLuceneSql(paras);
            List<string> hotelIDs = new List<string>();
            IndexLibrary indexManager = new IndexLibrary();
            hotelIDs.AddRange(indexManager.IndexDocQuery(luceneSql));
            return hotelIDs;
        }

        /// <summary>
        /// 根据传入查询参数列表，生成对应的盘古分词SQL语句(酒店查询)
        /// </summary>
        /// <param name="paras">The paras.</param>
        /// <returns>System.String.</returns>
        private static string GetLuceneSql(List<QueryParameter> paras)
        {
            string luceneSql = string.Empty;
            if (paras == null || paras.Count == 0)
            {
                return luceneSql;
            }
            List<string> paraSQL = new List<string>();
            foreach (var item in paras)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.ParameterName.ToLower() != "keyword")
                {
                    paraSQL.Add($"+({item.ParameterName}:{item.ParameterValue})");
                }
                else
                {
                    List<string> words = GetSegmentKeyword(item.ParameterValue);
                    if (words != null && words.Count > 0)
                    {
                        foreach (var word in words)
                        {
                            paraSQL.Add(string.Format("+(City:{0} Name:{0} Themes:{0})", word));
                            //paraSQL.Add(string.Format("+(City:{0}* Name:{0} DistrictName:{0} CityName:{0}  HotelID:{0})", word));
                        }
                    }
                    else
                    {
                        paraSQL.Add(string.Format("+(City:{0} Name:{0} Themes:{0})", item.ParameterValue));
                    }
                }
            }
            luceneSql = string.Join("", paraSQL.ToArray());
            return luceneSql;
        }

        /// <summary>
        /// 获取分词后关键字
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <returns>结果</returns>
        public static List<string> GetSegmentKeyword(string keyWord)
        {
            List<string> keys = new List<string>();
            if (!string.IsNullOrEmpty(keyWord))
            {
                Segment segment = new Segment();
                ICollection<WordInfo> words = segment.DoSegment(keyWord);
                keys.AddRange(from item in words where item != null && item.Word.Length > 1 select item.Word);
            }
            return keys;
        }
    }

    /// <summary>
    /// QueryParameter
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// 查询参数名称（查询字段名称请于数据库字段名称保持一致,输入关键字名称请用 keyWord 标识）
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }
    }
}