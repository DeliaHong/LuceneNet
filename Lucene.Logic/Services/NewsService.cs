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
using System.Reflection.Metadata;
using System.Xml.Linq;
using Yamool.CWSharp;
using Document = Lucene.Net.Documents.Document;

namespace Lucene.Logic.Services
{
    public class NewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly DirectoryInfo _dir;//文件路徑

        //【基本分詞器】
        //private readonly Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_CURRENT)

        //【CWSharp一元分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new UnigramTokenizer());

        //【CWSharp二元分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new BigramTokenizer());

        //【CWSharp詞庫分詞】
        //private readonly Analyzer analyzer = new CwsAnalyzer(new Yamool.CWSharp.StandardTokenizer(new FileStream("cwsharp.dawg", FileMode.Open)));

        //【盤古分詞】
        private readonly Analyzer analyzer = new PanGuAnalyzer();

        //【MMSeg Simple】
        //private readonly Analyzer analyzer = new Net.Analysis.MMSeg.SimpleAnalyzer();

        //【MMSeg Complex】
        //private readonly Analyzer analyzer = new Lucene.Net.Analysis.MMSeg.ComplexAnalyzer();

        //【MMSeg MaxWord】
        //private readonly Analyzer analyzer = new MMSegAnalyzer();

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

            Document document = new Document();
            foreach (var item in news)
            {
                document.Add(new Field("Id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED,Field.TermVector.NO));
                document.Add(new Field("Title", item.Title ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
                document.Add(new Field("Content", item.Content ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
                document.Add(new Field("Description", item.Description ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));

                document.GetField("Title").Boost = 1.5F;
                document.GetField("Description").Boost = 0.5F;
                
                indexWriter.AddDocument(document);
            }

            indexWriter.Optimize();
            indexWriter.Commit();
            indexWriter.Dispose();
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
                    document.Add(new Field("Title", news.Title, Field.Store.NO, Field.Index.ANALYZED));
                    document.Add(new Field("Content", news.Content ?? string.Empty, Field.Store.NO, Field.Index.ANALYZED));
                    document.Add(new Field("Description", news.Description, Field.Store.NO, Field.Index.ANALYZED));

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
                    var parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(
                        Net.Util.Version.LUCENE_CURRENT
                        , new string[] { "Ttile", "Content", "Description"}, 
                        analyzer)
                        .Parse(query);

                    var hits = indexSearcher.Search(parser, queryLimit);
                    if (!hits.ScoreDocs.Any())
                    {
                        return new List<NewsDto>();
                    }

                    var result = hits.ScoreDocs.Select(s => new NewsDto
                    {
                        Id = Convert.ToInt32(indexSearcher.Doc(s.Doc).Get("Id")),
                        Title = indexSearcher.Doc(s.Doc).Get("Title"),
                        Content = indexSearcher.Doc(s.Doc).Get("Content"),
                        Description = indexSearcher.Doc(s.Doc).Get("Description")
                    }).ToList();

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
