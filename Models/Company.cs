namespace API_Tuyen_Dung_CV.Models
{
    public class Company
    {
        public int ID { get; set; }
        public int accountID { get; set; }
        public string company_name { get; set; }
        public string link { get; set; }
        public string address { get; set; }
        public int extent { get; set; }
        public string logo { get; set; }
        public IFormFile file { get; set; }

    }
}
