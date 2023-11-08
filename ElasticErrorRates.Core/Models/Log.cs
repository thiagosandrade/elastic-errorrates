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
            get => _httpUrl;
            set
            {
                _httpUrl = null;

                if (value != null)
                {
                    var queryStringIndexOf = value.IndexOf("?", StringComparison.InvariantCulture);

                    if (queryStringIndexOf > 1)
                    {
                        var lengthToRemove = value.Length - queryStringIndexOf;

                        _httpUrl = value.Remove(queryStringIndexOf, lengthToRemove);
                    }
                    else
                    {
                        _httpUrl = value;
                    }
                }
            }
        }

        public DateTime DateTimeLogged { get; set; }
        public string DateTimeLoggedAsString => $"{Convert.ToDateTime(DateTimeLogged):yyyy/MM/dd HH:mm:ss}";

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
