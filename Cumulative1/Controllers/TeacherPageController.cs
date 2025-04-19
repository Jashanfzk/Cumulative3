using Microsoft.AspNetCore.Mvc;
using Cumulative1.Models;
namespace Cumulative1.Controllers
{
    public class TeacherPageController : Controller
    {
        //This is a private variable that holds the connection of data base
        private readonly TeacherAPIController _api;
        //This is a constructor that initializes the connection of data base
        public TeacherPageController(TeacherAPIController api)
        {
            //when this method is used we want to see the list of teachers 
            _api = api;
        }
        /// <summary>
        /// Retrieves a list of all teachers.
        /// </summary>
        /// <returns>A view displaying the list of teachers.</returns>
        /// <example>
        /// GET /TeacherPage/List
        /// </example>
        /// 
      
        public IActionResult List(string SearchKey)
        {
            List<Teacher> teacherList = _api.TeacherNameList(SearchKey);
            return View(teacherList);
        }
        /// <summary>
        /// Retrieves details of a specific teacher based on their ID.
        /// </summary>
        /// <param name="Id">The unique identifier of the teacher.</param>
        /// <returns>A view displaying the teacher's details.</returns>
        /// <example>
        /// GET /TeacherPage/Show/1
        /// </example>
        public IActionResult Show(int Id)
        {
            Teacher teacherlist1 = _api.FindTeacherById(Id);
            return View(teacherlist1);
        }
        [HttpGet]
        [Route(template: "/TeacherPage/AddTeacherPage")]

        public IActionResult AddTeacherPage()
        {
            return View();
        }
        /// <summary>
        /// Processes the form submission to add a new teacher.
        /// </summary>
        /// <param name="firstName">The first name of the teacher.</param>
        /// <param name="lastName">The last name of the teacher.</param>
        /// <param name="employeeId">The unique employee ID for the teacher.</param>
        /// <param name="hiredDate">The date when the teacher was hired.</param>
        /// <param name="salary">The salary of the teacher as a string (to be converted).</param>
        /// <returns>Redirects to the List page after adding the teacher.</returns>
        /// <example>
        /// POST /TeacherPage/AddTeacherData
        /// </example>
        [HttpPost]
        [Route(template: "/TeacherPage/AddTeacherdata")]

        public IActionResult AddTeacherData([FromForm] string firstName, [FromForm] string lastName, [FromForm] string employeeId, [FromForm] DateTime hiredDate, [FromForm] string salary)
        {
            Teacher teacher = new Teacher();

            teacher.TeacherFName = firstName;
            teacher.TeacherLName = lastName;
            teacher.TeacherEmployeeID = employeeId;
            teacher.HireDate = hiredDate;
            double ConvertedSalary = Convert.ToDouble(salary);
            teacher.Salary = ConvertedSalary;

            _api.AddATeacher(teacher);



            return RedirectToAction("List");
        }

        /// <summary>
        /// Displays a confirmation page before deleting a teacher.
        /// </summary>
        /// <param name="ID">The ID of the teacher to be deleted.</param>
        /// <returns>A view asking the user to confirm deletion.</returns>
        /// <example>
        /// GET /TeacherPage/DeleteConfirmPage/5
        /// </example>It redirects the teacher ID to be deleted

        [HttpGet]
        [Route(template: "/TeacherPage/DeleteConfirmPage/{ID}")]

        public IActionResult DeleteConfirmPage(int ID)

        {
            Teacher teacher = _api.FindTeacherById(ID);
        return View(teacher);
        }

        [HttpGet]
        [Route(template: "/TeacherPage/DeleteConfirmed/{ID}")]

        public IActionResult DeleteConfirmed(int ID)

        {
            _api.DeleteATeacher(ID);

            return RedirectToAction("List");
        }
        /// <summary>
        /// Shows the edit page for the selected teacher.
        /// </summary>
        /// <param name="ID">Teacher's ID to edit.</param>
        /// <returns>Form view with existing data pre-filled.</returns>
        

        [HttpGet]
        [Route(template: "/TeacherPage/Edit/{ID}")]
        public IActionResult Edit(int ID)
        {
            Teacher teacher = _api.FindTeacherById(ID);
            return View(teacher);
        }
        /// <summary>
        /// Updates the teacher's information based on form input.
        /// </summary>
        /// <param name="ID">Teacher ID to update.</param>
        /// <returns>Redirects to the Show view of updated teacher.</returns>
        /// 
        ///  Example:
        /// Method: POST  
        /// URL: /TeacherPage/UpdatePage/4  
        /// Form Data:
        /// firstName=UpdatedName  
        /// lastName=UpdatedLastName 
        /// employeeId=EMP456  
        /// hiredDate=2022-05-01  
        /// salary=60000
        [HttpPost]
        [Route("/TeacherPage/UpdatePage/{ID}")]
        public IActionResult Update(int ID, [FromForm]string firstName, string lastName, string employeeId, DateTime hiredDate, string salary)
        {

                 Teacher UpdatedTeacher = new Teacher();
                 UpdatedTeacher.TeacherFName = firstName;
                 UpdatedTeacher.TeacherLName = lastName;
                 UpdatedTeacher.TeacherEmployeeID = employeeId;
                 UpdatedTeacher.HireDate = hiredDate;
                 UpdatedTeacher.Salary = Convert.ToDouble(salary);

            _api.UpadateTeacher(ID, UpdatedTeacher);

                    return RedirectToAction("Show", new { Id = ID });
              
        }
    }
}
