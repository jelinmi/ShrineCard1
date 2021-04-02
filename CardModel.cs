
using System;
using System.Collections.Generic;
using System.Text;

namespace CardFez
{
    public class Content
    {
        public string shrineCard { get; set; }
    }

    public class Root
    {
        public Content content { get; set; }
        public int statusCode { get; set; }
        public string statusDetail { get; set; }
    }
}
