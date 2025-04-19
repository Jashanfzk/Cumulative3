using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Cumulative1.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
namespace Cumulative1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        // Dependency injection of database context
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns a list of all teachers in the system.
        /// </summary>
        /// <example>
        /// GET api/Teacher/TeacherNames -> [{"TeacherId":2, "TeacherFName":"Caitlin", "TeacherLName":"Cummings", "TeacherEmployeeID":"IdT381", "HireDate":"10-06-2014 00:00:00", "Salary":62.77}, ...]
        /// </example>
        /// <returns>
        /// A list of Teacher objects containing details of all teachers.
        /// </returns>
        [HttpGet]
        [Route(template: "TeacherNames/{SearchKey}")]
        public List<Teacher> TeacherNameList(string SearchKey)
        {
            // Create an empty list to store teacher details
            List<Teacher> teacherDetails = new List<Teacher>();
            // Connection is being assigned to the connection string
            MySqlConnection Connection = _context.AccessDataBase();

            Connection.Open();
            string SQLquery = "SELECT * FROM `teachers` WHERE teacherfname LIKE '%"+SearchKey+"%'";

            // Create a SQL command to retrieve all teachers
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = SQLquery;

            // Execute the query and store the result set
            MySqlDataReader ResultSet = Command.ExecuteReader();
            // Loop through each row in the result set
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string TeacherEmployeeID = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                double Salary = Convert.ToDouble(ResultSet["salary"]);



                
                Teacher newteacher = new Teacher();

                newteacher.TeacherFName = TeacherFName;
                newteacher.TeacherLName = TeacherLName;
                newteacher.TeacherEmployeeID = TeacherEmployeeID;
                newteacher.HireDate = HireDate;
                newteacher.Salary = Salary;
                newteacher.TeacherId = Convert.ToInt32(ResultSet["TeacherId"]);
                // Add the teacher object to the list
                teacherDetails.Add(newteacher);
            }
            Connection.Close();
            return teacherDetails;     // Return the final list of teacher details

        }
        /// <summary>
        /// Retrieves a specific teacher's details by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <returns>
        /// A Teacher object containing details all the teachers.
        /// </returns>
        /// <example>
        /// GET api/Teacher/FindTeacherById/1 -> {"TeacherId":3, "TeacherFName":"Linda", "TeacherLName":"Chan", "TeacherEmployeeID":"EMP001", "HireDate":"22-08-2015 00:00:00", "Salary":60.22}
        /// In this we can get a list of teachers on a sam page and by 
        /// </example>
        [HttpGet]
        [Route(template: "FindTeacherById/{id}")]
        public Teacher FindTeacherById(int id)
        {

            // Connection is being assigned to the connection string
            MySqlConnection Connection = _context.AccessDataBase();

            Connection.Open();

            string SQLquery = "SELECT * FROM teachers WHERE teacherid =" + id.ToString();
            // Create SQL query to find teacher by ID
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = SQLquery;

            MySqlDataReader ResultSet = Command.ExecuteReader();

            Teacher newteacher = new Teacher();
            // Loop through the result set
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFName = ResultSet["teacherfname"].ToString();
                string TeacherLName = ResultSet["teacherlname"].ToString();
                string TeacherEmployeeID = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                double Salary = Convert.ToDouble(ResultSet["salary"]);



                newteacher.TeacherFName = TeacherFName;
                newteacher.TeacherLName = TeacherLName;
                newteacher.TeacherEmployeeID = TeacherEmployeeID;
                newteacher.HireDate = HireDate;
                newteacher.Salary = Salary;
                newteacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
            }
            Connection.Close();
            // Return the teacher object with the retrieved details
            return newteacher;
        }
        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="teacher">The teacher object to add.</param>
        /// /// Request Body:
        /// {
        ///     "TeacherFirstName": "Ikki",
        ///     "TeacherLastName": "Gaddar",
        ///     "EmployeeID": "T98765",
        ///     "HireDate": "2024-08-15",
        ///     "Salary": 82000
        /// }
        /// <returns> Inserted Teacher Id from database is added .</returns>
        [HttpPost]
        [Route(template: "/AddTeacher")]
        public string AddATeacher([FromBody]Teacher teacher)
        {
            // Get a new database connection
            using (MySqlConnection connection = _context.AccessDataBase())
            {
                connection.Open();
                // Prepare the SQL command to insert a new teacher
                MySqlCommand Command = connection.CreateCommand();

                Command.CommandText = "INSERT into teachers(teacherfname, teacherlname, employeenumber, hiredate, salary )" +
                    "VALUES (@Fname, @Lname, @eid, @HD, @Sal) ";
                // Add parameters to prevent SQL injection
                Command.Parameters.AddWithValue("@Fname", teacher.TeacherFName);
                Command.Parameters.AddWithValue("@Lname", teacher.TeacherLName);
                Command.Parameters.AddWithValue("@eid", teacher.TeacherEmployeeID);
                Command.Parameters.AddWithValue("@HD", teacher.HireDate);
                Command.Parameters.AddWithValue("@Sal", teacher.Salary);

                Command.Prepare();

                Command.ExecuteNonQuery();
            }
            // Prepare and execute the insert command
            return "Add a teacher";
        }
        /// <summary>
        /// Deletes a teacher from the database based on their ID.
        /// </summary>
        /// <param name="ID">The ID of the teacher to delete.</param>
        /// <returns>A confirmation message of deletion and the given name is deleted.</returns>

        [HttpDelete]
        [Route(template:"/Delete")]
        public string DeleteATeacher(int ID)
        {
            using (MySqlConnection connection= _context.AccessDataBase())
            {
                // Get the database connection
                connection.Open();

                // Create a command to delete a teacher by ID
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE from teachers WHERE teacherid = @ID";
                command.Parameters.AddWithValue("@ID", ID);
                // Prepare and execute the delete command
                command.Prepare();
                command.ExecuteNonQuery();
            }
            // Return a confirmation message
            return "The Teacher Has Been Deleted";
           
        }
        /// <summary>
        /// Updates an existing teacher's details using their ID.
        /// </summary>
        /// <param name="TeacherId">ID of the teacher to update.</param>
        /// <param name="teacher">The new details of the teacher.</param>
        ///  Example:
        /// PUT: api/Teacher/UpdateTeacher/5
      
        /// {
        ///   "TeacherFName": "UpdatedName",
        ///   "TeacherLName": "UpdatedLName",
        ///   "TeacherEmployeeID": "EMP9999",
        ///   "HireDate": "2022-01-01",
        ///   "Salary": 75000
        /// }
        [HttpPut(template:"UpdateTeacher/{TeacherId}")]
        public void UpadateTeacher(int TeacherId, Teacher teacher)
        {
            using (MySqlConnection connection = _context.AccessDataBase())
            {
                connection.Open();
                // Create a command to update a teacher by ID
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE teachers SET teacherfname = @Fname, teacherlname = @Lname, employeenumber = @eid, hiredate = @HD, salary = @Sal WHERE teacherid = @ID";
                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@Fname", teacher.TeacherFName);
                command.Parameters.AddWithValue("@Lname", teacher.TeacherLName);
                command.Parameters.AddWithValue("@eid", teacher.TeacherEmployeeID);
                command.Parameters.AddWithValue("@HD", teacher.HireDate);
                command.Parameters.AddWithValue("@Sal", teacher.Salary);
                command.Parameters.AddWithValue("@ID", TeacherId);
                command.Prepare();
                command.ExecuteNonQuery();
            }
           
        }


    }
}

