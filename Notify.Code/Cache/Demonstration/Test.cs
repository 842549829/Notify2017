/*
 神兽保佑－代码无BUG
       ┏┓　　　┏┓
     ┏┛┻━━━┛┻┓
     ┃　　　　　　　┃ 　
     ┃　　　━　　　┃
     ┃　┳┛　┗┳　┃
     ┃　　　　　　　┃
     ┃　　　┻　　　┃
     ┃　　　　　　　┃
     ┗━┓　　　┏━┛
         ┃　　　┃   神兽保佑　　　　　　　　
         ┃　　　┃   代码无BUG！
         ┃　　　┗━━━┓
         ┃　　　　　　　┣┓
         ┃　　　　　　　┏┛
         ┗┓┓┏━┳┓┏┛
           ┃┫┫　 ┃┫┫
           ┗┻┛　 ┗┻┛
*/

/*
 适用范围 : 基础数据
 */

namespace Notify.Code.Cache.Demonstration
{
    public class Test
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        public void AddData()
        {
            var city = new City();
            object obj = CityCollection.Instance.Add(city);

            // obj就是数据库返回自动增长列
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void GetData()
        {
            var city = CityCollection.Instance.Values;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="code">根据城市代码获取</param>
        public void GetData(string code)
        {
            var city = CityCollection.Instance[code];
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="city">修改数据</param>
        public void UpdateDate(City city)
        {
            CityCollection.Instance.Update(city.Code, city);
        }

        /// <summary>
        /// 删除数据 
        /// </summary>
        /// <param name="code">城市代码</param>
        public void Remove(string code)
        {
            CityCollection.Instance.Remove(code);
        }
    }
}
