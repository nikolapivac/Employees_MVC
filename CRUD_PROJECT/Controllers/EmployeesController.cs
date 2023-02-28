using CRUD_PROJECT.Data;
using CRUD_PROJECT.Models;
using CRUD_PROJECT.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_PROJECT.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MvcDemoDbContext mvcDemoDbContext;

        // dependency injection (accessing the database)
        public EmployeesController(MvcDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        // Display all employees
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        // Display the "Add Employees" page
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Add a new employee
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            // converting from addEmployeeRequest to an instance of Employee 
            // so we can save them in the database
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            // save the new employee and redirect the user to the Index page
            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Display (view) a certain employee
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            // return the employee with this id from the database
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            // if an employee was found, convert it into a View Model so it can be displayed
            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        // Edit a certain employee
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            // find the employee in the database
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            // if an employee was found, update its properties, save changes to the database
            // and redirect the user to the index page
            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // delete a certain employee
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            // find the employee in the database
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            // if an employee is found, remove them from the database, save changes
            // and redirect the user to the Index page
            if(employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
