using Data.Models;
using Repo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class CityService:ICityService
    {
        private IRepository<City> cityRepo;

        public CityService(IRepository<City> cityRepo)
        {
            this.cityRepo = cityRepo;
        }

        public async System.Threading.Tasks.Task<City> GetCity(long id)
        {
            try
            {
                return await cityRepo.Get(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
