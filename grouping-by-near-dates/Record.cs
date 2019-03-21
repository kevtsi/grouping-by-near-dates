using System;
using System.Collections.Generic;

namespace grouping_by_near_dates
{
    public class Record
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Batch
    {
        public string Category { get; set; }
        public List<Record> Records { get; set; }
    }

}
