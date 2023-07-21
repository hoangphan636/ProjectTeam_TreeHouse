using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int? FamilyId { get; set; }
        [Required]
        public string AlbumName { get; set; }
        [Required]
        public string UrlAlbum { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Family Family { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}