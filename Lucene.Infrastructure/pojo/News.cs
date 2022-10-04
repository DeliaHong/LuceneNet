using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene.Infrastructure.pojo
{
    public class News
    {
        public int Id { get; set; }

        public int Fun_Id { get; set; }

        public int? CategoryId { get; set; }

        public string Cover { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Content_head { get; set; }

        public string Content { get; set; }

        public bool Blank { get; set; }

        public int Click { get; set; }

        public bool Display { get; set; }

        public int Sort { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string AttFile { get; set; }

        public string AttFileName { get; set; }

        public int UpdateUserId { get; set; }

        public bool Del { get; set; }

        public string Writer { get; set; }

        public int? CoverWidth { get; set; }

        public int? CoverHeight { get; set; }

        public string AppContent { get; set; }

        public int CreateUserId { get; set; }

        public string ProductId { get; set; }

        public bool AppDisplay { get; set; }

        public int? Week_Click { get; set; }

        public bool Top { get; set; }

        public string ProfilePic { get; set; }

        public string WebImageSizeBanner { get; set; }

        public string Description { get; set; }

    }

}
