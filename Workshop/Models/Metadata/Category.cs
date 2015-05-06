using System;
using System.ComponentModel.DataAnnotations;

namespace Workshop.Models
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        public class CategoryMetadata
        {
            [UIHint("SystemUser")]
            public object CreateUser { get; set; }

            [UIHint("SystemUser")]
            public object UpdateUser { get; set; }
        }
    }
}