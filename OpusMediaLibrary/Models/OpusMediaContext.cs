using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OpusMediaLibrary.Models
{
    public class OpusMediaContext : DbContext
    {
        public OpusMediaContext() : base("DbConnJetAccess")
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<CloudToken> CloudTokens { get; set; }
    }
}