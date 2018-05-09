/******************
*修改时间：2018/5/5 23:10:57
*修改人:qiuguochao
******************/
using Lucene.Net.Analysis.PanGu;
using PanGu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace seo_lucene.net_demo.Tools
{
    public class LuceneHelper
    {
        /// <summary>
        /// 处理关键字为索引格式
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static string GetKeyWordsSplitBySpace(string keywords)
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
        //高亮显示
        public static string CreateHightLight(string keywords, string Content)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
             new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"red\">", "</font>");
            //创建Highlighter ，输入HTMLFormatter 和盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 150;
            //获取最匹配的摘要段
            var result = highlighter.GetBestFragment(keywords, Content);
            return string.IsNullOrEmpty(result) ? Content : result;
        }
    }
}