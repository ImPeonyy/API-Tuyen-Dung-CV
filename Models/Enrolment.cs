namespace API_Tuyen_Dung_CV.Models
{
    public class Enrolment
    {
        public int ID { get; set; }
        public int job { get; set; }
        public int account { get; set; }
        public string cv { get; set; }
        public int rank { get; set; }
        public int state { get; set; }

        public IFormFile file { get; set; }

    }
}
