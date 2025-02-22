﻿using API_Tuyen_Dung_CV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Tuyen_Dung_CV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _storageFolder = @"D:\Study\WorkSpace\Job_Recruitment\src\assets\img\company_logo";
        public CompanyController(IConfiguration configuration)
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
            string query = "SELECT * FROM Company";
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
        public async Task<IActionResult> Post([FromForm] Company cpn)
        {
            var filePath = "./assets/img/company_logo/default-company-logo.png";
            // Kiểm tra tệp tải lên
            if (cpn.file != null || cpn.file.Length > 0)
            {
                // Tạo tên tệp duy nhất để tránh trùng lặp
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(cpn.file.FileName);

                // Đường dẫn lưu tệp
                var fileSave = Path.Combine(_storageFolder, uniqueFileName);
                filePath = "./assets/img/company_logo/" + uniqueFileName;

                // Lưu tệp vào thư mục lưu trữ
                using (var stream = new FileStream(fileSave, FileMode.Create))
                {
                    await cpn.file.CopyToAsync(stream);
                }
            }

            string query = @"INSERT INTO Company(accountID, company_name, link, address, extent, logo)
                            VALUES(@accountID, @company_name, @link, @address, @extent, @logo)";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@accountID", cpn.accountID);
                    myCommand.Parameters.AddWithValue("@company_name", cpn.company_name);
                    myCommand.Parameters.AddWithValue("@link", cpn.link);
                    myCommand.Parameters.AddWithValue("@address", cpn.address);
                    myCommand.Parameters.AddWithValue("@extent", cpn.extent);
                    myCommand.Parameters.AddWithValue("@logo", filePath);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Create Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Company cpn)
        {
            string query = @"UPDATE Company
                            SET accountID = @accountID,
                                company_name = @company_name,
                                link = @link,
                                address = @address,
                                extent = @extent,
                                logo = @logo,
                            WHERE ID = @id";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", cpn.ID);
                    myCommand.Parameters.AddWithValue("@accountID", cpn.accountID);
                    myCommand.Parameters.AddWithValue("@company_name", cpn.company_name);
                    myCommand.Parameters.AddWithValue("@link", cpn.link);
                    myCommand.Parameters.AddWithValue("@address", cpn.address);
                    myCommand.Parameters.AddWithValue("@extent", cpn.extent);
                    myCommand.Parameters.AddWithValue("@logo", cpn.logo);
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
            string query = @"DELETE From Company 
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

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = "SELECT * FROM Company WHERE accountID = @accID";
            DataTable table = new DataTable();
            String sqlDataSource = _configuration.GetConnectionString("CV");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@accID", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
