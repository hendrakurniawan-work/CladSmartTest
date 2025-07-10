using CladSmartTest.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CladSmartTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Employee()
        {
            ViewBag.Message = "Your contact page. Test";

            // Ensure cladsmartEntities implements IDisposable  
            var db = new cladsmartEntities();

            // Define a list to hold employee data
            List<employee> employeeList = new List<employee>();


            // Retrieve all employees and their departments  
            var employees = db.employees
                    .Select(e => new
                    {
                        e.nik,
                        e.name,
                        e.position,
                        e.bpjs_allowance,
                        e.meal_allowance,
                        e.laptop_allowance,
                        DepartmentName = e.department.department_name
                    })
                    .ToList();

            // Convert anonymous types to a list of dictionaries for easier access in the view
           
           foreach (var employee in employees)
            {
                employee emp = new employee();
                emp.nik = employee.nik;
                emp.name = employee.name;
                emp.position = employee.position;
                emp.bpjs_allowance = employee.bpjs_allowance;  
                emp.meal_allowance = employee.meal_allowance;
                emp.laptop_allowance = employee.laptop_allowance;
                emp.department = new department
                {
                    department_name = employee.DepartmentName
                };

                employeeList.Add(emp); 
            }

            // Pass the employee data to the view  
            ViewBag.Employees = employeeList;

            return View();
        }
    }
}