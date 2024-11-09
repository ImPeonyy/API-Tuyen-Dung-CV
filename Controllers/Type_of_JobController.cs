using API_Tuyen_Dung_CV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Tuyen_Dung_CV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Type_of_JobController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public Type_of_JobController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT * FROM Type_of_Job";
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
        public JsonResult Post(Type_of_Job toj)
        {
            string query = @"INSERT INTO Type_of_Job(type_name)
                            VALUES(@type_name)";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@type_name", toj.type_name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Create Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Type_of_Job toj)
        {
            string query = @"UPDATE Type_of_Job
                            SET type_name = @type_name,
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ID", toj.ID);
                    myCommand.Parameters.AddWithValue("@type_name", toj.type_name);
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
            string query = @"DELETE From Type_of_Job 
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
