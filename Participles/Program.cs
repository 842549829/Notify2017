using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Participles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 初始化索引器
            IndexLibrary indexLibrary = new IndexLibrary();
            var result = indexLibrary.GenerationIndex();

            SearchEngine searchEngine = new SearchEngine();
            var ids = searchEngine.SearchHotel("速8");
        }
    }
}