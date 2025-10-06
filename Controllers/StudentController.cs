using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Student_RegistrationPage.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting.Server;

namespace Student_RegistrationPage.Controllers
{
    public class StudentsController : Controller
    {
        private readonly string _connectionString;

        public StudentsController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: Registration Form
        public IActionResult Register()
        {
            return View();
        }

        // POST Save Student
        [HttpPost]
        public IActionResult Register(Student student, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            try
            {
                // Handle Image Upload
                string fileName = "default.png";
                if (imageFile != null && imageFile.Length > 0)
                {
                    fileName = Path.GetFileName(imageFile.FileName);

                    string uploadfolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    bool exists = System.IO.Directory.Exists(uploadfolderPath);

                    if (!exists)
                        System.IO.Directory.CreateDirectory(uploadfolderPath);
                    string uploadFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(uploadFilePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
                student.ImagePath = fileName;

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Students (FirstName, LastName, Email, RollNumber, Gender, ImagePath,CreatedDate) VALUES (@FirstName, @LastName, @Email, @RollNumber, @Gender, @ImagePath,@CreatedDate)", con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", student.LastName);
                        cmd.Parameters.AddWithValue("@Email", student.Email);
                        cmd.Parameters.AddWithValue("@RollNumber", student.RollNumber);
                        cmd.Parameters.AddWithValue("@Gender", student.Gender);
                        cmd.Parameters.AddWithValue("@ImagePath", student.ImagePath);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                TempData["Success"] = "Student registered successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        //  Students List
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();
            if(TempData["ErrorMessage"]!= null)
            {
                string message = TempData["ErrorMessage"].ToString();
                TempData["ErrorMessage"] = null;
                ViewData["ErrorMessage"] = message;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Students", con))
                    {
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = Convert.ToInt32(rdr["StudentId"]),
                                FirstName = rdr["FirstName"].ToString(),
                                LastName = rdr["LastName"].ToString(),
                                Email = rdr["Email"].ToString(),
                                RollNumber = rdr["RollNumber"].ToString(),
                                Gender = rdr["Gender"].ToString(),
                                ImagePath = rdr["ImagePath"].ToString(),
                                CreatedDate = Convert.ToDateTime(rdr["CreatedDate"])

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(students);
        }

        //  Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Students WHERE StudentId = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await con.CloseAsync();
                    }
                }

                TempData["Success"] = "Student deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
