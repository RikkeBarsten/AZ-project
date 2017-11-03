using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AZ_project.Model;

namespace AZ_project.Pages
{
    public class AntalModel : PageModel
    {
        private IEmployeeAntalRepository _employeeAntalRepo;
        public List<EmployeeAntal> EmployeeAntal { get; set; }

        public AntalModel(IEmployeeAntalRepository userRepo)
        {
            _employeeAntalRepo = userRepo;
        }

        public string Message { get; set; }
        
        
        public void OnGet()
        {
            EmployeeAntal = _employeeAntalRepo.GetAll();
            Message = "Test";
        }
    }
}
