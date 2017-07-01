using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpusMediaLibrary.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string OrigFilename { get; set; }
        public string Link { get; set; }
        [ForeignKey("CloudToken")]
        public int CloudTokenId { get; set; }
        public virtual CloudToken CloudToken { get; set; }
        public DateTime DateAdded { get; set; }
    }
}