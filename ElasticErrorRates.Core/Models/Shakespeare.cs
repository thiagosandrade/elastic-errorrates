using System;
using System.Collections.Generic;
using System.Text;
using Nest;

namespace ElasticErrorRates.Core.Models
{
    public class Shakespeare
    {
        [Text(Name = "document_id")]
        public int DocumentId { get; set; }

        [Text(Name = "line_id")]
        public int Id { get; set; }

        [Text(Name = "play_name", Fielddata = true)]
        public string Name { get; set; }

        [Text(Name = "speech_number")]
        public string SpeechNumber { get; set; }

        [Text(Name = "line_number")]
        public string LineNumber { get; set; }

        [Text(Name = "speaker")]
        public string Speaker { get; set; }

        [Keyword(Name = "text_entry")]
        public string TextEntry { get; set; }

        public string Highlight { get; set; }
    }
}
