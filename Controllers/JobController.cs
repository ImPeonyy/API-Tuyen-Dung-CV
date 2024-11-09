using API_Tuyen_Dung_CV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Tuyen_Dung_CV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public JobController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT * FROM Job";
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
        public JsonResult Post(Job j)
        {
            string query = @"INSERT INTO Job(title, company, location, address, job_des, job_req, date_expired, welfare, job_title, job_type)
                            VALUES(@title, @company, @location, @address, @job_des, @job_req, @date_expired, @welfare, @job_title, @job_type)";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@title", j.title);
                    myCommand.Parameters.AddWithValue("@company", j.company);
                    myCommand.Parameters.AddWithValue("@location", j.location);
                    myCommand.Parameters.AddWithValue("@address", j.address);
                    myCommand.Parameters.AddWithValue("@job_des", j.job_des);
                    myCommand.Parameters.AddWithValue("@job_req", j.job_req);
                    myCommand.Parameters.AddWithValue("@date_expired", j.date_expired);
                    myCommand.Parameters.AddWithValue("@welfare", j.welfare);
                    myCommand.Parameters.AddWithValue("@job_title", j.job_title);
                    myCommand.Parameters.AddWithValue("@job_type", j.job_type);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Create Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Job j)
        {
            string query = @"UPDATE Job
                            SET title = @title,
                                company = @company,
                                location = @location,
                                address = @address,
                                job_des = @job_des,
                                job_req = @job_req,
                                date_expired = @date_expired,
                                welfare = @welfare,
                                job_title = @job_title,
                                job_type = @job_type
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", j.ID);
                    myCommand.Parameters.AddWithValue("@title", j.title);
                    myCommand.Parameters.AddWithValue("@company", j.company);
                    myCommand.Parameters.AddWithValue("@location", j.location);
                    myCommand.Parameters.AddWithValue("@address", j.address);
                    myCommand.Parameters.AddWithValue("@job_des", j.job_des);
                    myCommand.Parameters.AddWithValue("@job_req", j.job_req);
                    myCommand.Parameters.AddWithValue("@date_expired", j.date_expired);
                    myCommand.Parameters.AddWithValue("@welfare", j.welfare);
                    myCommand.Parameters.AddWithValue("@job_title", j.job_title);
                    myCommand.Parameters.AddWithValue("@job_type", j.job_type);
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
            string query = @"DELETE From Job
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
    }
}
