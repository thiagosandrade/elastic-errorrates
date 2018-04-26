using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    [Table("Log")]
    public class Log
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Exception { get; set; }
        [Text(Fielddata = true)]
        public string HttpUrl { get; set; }
        public string DateTimeLogged { get; set; }

        [NotMapped]
        public string Highlight { get; set; }
    }
}
