using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface ICityService
    {
        System.Threading.Tasks.Task<City> GetCity(long id);
    }
}
