namespace PkuAggregated.Models
{
    public class CourseReviewResponse
    {
        public string status { get; set; }
        public Cdata[] cDatas { get; set; }
        public Rlatest rLatest { get; set; }
        public int countReviews { get; set; }
        public int countCourses { get; set; }
        public string version { get; set; }
        public object sSessionKey { get; set; }
        public object sSessionId { get; set; }
        public object sSessionKeyAes { get; set; }
        public int safetyFlag { get; set; }
        public string homepageNews { get; set; }
    }

    public class Rlatest
    {
        public int id { get; set; }
        public string title { get; set; }
        public string teacher_name { get; set; }
        public int recommended { get; set; }
        public int rating_content { get; set; }
        public int rating_workload { get; set; }
        public int rating_exam { get; set; }
        public string result { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int vote_score { get; set; }
        public int term_id { get; set; }
        public int course_id { get; set; }
        public Course course { get; set; }
    }

    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Cdata
    {
        public int id { get; set; }
        public int credits { get; set; }
        public string department { get; set; }
        public string elect_type { get; set; }
        public string elect_type_new { get; set; }
        public string name { get; set; }
        public string name_pinyin { get; set; }
        public int review_count { get; set; }
        public string system { get; set; }
    }
}
