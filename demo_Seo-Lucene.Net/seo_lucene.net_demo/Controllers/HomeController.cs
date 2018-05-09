using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Newtonsoft.Json;
using seo_lucene.net_demo.Models;
using seo_lucene.net_demo.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace seo_lucene.net_demo.Controllers
{
    public class HomeController : Controller
    {
        //索引地址
        string indexPath = @"D:\学习代码\Seo-Lucene.Net\seo_lucene.net_demo\bin\LuceneIndex";
        private ArticleContext db = new ArticleContext();

        /// <summary>
        /// 索引目录
        /// </summary>
        public Lucene.Net.Store.Directory directory
        {
            get
            { 
                //创建索引目录
                if (!System.IO.Directory.Exists(indexPath))
                {
                    System.IO.Directory.CreateDirectory(indexPath);
                }
                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
                return directory;
            }

        }

        public ActionResult Index()
        {
           // CreateIndex();
            //当操作的表存在时，则不进行创建如果不存在，则创建
           var a= db.Database.CreateIfNotExists();
            return View();
        }
        /// <summary>
        /// 获取客户列表 模糊查询
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public string GetKeyWordsList(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return null;

            var list = new List<string> {"自动加载1", "自动加载2", "自动加载3" };

            return JsonConvert.SerializeObject(list.ToArray());
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="txtSearch">搜索字符串</param>
        /// <param name="id">当前页</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(string txtSearch,int id=1)
        {
            int pageNum = 1;
            int currentPageNo = id;
            IndexSearcher search = new IndexSearcher(directory, true);
            BooleanQuery bQuery = new BooleanQuery();

            //总的结果条数
            List<Article> list = new List<Article>();
            int recCount = 0;
            //处理搜索关键词
            txtSearch = LuceneHelper.GetKeyWordsSplitBySpace(txtSearch);

            //多个字段查询 标题和内容title, content
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,new string[] { "title", "Content" }, new PanGuAnalyzer());
            Query query =parser.Parse(txtSearch);

            //Occur.Should 表示 Or , Occur.MUST 表示 and
            bQuery.Add(query, Occur.MUST);

            if (bQuery != null && bQuery.GetClauses().Length > 0)
            {
                //盛放查询结果的容器
                TopScoreDocCollector collector = TopScoreDocCollector.Create(1000, true);
                //使用query这个查询条件进行搜索，搜索结果放入collector
                search.Search(bQuery, null, collector);        
                recCount = collector.TotalHits;
                //从查询结果中取出第m条到第n条的数据
                ScoreDoc[] docs = collector.TopDocs((currentPageNo - 1) * pageNum, pageNum).ScoreDocs;
                //遍历查询结果
                for (int i = 0; i < docs.Length; i++)
                {
                    //只有 Field.Store.YES的字段才能用Get查出来
                    Document doc = search.Doc(docs[i].Doc);
                    list.Add(new Article() {
                        Id = doc.Get("id"),
                        Title = LuceneHelper.CreateHightLight(txtSearch, doc.Get("title")),//高亮显示
                        Content = LuceneHelper.CreateHightLight(txtSearch, doc.Get("Content"))//高亮显示
                    });
                }
            }
            //分页
            PagedList<Article> plist = new PagedList<Article>(list, currentPageNo, pageNum, recCount);
            plist.TotalItemCount = recCount;
            plist.CurrentPageIndex = currentPageNo;
            return View("Index", plist);
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        private void CreateIndex()
        {
            bool isExists = IndexReader.IndexExists(directory);//判断索引库是否存在  
            if (isExists)
            {
                //如果因异常情况索引目录被锁定，先解锁  
                //Lucene.Net每次操作索引库之前会自动加锁，在close的时候会自动解锁  
                //不能多线程执行，只能处理意外被永远锁定的情况  
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);//解锁  
                }
            }

            //IndexWriter第三个参数:true指重新创建索引,false指从当前索引追加,第一次新建索引库true，之后直接追加就可以了
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExists, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);

            //Field.Store.YES：存储原文并且索引
            //Field.Index. ANALYZED：分词存储
            //Field.Index.NOT_ANALYZED：不分词存储
            //一条Document相当于一条记录
            //所有自定义的字段都是string
            try
            {
                //以下语句可通过id判断是否存在重复索引，存在则删除，如果不存在则删除0条
                //writer.DeleteDocuments(new Term("id", "1"));//防止存在的数据
                //writer.DeleteDocuments(new Term("id", "2"));//防止存在的数据
                //writer.DeleteDocuments(new Term("id", "3"));//防止存在的数据

                //或是删除所有索引
                writer.DeleteAll();
                writer.Commit();
                //是否删除成功
                var IsSuccess = writer.HasDeletions();

                Document doc = new Document();
                doc.Add(new Field("id", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title", "三国演义", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("Content", "刘备、云长、翼德点精兵三千，往北海郡进发。", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(doc);

                doc = new Document();
                doc.Add(new Field("id", "2", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title", "西游记", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("Content", "话表齐天大圣到底是个妖猴，唐三藏。", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(doc);

                doc = new Document();
                doc.Add(new Field("id", "22", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title", "西游记2", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("Content", "话表齐天大圣到底是个妖猴，唐三藏。", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(doc);

                doc = new Document();
                doc.Add(new Field("id", "23", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title", "西游记3", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("Content", "话表齐天大圣到底是个妖猴，唐三藏。", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(doc);

                doc = new Document();
                doc.Add(new Field("id", "3", Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("title", "水浒传", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                doc.Add(new Field("Content", "梁山泊义士尊晁盖 郓城县月夜走刘唐。", Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(doc);
             
            }
            catch (FileNotFoundException fnfe)
            {
                throw fnfe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Optimize();
                writer.Dispose();
                directory.Dispose();
            }

        }
    }
}
