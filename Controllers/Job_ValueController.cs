using API_Tuyen_Dung_CV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Tuyen_Dung_CV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job_ValueController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public Job_ValueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = "SELECT * FROM Job_Value";
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
        public JsonResult Post(Job_Value jv)
        {
            string query = @"INSERT INTO Job_Value(min_salary, max_salary, min_exp, max_exp)
                            VALUES(@min_salary, @max_salary, @min_exp, @max_exp)";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@min_salary", jv.min_salary);
                    myCommand.Parameters.AddWithValue("@max_salary", jv.max_salary);
                    myCommand.Parameters.AddWithValue("@min_exp", jv.min_exp);
                    myCommand.Parameters.AddWithValue("@max_exp", jv.max_exp);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Create Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Job_Value jv)
        {
            string query = @"UPDATE Job_Value
                            SET min_salary = @min_salary,
                                max_salary = @max_salary,
                                min_exp = @min_exp,
                                max_exp = @max_exp,
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", jv.ID);
                    myCommand.Parameters.AddWithValue("@min_salary", jv.min_salary);
                    myCommand.Parameters.AddWithValue("@max_salary", jv.max_salary);
                    myCommand.Parameters.AddWithValue("@min_exp", jv.min_exp);
                    myCommand.Parameters.AddWithValue("@max_exp", jv.max_exp);
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
            string query = @"DELETE From Job_Value 
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
