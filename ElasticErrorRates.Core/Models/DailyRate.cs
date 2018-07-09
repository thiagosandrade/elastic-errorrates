using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    [Table("DPGErrorStats")]
    public class DailyRate
    {
        [Number]
        public int Id { get; set; }
        [Number]
        public int CountryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ErrorCount { get; set; }
        public int OrderCount { get; set; }
        public double OrderValue { get; set; }

        [NotMapped]
        public double ErrorPercentage { get; set; }
    }
}
