using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace Participles
{
    /// <summary>
    /// IndexLibrary
    /// </summary>	
    public class IndexLibrary
    {
        /// <summary>
        /// 索引地址
        /// </summary>
        private static readonly string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

        /// <summary>
        /// 盘古对象
        /// </summary>
        private readonly Analyzer analyzer = new PanGuAnalyzer();

        /// <summary>
        /// 索引总数
        /// </summary>
        private int docCount;

        /// <summary>
        /// 生成索引文档
        /// </summary>
        /// <returns>索引文档大小</returns>
        public int GenerationIndex()
        {
            using (IndexWriter iw = new IndexWriter(FSDirectory.Open(indexPath), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // 最大缓存长度
                iw.SetRAMBufferSizeMB(256);
                var baseList = GetHotelBaseData();
                foreach (var item in baseList)
                {
                    Document doc = GetDocument(item);
                    if (doc != null)
                    {
                        iw.AddDocument(doc, analyzer);
                    }
                }

                //获取索引数量
                this.docCount = iw.MaxDoc();

                //资源释放
                iw.UseCompoundFile = true;
                iw.Commit();
                iw.Optimize();
                iw.Dispose();
            }
            return this.docCount;
        }

        /// <summary>
        /// 修改索引文档
        /// </summary>
        /// <param name="hotel">信息</param>
        /// <returns>索引文档大小</returns>
        public int UpdateIndex(Hotel hotel)
        {
            FSDirectory fsd = FSDirectory.Open(indexPath);
            if (IndexWriter.IsLocked(fsd))
            {
                IndexWriter.Unlock(fsd);
            }
            using (IndexWriter writer = new IndexWriter(FSDirectory.Open(indexPath), analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                Term term = new Term("Id", hotel.Id);
                Document doc = GetDocument(hotel);
                if (doc != null)
                {
                    writer.UpdateDocument(term, doc);
                }
                else
                {
                    writer.DeleteDocuments(term);
                }
                this.docCount = writer.MaxDoc();
                writer.Commit();
                writer.Optimize();
                writer.Dispose();
            }

            return this.docCount;
        }

        /// <summary>
        /// 索引文档查询
        /// </summary>
        /// <param name="luceneSql">查询盘古SQL</param>
        /// <returns>结果</returns>
        public List<string> IndexDocQuery(string luceneSql)
        {
            List<string> hotelIds = new List<string>();
            if (!string.IsNullOrWhiteSpace(luceneSql))
            {
                IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(indexPath));
                string[] fields = { "Name", "City", "Themes"};
                MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
                Query query = parser.Parse(luceneSql);
                TopDocs tds = searcher.Search(query, int.MaxValue);
                hotelIds.AddRange(tds.ScoreDocs.Select(sd => searcher.Doc(sd.Doc)).Select(doc => doc.Get("Id")));
            }
            return hotelIds;
        }

        /// <summary>
        /// 生成Document节点
        /// </summary>
        /// <param name="hotel">酒店</param>
        /// <returns>文档</returns>
        public Document GetDocument(Hotel hotel)
        {
            //初始化Document对象
            Document doc = new Document();
            try
            {
                if (hotel == null)
                {
                    return null;
                }
                doc.Add(new Field("Id", hotel.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Name", hotel.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("City", hotel.City, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Themes", hotel.Themes, Field.Store.YES, Field.Index.ANALYZED));
            }
            catch (Exception)
            {
                return null;
            }
            return doc;
        }

        /// <summary>
        /// 获取酒店基础数据
        /// </summary>
        public static List<Hotel> GetHotelBaseData()
        {
            return Hotel.Default;
        }
    }
}