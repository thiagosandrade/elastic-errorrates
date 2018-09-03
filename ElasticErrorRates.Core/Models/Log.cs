using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ElasticErrorRates.Core.Enums;
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

        private string _httpUrl;

        [Text(Fielddata = true)]
        public string HttpUrl
        {
            get
            {
                if (_httpUrl == null) return _httpUrl;

                var queryStringIndexOf = _httpUrl.IndexOf("?", StringComparison.InvariantCulture);

                if (queryStringIndexOf <= -1) return _httpUrl;

                var lengthToRemove = _httpUrl.Length - queryStringIndexOf;

                _httpUrl = _httpUrl.Remove(queryStringIndexOf, lengthToRemove);

                return _httpUrl;

            }
            set => _httpUrl = value;
        }

        public DateTime DateTimeLogged { get; set; }

        public Country CountryId
        {
            get
            {
                if (HttpUrl == null) return Country.NONE;

                var isUkUrl = HttpUrl.Contains(".uk");

                return isUkUrl ? Country.UK : Country.ROI;
            }
        }

        [NotMapped]
        public IEnumerable<string> Highlight { get; set; }
    }
}
