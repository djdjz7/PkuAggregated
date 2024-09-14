namespace PkuAggregated.Models
{
    public class NoticeResponse
    {
        public bool success { get; set; }
        public int results { get; set; }
        public Row[] rows { get; set; }
    }

    public class Row
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public string Department { get; set; }
        public string Number { get; set; }
        public string DocUrl { get; set; }
    }

}
