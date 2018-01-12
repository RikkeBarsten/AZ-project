using System;
using System.Collections.Generic;

namespace AZ_project.Model
{
    public interface IEmployeeAntalRepository
    {
        string GetAll();

        string GetFuldtid();

        string GetFuldtid(bool csv);

        string GetAntal();

        string GetAntal(bool csv);
    }
}

