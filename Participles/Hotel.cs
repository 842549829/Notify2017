using System.Collections.Generic;

namespace Participles
{
    /// <summary>
    /// Hotel
    /// </summary>	
    public class Hotel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Themes { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 默认数据
        /// </summary>
        public static List<Hotel> Default => new List<Hotel>
        {
            new Hotel
            {
                Id = "001",
                City = "成都",
                Name = "如家",
                Themes = "青年旅社"
            },
            new Hotel
            {
                Id = "002",
                City = "成都",
                Name = "如家",
                Themes = "聚会做饭"
            },
            new Hotel
            {
                Id = "003",
                City = "北京",
                Name = "7天",
                Themes = "度假休闲"
            },
            new Hotel
            {
                Id = "004",
                City = "北京",
                Name = "速8",
                Themes = "四合院"
            },
            new Hotel
            {
                Id = "005",
                City = "北京",
                Name = "速8",
                Themes = "美味"
            }
        };
    }
}