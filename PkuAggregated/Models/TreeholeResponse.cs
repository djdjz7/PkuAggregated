namespace PkuAggregated.Models
{
    public class TreeholeResponse<T>
    {
        public int code { get; set; }
        public T? data { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public int timestamp { get; set; }
    }

    public class LoginResponseData
    {
        public string jwt { get; set; }
    }

    public class SearchResponseData
    {
        public int current_page { get; set; }
        public TreeholeData[] data { get; set; }
        public string first_page_url { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string last_page_url { get; set; }
        public string next_page_url { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public string? prev_page_url { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class TreeholeData
    {
        public int pid { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public int timestamp { get; set; }
        public int reply { get; set; }
        public int likenum { get; set; }
        public int extra { get; set; }
        public int anonymous { get; set; }
        public int is_top { get; set; }
        public int label { get; set; }
        public int status { get; set; }
        public int is_comment { get; set; }
        public string tag { get; set; }
        public int is_follow { get; set; }
        public int is_protect { get; set; }
        public int[] image_size { get; set; }
        public Label_Info label_info { get; set; }
    }

    public class Label_Info
    {
        public int id { get; set; }
        public string tag_name { get; set; }
        public object created_at { get; set; }
        public object updated_at { get; set; }
    }

    public class DetailsResponseData
    {
        public int current_page { get; set; }
        public CommentData[] data { get; set; }
        public string first_page_url { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string last_page_url { get; set; }
        public string? next_page_url { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public string? prev_page_url { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class CommentData
    {
        public int cid { get; set; }
        public int pid { get; set; }
        public string text { get; set; }
        public int timestamp { get; set; }
        public object tag { get; set; }
        public object comment_id { get; set; }
        public string name { get; set; }
        public QuoteData? quote { get; set; }
    }

    public class QuoteData
    {
        public int pid { get; set; }
        public string text { get; set; }
        public string name_tag { get; set; }
    }
}
