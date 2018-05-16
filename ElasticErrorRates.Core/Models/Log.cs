using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    [Table("Log")]
    public class Log
    {
        [Number]
        public int Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        //[Text(SearchAnalyzer = "trigrams")]
        public string Exception { get; set; }

        [Text(Fielddata = true)]
        public string HttpUrl { get; set; }

        public DateTime DateTimeLogged { get; set; }

        [NotMapped]
        public IEnumerable<string> Highlight { get; set; }
    }
}
