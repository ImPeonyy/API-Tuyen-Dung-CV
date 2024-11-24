
using Microsoft.VisualBasic;

namespace API_Tuyen_Dung_CV.Models
{
    public class Job
    {
        public int ID { get; set; }
        public string title { get; set; }
        public int company { get; set; }
        public int location { get; set; }
        public string address { get; set; }
        public string job_des { get; set; }
        public string job_req { get; set; }
        public DateTime date_expired { get; set;}
        public string welfare { get; set;}
        public string job_title { get; set; }
        public int job_type { get; set;}
        public int state { get; set; }
    }
}
