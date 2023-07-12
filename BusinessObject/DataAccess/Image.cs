using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Image
    {
        public int Id { get; set; }
        public int? AlbumId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UrlImage { get; set; }

        [JsonIgnore]
        public virtual Album Album { get; set; }
    }
}
