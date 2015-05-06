//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Workshop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Article
    {
        public Article()
        {
            this.Photos = new HashSet<Photo>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid CategoryID { get; set; }
        public string Subject { get; set; }
        public string Summary { get; set; }
        public string ContentText { get; set; }
        public bool IsPublish { get; set; }
        public System.DateTime PublishDate { get; set; }
        public int ViewCount { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
