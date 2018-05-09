using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using LN = Lucene.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Search;
using PanGu;
using Lucene.Net.QueryParsers;
using PanGu.HighLight;
using System.Diagnostics;

namespace lucuneTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //定义盘古分词的xml引用路径
            PanGu.Segment.Init(PanGuXmlPath);
            //创建索引
            createIndex();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        void createIndex()
        {
          
            //IndexWriter第三个参数:true指重新创建索引,false指从当前索引追加....此处为新建索引所以为true,后续应该建立的索引应采用追加
            IndexWriter writer = new IndexWriter(direcotry, PanGuAnalyzer, true, IndexWriter.MaxFieldLength.LIMITED);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 1; i < 101; i++)
            {
                AddIndex(writer, "我的标题" + i, i + "这是我的标题啦" + i, DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
                AddIndex(writer, "射雕英雄传作者金庸" + i, i + "我是欧阳锋" + i, DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
                AddIndex(writer, "天龙八部12" + i, i + "慕容废墟,上官静儿,打撒飞艾丝凡爱上,虚竹" + i, DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
                AddIndex(writer, "倚天屠龙记12" + i, i + "张无忌机" + i, DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
                AddIndex(writer, "三国演义" + i, i + "刘备,张飞,关羽还有谁来着 忘记啦" + i, DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
            }
            //释放资源
            writer.Optimize();
            writer.Dispose();
            string time = ((double)sw.ElapsedMilliseconds / 1000).ToString();
            sw.Stop();
            Console.WriteLine("创建100条记录需要时长：" + time + "秒");
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        private void AddIndex(IndexWriter writer, string title, string content, string date)
        {
            try
            {
                Document doc = new Document();
                doc.Add(new Field("title", title, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("content", content, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("addtime", date, Field.Store.YES, Field.Index.NOT_ANALYZED));//不分词存储
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
        }

        /// <summary>
        /// 分词测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCutWords_Click(object sender, EventArgs e)
        {
            this.txtWords.Text = "";
            Lucene.Net.Analysis.Standard.StandardAnalyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            txtWords.Text = cutWords(this.textBox2.Text, PanGuAnalyzer);//盘古分词
            txtWords2.Text = cutWords(this.textBox2.Text, analyzer);    //自带标准分词
        }

        /// <summary>
        /// 分词方法
        /// </summary>
        /// <param name="words">待分词内容</param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        private string cutWords(string words, Analyzer analyzer)
        {
            string resultStr = "";
            System.IO.StringReader reader = new System.IO.StringReader(words);
            Lucene.Net.Analysis.TokenStream ts = analyzer.TokenStream(words, reader);
            bool hasNext = ts.IncrementToken();
            Lucene.Net.Analysis.Tokenattributes.ITermAttribute ita;
            while (hasNext)
            {
                ita = ts.GetAttribute<Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();
                resultStr += ita.Term + "|";
                hasNext = ts.IncrementToken();
            }
            ts.CloneAttributes();
            reader.Close();
            analyzer.Close();
            return resultStr;
        }


        protected IList<Article> list = new List<Article>();




        /// <summary>
        /// 查询多个字段
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="pageNum">页长</param>
        /// <param name="currentPageNo">当前页</param>
        private void SearchIndex(string searchKey,int pageNum=10,int currentPageNo=1)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            BooleanQuery bQuery = new BooleanQuery();

            #region 一个字段查询 
            //if (!string.IsNullOrEmpty(title))
            //{
            //    title = GetKeyWordsSplitBySpace(title);
            //    QueryParser parse = new QueryParser(LN.Util.Version.LUCENE_30, "title", PanGuAnalyzer);//一个字段查询  
            //    Query query = parse.Parse(title);
            //    parse.DefaultOperator = QueryParser.Operator.OR;
            //    bQuery.Add(query, new Occur());
            //    dic.Add("title", title);
            //}

            #endregion

            string[] fileds = { "title", "content" };//查询字段  
            searchKey = GetKeyWordsSplitBySpace(searchKey);
            QueryParser parse = new MultiFieldQueryParser(LN.Util.Version.LUCENE_30, fileds, PanGuAnalyzer);//多个字段查询
            Query query = parse.Parse(searchKey);
            bQuery.Add(query, new Occur());

            dic.Add("title", searchKey);
            dic.Add("content", searchKey);

            if (bQuery != null && bQuery.GetClauses().Length > 0)
            {
                GetSearchResult(bQuery, dic);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="bQuery"></param>
        private void GetSearchResult(BooleanQuery bQuery, Dictionary<string, string> dicKeywords)
        {
            IndexSearcher search = new IndexSearcher(direcotry, true);
            // Stopwatch stopwatch = Stopwatch.StartNew();
            //SortField构造函数第三个字段true为降序,false为升序
            Sort sort = new Sort(new SortField("addtime", SortField.DOC, true));
            int maxNum = 100;//查询条数
            TopDocs docs = search.Search(bQuery, (Filter)null, maxNum, sort);

            if (docs != null)
            {

                for (int i = 0; i < docs.TotalHits && i < maxNum; i++)
                {
                    Document doc = search.Doc(docs.ScoreDocs[i].Doc);
                    Article model = new Article()
                    {
                        Title = doc.Get("title").ToString(),
                        Content = doc.Get("content").ToString(),
                        AddTime = doc.Get("addtime").ToString()
                    };
                    list.Add(SetHighlighter(dicKeywords, model));

                }
            }

        }

        /// <summary>
        /// 索引存放目录
        /// </summary>
        protected string IndexDic
        {
            get
            {
                return Application.StartupPath + "/IndexDic";
            }
        }

        public LN.Store.Directory direcotry
        {
            get
            { //创建索引目录
                if (!System.IO.Directory.Exists(IndexDic))
                {
                    System.IO.Directory.CreateDirectory(IndexDic);
                }

                LN.Store.Directory direcotry = FSDirectory.Open(IndexDic);
                return direcotry;
            }

        }
        /// <summary>
        /// 盘古分词的配置文件
        /// </summary>
        protected string PanGuXmlPath
        {
            get
            {
                return Application.StartupPath + "/PanGu/PanGu.xml";
            }
        }

        /// <summary>
        /// 盘古分词器
        /// </summary>
        protected Analyzer PanGuAnalyzer
        {
            get { return new PanGuAnalyzer(); }

        }

        /// <summary>
        /// 处理关键字为索引格式
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        private string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }

        /// <summary>
        /// 设置关键字高亮
        /// </summary>
        /// <param name="dicKeywords">关键字列表</param>
        /// <param name="model">返回的数据模型</param>
        /// <returns></returns>
        private Article SetHighlighter(Dictionary<string, string> dicKeywords, Article model)
        {
            SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"green\">", "</font>");
            Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = 50;
            string strTitle = string.Empty;
            string strContent = string.Empty;
            dicKeywords.TryGetValue("title", out strTitle);
            dicKeywords.TryGetValue("content", out strContent);
            if (!string.IsNullOrEmpty(strTitle))
            {
                var transStr = highlighter.GetBestFragment(strTitle, model.Title);
                model.Title = string.IsNullOrEmpty(transStr) ? model.Title : transStr;
            }
            if (!string.IsNullOrEmpty(strContent))
            {
                var transStr = highlighter.GetBestFragment(strContent, model.Content);
                model.Content = string.IsNullOrEmpty(transStr) ? model.Content : transStr;
            }
            return model;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            list.Clear();
            this.txtResult.Text = "";
            SearchIndex(this.textBox2.Text);
            if (list.Count == 0)
            {
                this.txtResult.Text = "没有查询到结果";
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                this.txtResult.Text += "标题：" + list[i].Title + " 内容：" + list[i].Content + " 时间：" + list[i].AddTime + "\r\n";
            }
        }









        #region 删除索引数据（根据id）  
        /// <summary>  
        /// 删除索引数据（根据id）  
        /// </summary>  
        /// <param name="id"></param>  
        /// <returns></returns>  
        public bool Delete(string id)
        {
            bool IsSuccess = false;
            Term term = new Term("id", id);
            //Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);  
            //Version version = new Version();  
            //MultiFieldQueryParser parser = new MultiFieldQueryParser(version, new string[] { "name", "job" }, analyzer);//多个字段查询  
            //Query query = parser.Parse("小王");  

            //IndexReader reader = IndexReader.Open(directory_luce, false);  
            //reader.DeleteDocuments(term);  
            //Response.Write("删除记录结果： " + reader.HasDeletions + "<br/>");  
            //reader.Dispose();  

            IndexWriter writer = new IndexWriter(direcotry, PanGuAnalyzer, false, IndexWriter.MaxFieldLength.LIMITED);
            writer.DeleteDocuments(term); // writer.DeleteDocuments(term)或者writer.DeleteDocuments(query);  
            ////writer.DeleteAll();  
            writer.Commit();
            //writer.Optimize();//  
            IsSuccess = writer.HasDeletions();
            writer.Dispose();
            return IsSuccess;
        }
        #endregion

        #region 删除全部索引数据  
        /// <summary>  
        /// 删除全部索引数据  
        /// </summary>  
        /// <returns></returns>  
        public bool DeleteAll()
        {
            bool IsSuccess = true;
            try
            {
            
                IndexWriter writer = new IndexWriter(direcotry, PanGuAnalyzer, false, IndexWriter.MaxFieldLength.LIMITED);
                writer.DeleteAll();
                writer.Commit();
                //writer.Optimize();//  
                IsSuccess = writer.HasDeletions();
                writer.Dispose();
            }
            catch
            {
                IsSuccess = false;
            }
            return IsSuccess;
        }
        #endregion

    }



    public class Article
    {
        public string Id
        {
            set;
            get;
        }

        public string Title
        {
            set;
            get;
        }

        public string Content
        {
            set;
            get;
        }

        public string AddTime
        {
            set;
            get;
        }
    }
}
