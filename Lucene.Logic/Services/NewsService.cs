using Lucene.Logic.Dto;
using Lucene.Logic.Interfaces;
using Lucene.Logic.Options;
using Lucene.Logic.query;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.CWSharp;
using Lucene.Net.Analysis.MMSeg;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.VisualBasic;
using PanGu;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Yamool.CWSharp;
using static Lucene.Net.Index.IndexWriter;
using Document = Lucene.Net.Documents.Document;

namespace Lucene.Logic.Services
{
    public class NewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly DirectoryInfo _dir;//文件路徑

        //【基本分詞器】
        //private readonly Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT);

        //【CWSharp一元分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new UnigramTokenizer());

        //【CWSharp二元分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new BigramTokenizer());

        //【CWSharp詞庫分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new Yamool.CWSharp.StandardTokenizer(new FileStream("cwsharp.dawg", FileMode.Open)));

        //【盤古分詞】
        //private readonly Analyzer analyzer = new PanGuAnalyzer();

        //【MMSeg Simple】
        //private readonly Analyzer analyzer = new Net.Analysis.MMSeg.SimpleAnalyzer();

        //【MMSeg Complex】
        //private readonly Analyzer analyzer = new Lucene.Net.Analysis.MMSeg.ComplexAnalyzer();

        //【MMSeg MaxWord】
        private readonly Analyzer analyzer = new MMSegAnalyzer();

        public NewsService(INewsRepository newsRepository, IndexOption indexOption)
        {
            _newsRepository = newsRepository;
            _dir = new DirectoryInfo(indexOption.IndexFileFolder);
        }

