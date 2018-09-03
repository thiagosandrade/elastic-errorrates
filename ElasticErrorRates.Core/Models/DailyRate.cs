using System;
using System.ComponentModel.DataAnnotations.Schema;
using ElasticErrorRates.Core.Enums;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    [Table("DPGErrorStats")]
    public class DailyRate
    {
        [Number]
        public int Id { get; set; }
        [Number]
        public Country CountryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ErrorCount { get; set; }
        public int OrderCount { get; set; }
        public double OrderValue { get; set; }

        [NotMapped]
        public double ErrorPercentage { get; set; }
    }
}
