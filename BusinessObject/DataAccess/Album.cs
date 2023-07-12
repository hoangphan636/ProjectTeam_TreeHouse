using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Album
    {
        public Album()
        {
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public int? FamilyId { get; set; }
        public string AlbumName { get; set; }
        public string UrlAlbum { get; set; }
        public string Description { get; set; }

        public virtual Family Family { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