        public async void Create()
        {
            //取得所有文章
            var news = await _newsRepository.GetAll();

            // 取得或建立Lucene文件資料夾
            if (!File.Exists(_dir.FullName))
            {
                System.IO.Directory.CreateDirectory(_dir.FullName);
            }

            Net.Store.Directory directory = FSDirectory.Open(_dir);

            var indexWriter = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

            indexWriter.DeleteAll();

            foreach (var item in news)
            {
                Document document = new Document();
                document.Add(new Field("Id", item.Id.ToString(), 
                    Field.Store.YES, Field.Index.NOT_ANALYZED,Field.TermVector.NO));

                document.Add(new Field("Title", item.Title ?? string.Empty, 
                    Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                document.Add(new Field("Content", item.Content ?? string.Empty, 
                    Field.Store.YES, Field.Index.NO, Field.TermVector.NO));

                document.Add(new Field("Description", item.Description ?? string.Empty, 
                    Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                document.GetField("Title").Boost = 1.5F;
                document.GetField("Description").Boost = 0.5F;
                
                indexWriter.AddDocument(document);
            }

            indexWriter.Optimize();
            indexWriter.Commit();
            indexWriter.Dispose();
            //通过IndexWriter的Optimize方法优化索引，以加快搜索的速度，该方法提供多个重载，其执行相当耗时，应谨慎使用。优化产生的垃圾文件，在执行Flush / Commit / Close方法后才会被物理删除。Optimize方法及其重载包括：
            //Optimize() 合并段，完成后返回。
            //Optimize(bool doWait) 与Optimize()相同，但立即返回。
            //Optimize(int maxNumSegments) 针对最多maxNumSegments个段进行优化，而并非全部索引。
            //Optimize(int maxNumSegments, bool doWait) 与Optimize(int maxNumSegments)相同，但立即返回。
        }

        public void DeleteById(int id)
        {
            using (var directory = FSDirectory.Open(_dir))
            {
                using (var indexWriter = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED))
                {
                    indexWriter.GetDocCount(1);
                    indexWriter.DeleteDocuments(new Term("Id", id.ToString()));
                    indexWriter.Optimize();
                    indexWriter.Commit();
                }
            }
        }

        public void DeleteAll()
        {
            using (var directory = FSDirectory.Open(_dir))
            {
                using (var indexWriter = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED))
                {
                    indexWriter.DeleteAll();
                    indexWriter.Optimize();
                    indexWriter.Commit();
                }
            }
        }

        public void Update(int id)
        {
            //取得更新文章
            var news = _newsRepository.Get(id);

            using (var directory = FSDirectory.Open(_dir))
            {
                using (var indexWriter = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED))
                {
                    var document = new Document();
                    document.Add(new Field("Id", news.Id.ToString(), Field.Store.NO, Field.Index.ANALYZED));
                    document.Add(new Field("Title", news.Title, Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("Content", news.Content ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("Description", news.Description, Field.Store.YES, Field.Index.ANALYZED));

                    indexWriter.UpdateDocument(new Term("Id", id.ToString()), document);

                    indexWriter.Optimize();
                    indexWriter.Commit();
                }
            }
        }

        public List<NewsDto> SearchByIndex(string query, int queryLimit = 20)
        {
            using (var directory = FSDirectory.Open(_dir))
            {

                using (var indexSearcher = new IndexSearcher(directory))
                {
                    //單Field查詢
                    var queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_CURRENT, "Description", analyzer).Parse(query);

                    //多Field查詢
                    //var queryParser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(
                    //    Net.Util.Version.LUCENE_CURRENT
                    //    , new string[] { "Title", "Content", "Description" },
                    //    analyzer)
                    //    .Parse(query);

                    //排序
                    Sort sort = new();
                    SortField sortField = new SortField("Id", SortField.STRING, true);//true表示逆向
                    sort.SetSort(sortField);
                    //過濾
                    Filter filter = NumericRangeFilter.NewIntRange("size", 400, 450, true, true);//過濾檔案大小400~450

                    var hits = indexSearcher.Search(queryParser, filter, queryLimit, sort);
                    if (!hits.ScoreDocs.Any())
                    {
                        return new List<NewsDto>();
                    }

                    Document document ;
                    var result = new List<NewsDto>();
                    foreach (var hit in hits.ScoreDocs)
                    {
                        document = indexSearcher.Doc(hit.Doc);
                        NewsDto news = new NewsDto()
                        {
                            Id = Convert.ToInt32(document.Get("Id")),
                            Title = document.Get("Title"),
                            Content = document.Get("Content"),
                            Description = document.Get("Description")
                        };
                        result.Add(news);
                    }

                    return result;
                }
            }
        }

        public async Task<List<NewsDto>> SearchByIndexWithLogic(string[] queryArr, int queryLimit = 20)
        {
            //製造邏輯查詢
            BooleanQuery bq = new BooleanQuery();
            for (int i = 0; i < queryArr.Length; i++)
            {
                Term term = new Term("Title", queryArr[i]);
                TermQuery tq = new TermQuery(term);
                if (i == 0)
                {
                    bq.Add(tq, Occur.MUST);
                }
                else
                {
                    bq.Add(tq, Occur.MUST_NOT);
                }
                //must:and , should:or
            }

            using (var directory = FSDirectory.Open(_dir))
            {

                using (var indexSearcher = new IndexSearcher(directory))
                {

                    var hits = indexSearcher.Search(bq, queryLimit);
                    if (!hits.ScoreDocs.Any())
                    {
                        return new List<NewsDto>();
                    }

                    Document document;
                    var result = new List<NewsDto>();
                    foreach (var hit in hits.ScoreDocs)
                    {
                        document = indexSearcher.Doc(hit.Doc);
                        NewsDto news = new NewsDto()
                        {
                            Id = Convert.ToInt32(document.Get("Id")),
                            Title = document.Get("Title"),
                            Content = document.Get("Content"),
                            Description = document.Get("Description")
                        };
                        result.Add(news);
                    }

                    return result;
                }
            }
        }

        public async Task<List<NewsDto>> SearchBySql(GetNewsQuery query)
        {
            return await _newsRepository.Get(query);
        }
    }
}
