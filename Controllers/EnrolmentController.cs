using API_Tuyen_Dung_CV.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Tuyen_Dung_CV.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAngularApp")] 
    [ApiController]
    public class EnrolmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _storageFolder = @"D:\Study\WorkSpace\Job_Recruitment\src\assets\img\cv_storage";
        public EnrolmentController(IConfiguration configuration)
        {
            _configuration = configuration;

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }

        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT * FROM Enrolment";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Enrolment erm)
        {
            // Kiểm tra tệp tải lên
            if (erm.file == null || erm.file.Length == 0)
            {
                return BadRequest("Không có tệp tải lên.");
            }

            // Kiểm tra xem account đã ứng tuyển vào job này chưa
            string checkQuery = @"SELECT COUNT(*) FROM Enrolment WHERE account = @account AND job = @job";

            bool accountAlreadyAppliedForJob = false;
            string sqlDataSource = _configuration.GetConnectionString("CV");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand myCommand = new SqlCommand(checkQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@account", erm.account);
                    myCommand.Parameters.AddWithValue("@job", erm.job);
                    var result = await myCommand.ExecuteScalarAsync();
                    accountAlreadyAppliedForJob = Convert.ToInt32(result) > 0; // Nếu có ít nhất 1 bản ghi trùng
                }
            }

            // Nếu đã có bản ghi ứng tuyển với cùng account và job, trả về lỗi
            if (accountAlreadyAppliedForJob)
            {
                return BadRequest("Tài khoản này đã ứng tuyển cho công việc này.");
            }

            // Tạo tên tệp duy nhất để tránh trùng lặp
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(erm.file.FileName);

            // Đường dẫn lưu tệp
            var filePath = @"D:/Study/WorkSpace/Job_Recruitment/src/assets/cv_storage/" + uniqueFileName;

            // Lưu tệp vào thư mục lưu trữ
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await erm.file.CopyToAsync(stream);
            }

            // Sau khi tệp đã được tải lên, ta chèn dữ liệu vào cơ sở dữ liệu
            string query = @"INSERT INTO Enrolment(job, account, cv, rank, state)
                     VALUES(@job, @account, @cv, 0, 0)";

            // Chuyển đường dẫn tệp để lưu vào cơ sở dữ liệu
            string cvFilePath = filePath;

            // Chuẩn bị dữ liệu để chèn vào câu truy vấn
            DataTable table = new DataTable();
            SqlDataReader myReader;

            // Chèn dữ liệu vào cơ sở dữ liệu
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@job", erm.job);
                    myCommand.Parameters.AddWithValue("@account", erm.account);
                    myCommand.Parameters.AddWithValue("@cv", cvFilePath); // Lưu đường dẫn tệp vào cơ sở dữ liệu
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Tạo thành công!");
        }




        [HttpPut]
        public JsonResult Put(Enrolment erm)
        {
            string query = @"UPDATE Enrolment
                            SET job = @job,
                                account = @account,
                                cv = @cv,
                                state = @state
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", erm.ID);
                    myCommand.Parameters.AddWithValue("@job", erm.job);
                    myCommand.Parameters.AddWithValue("@account", erm.account);
                    myCommand.Parameters.AddWithValue("@cv", erm.cv);
                    myCommand.Parameters.AddWithValue("@state", erm.state);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Successfully!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE From Enrolment 
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successfully!");
        }

        [Route("GetListEnrolmentByAccount/{id}")]
        [HttpGet]
        public JsonResult GetListEnrolmentByAccount(int id)
        {
            string query = "select Enrolment.ID, Enrolment.cv, Enrolment.state, Account.name as Accountname, Account.email as Accountemail, Account.phone_number as Accountphone_number," +
                           "\r\njob.title as jobtitle, Job.job_des AS JobDescription, company.company_name ,Type_of_Job.type_name\r\nfrom Enrolment join Account on Enrolment.account = Account.ID\r\n " +
                           "join Job on Enrolment.job = Job.ID JOIN Type_of_Job ON Job.job_type = Type_of_Job.ID join Company on job.company = Company.ID WHERE Enrolment.account = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("GetListEnrolmentByCompany/{id}")]
        [HttpGet]
        public JsonResult GetListEnrolmentByCompany(int id)
        {
            string query = @"SELECT *
                            FROM Enrolment e
                            JOIN Job j ON e.job = j.ID
                            JOIN Company c ON j.company = c.ID
                            JOIN Account a ON e.account = a.ID
                            WHERE c.ID = 5";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("Putstate")]
        [HttpPut]
        public JsonResult Putstate(Enrolment erm)
        {
            string query = @"UPDATE Enrolment
                    SET state = @state
                    WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", erm.ID);
                    myCommand.Parameters.AddWithValue("@state", erm.state);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Successfully!");
        }

        [Route("PutRanking")]
        [HttpPut]
        public JsonResult PutRanking(Enrolment erm)
        {
            string query = @"UPDATE Enrolment
                    SET rank = @rank
                    WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", erm.ID);
                    myCommand.Parameters.AddWithValue("@ranking", erm.rank);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Successfully!");
        }

        // GET: api/enrolment/download/{id}
        [HttpGet("download/{id}")]
        public IActionResult DownloadFile(int id)
        {
            string filePath = null;

            // Kết nối tới SQL để lấy đường dẫn file CV
            string query = "SELECT cv FROM Enrolment WHERE ID = @id";
            string sqlDataSource = _configuration.GetConnectionString("CV");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    if (myReader.Read())
                    {
                        filePath = myReader["cv"].ToString();
                    }
                    myReader.Close();
                }
            }


            // Kiểm tra nếu không tìm thấy file
            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound("File not found in the database.");
            }

            // Kiểm tra file có tồn tại trên server không
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found on the server.");
            }

            // Trả về file PDF
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);

            return File(fileBytes, "application/pdf", fileName);
        }

    }
}
