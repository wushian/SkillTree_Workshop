using System;
using System.ComponentModel.DataAnnotations;

namespace Workshop.Models
{
    [MetadataType(typeof(SystemUserMetadata))]
    public partial class SystemUser
    {
        public class SystemUserMetadata
        {
            [DataType(DataType.Password)]
            public object Password { get; set; }

            [DataType(DataType.EmailAddress)]
            public object Email { get; set; }

            [UIHint("SystemUser")]
            public object CreateUser { get; set; }

            [UIHint("SystemUser")]
            public object UpdateUser { get; set; }
        }
    }
}