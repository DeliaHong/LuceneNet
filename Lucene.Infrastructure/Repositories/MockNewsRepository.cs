using Lucene.Logic.Dto;
using Lucene.Logic.Interfaces;
using Lucene.Logic.query;

namespace Lucene.Infrastructure.Repositories
{
    public class MockNewsRepository : INewsRepository
    {
        //private List<NewsDto> _news = new List<NewsDto>()
        //{
        //    new NewsDto()
        //    {
        //        Id = 1,
        //        Title = "安心、營養、美味！有機寶寶粥的最佳選擇",
        //        Description = "職業婦女家庭事業兩頭燒，為了有更充裕的時間陪伴孩子，許多媽媽選擇購買市面上已經烹煮好的寶寶粥，在家加熱給寶寶吃！然而，市面上販售嬰兒副食品的廠商好多家，您選擇的副食品真的夠安心、值得信賴嗎？"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 2,
        //        Title = "嬰兒副食品原物料很重要！寶寶粥用什麼米？",
        //        Description = "食安問題再傳，媽咪有關心過熬煮寶寶粥的米是什麼米嗎？挑選安心的好米，是讓寶寶健康茁壯的第一步。"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 3,
        //        Title = "無毒農粥寶寶 大寶寶粥彩虹組合：寶寶粥組合讓十個月雙寶副食品免煩惱！",
        //        Description = "雙寶的副食品我算是給得很晚，我私心希望他們的奶喝得很好後再來碰食物  直到雙寶六個月又一週，我才開始給十倍粥  意外的～雙寶竟吃得很好，所以我也在第二週直接跳到五倍粥  雙寶的副食品全部是由媽咪我準備，畢竟自己煮的最健康安全了！！  一直到雙寶目前十個月大了，我還是堅持自己準備所有的食材"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 4,
        //        Title = "寶寶副食品推薦，彩虹組合大寶寶粥，滿足1歲前的營養需求！",
        //        Description = "由於我廚藝不佳，沒辦法變出色香味俱全的營養寶寶粥，成品總是暗黑料理。從丁寶4個月開始吃嬰兒副食品，我就開始挑選各大品牌寶寶粥，對媽媽來說很方便，丁寶也覺得好吃，能吃的更多。"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 5,
        //        Title = "夢幻育兒好物！最多網美媽咪推薦，粉紅泡泡寶寶粥",
        //        Description = "想找一個方便、營養、安心又可愛的副食品品牌嗎？絕對是無毒農粥寶寶，粥寶寶嚴選當季無毒的蔬果熬煮寶寶粥，特別製作的粉紅泡泡寶寶粥，讓寶寶充滿好奇心，刺激食慾，也讓媽咪餵得輕鬆又安心！"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 6,
        //        Title = "【無毒農小廚房】海鮮五味醬",
        //        Description = "小卷、九孔，各式各樣的海鮮就是要吃原味，這時候再搭配上酸鹹清爽的，又有點辣的海鮮五味醬，彷彿體驗到熱帶島嶼國家的熱情啊。"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 7,
        //        Title = "∥ 必學海鮮去腥的五種超強方法 ∥",
        //        Description = "【必學海鮮去腥的五種超強方法】  海鮮永遠是許多人內心的最愛，但海鮮的腥味也是大家內心深處最不敢面對的恐懼，導致很多人都不大敢自己在家料理海鮮，擔心自己處理不當，不單單會浪費海鮮的好品質好滋味，也會有濃烈的海鮮腥味。  　  想不想做出像餐廳一樣烹調得宜、令人回味吮指的海鮮大餐呢？  熱愛海鮮的您一定不能"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 8,
        //        Title = "【澎湖白沙鮮蚵郭家賜福】用澎湖鮮蚵撐起一整家",
        //        Description = "擁有三十年養蚵歷史的郭家，在澎湖已是知名鮮蚵品牌，對產品的堅持把關及六級產業的多元發展，讓郭家鮮蚵聲名遠播，許多慕名者跨海指定朝聖，只為了一嚐在澎湖白沙內海中，淨海養殖的澎湖鮮蚵。"
        //    },
        //    new NewsDto()
        //    {
        //        Id = 9,
        //        Title = "【傳說中的澎湖米粉湯】農粉私房食譜",
        //        Description = "來自農粉的私房食譜，澎湖米粉湯也可在家輕鬆做！農粉Doris Lee前幾天與我們分享他幾年前學會的食譜，誘人的成品照讓我們也口水直流，以下就來跟大家介紹她的食譜吧"
        //    },
        //};

        private List<NewsDto> _news = new List<NewsDto>()
        {
            new NewsDto()
            {
                Id = 1,
                Title = "安心、營養、美味！有機爸爸粥的最佳選擇",
                Description = "許多媽媽選擇購買市面上已經烹煮好的爸爸粥，在家加熱給爸爸吃！"
            },
            new NewsDto()
            {
                Id = 2,
                Title = "【傳說中的澎湖米粉湯】農粉私房食譜",
                Description = "來自農粉的私房食譜，澎湖米粉湯也可在家輕鬆做！"
            },
        };

        public async Task<List<NewsDto>> GetAll()
        {
            return _news;
        }

        public async Task<List<NewsDto>> Get(GetNewsQuery query)
        {
            return _news.Where(w => w.Title.Contains(query.Title) || w.Content.Contains(query.Content) || w.Description.Contains(query.Description))
                .ToList();
        }

        public NewsDto Get(int id)
        {
            return _news.FirstOrDefault(f => f.Id == id);
        }
    }
}
