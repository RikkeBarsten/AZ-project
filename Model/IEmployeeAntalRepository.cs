using System;
using System.Collections.Generic;

namespace AZ_project.Model
{
    public interface IEmployeeAntalRepository
    {
        List<EmployeeAntalDTO> GetAll();
    }
}

