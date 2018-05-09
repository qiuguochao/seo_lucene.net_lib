using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace seo_1_分词.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cut(string str)
        {
            //一元分词-简单分词
            StringBuilder sb = new StringBuilder();

            StandardAnalyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(str));

            ITermAttribute item = tokenStream.GetAttribute<ITermAttribute>();

            while (tokenStream.IncrementToken())

            {
                sb.Append(item.Term + "|");

            }
            tokenStream.CloneAttributes();
            analyzer.Close();
            return Content(sb.ToString());
        }
        [HttpPost]
        public ActionResult Cut_2(string str)
        {
            //盘古分词
            StringBuilder sb = new StringBuilder();

            Analyzer analyzer = new PanGuAnalyzer();

            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(str));

            ITermAttribute item = tokenStream.GetAttribute<ITermAttribute>();

            while (tokenStream.IncrementToken())

            {
                sb.Append(item.Term + "|");

            }
            tokenStream.CloneAttributes();
            analyzer.Close();
            return Content(sb.ToString());
        }
        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
