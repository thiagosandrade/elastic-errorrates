using System;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    [Table("Log")]
    public class Log
    {
        [Keyword]
        public int Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Exception { get; set; }
        [Text(Fielddata = true, SearchAnalyzer = "not_analyzed")]
        public string HttpUrl { get; set; }
        public DateTime DateTimeLogged { get; set; }

        [NotMapped]
        public string Highlight { get; set; }
    }
}
