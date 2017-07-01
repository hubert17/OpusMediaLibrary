using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpusMediaLibrary.Models
{
    public class CloudToken
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string Token { get; set; }
        public bool Inactive { get; set; }
    }
}