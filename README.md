# .net-core-onion-architecture
Seed project with onion architecture on .net core framework

There will be 4 main layer in the project. Which are Data, Repository, Service and GUI.
In this sample code web project is created for GUI layer.

#  Data Layer

Install Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Tools, MySql.Data.EntityFrameworkCore, MySql.Data.EntityFrameworkCore.Design from NuGet Package Manager

Create BaseEntity object. Which your objects inherit from it. In this project common variables of all classes are Id, Status and CreateDate.

BaseEntity.cs


                using System;
                using System.ComponentModel.DataAnnotations.Schema;

                namespace Data.Models
                {
                    public class BaseEntity
                    {
                        public int? Id { get; set; }
                        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
                        public DateTime? CreateDate { get; set; }
                        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
                        public int? Status { get; set; }
                    }
                }
                
Some class. Let City.cs

                using System.Collections.Generic;

                namespace Data.Models
                {
                    public partial class City : BaseEntity
                    {
                        public City()
                        {
                            Town = new HashSet<Town>();
                        }

                        public int IdCountry { get; set; }
                        public string Name { get; set; }
                        public string PlateNo { get; set; }
                        public string PhoneCode { get; set; }

                        public virtual ICollection<Town> Town { get; set; }
                    }
                }
                
                
# Repository Layer

Install Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Proxies from NuGet Package Manager

Create DBContext Class and enable Lazy Loading

    public class ApplicationContext : DbContext
    {

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Town> Town { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
            .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city", "DB_NAME");
                //fill accordingly...
            
            });
            
            //...
        }
    }

Then create Repository.cs for all operations related to database. We will use following methods;

        public IQueryable<T> GetAll()
        {
            return entities.Where(s => s.Status == 1);
        }
        public async Task<T> Get(long id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id && s.Status == 1);
        }

        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> whereClause)
        {
            return entities.Where(whereClause);
        }

        public int? Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            try
            {
                SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
            return entity.Id;
        }

        public IQueryable<T> GetSPFromDatabase(string _query)
        {

            return entities.FromSql(_query);

        }

        public int Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }


            context.Entry(entity).State = EntityState.Modified;

            try
            {
                SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        public void UpdateStatus(T entity, int status)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                string _query = String.Format("call sp_update_status ({0},{1});", entity.Id, status);
                GetSPFromDatabase(_query);

            }
            catch (Exception ex)
            {
            }

        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            SaveChanges();
        }
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }
        public async void SaveChanges()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

# Service Layer

In this layer we will implement what we will need on GUI layer. Let say we need Cities, then we will create CityService and implement necessary methods to get data from database by using IRepository methods.

CityService.cs

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

# GUI Layer

We will reach data from Service layer by implementing necessary service classes. In this project we have already implemented the CityService. To reach that layer first of all we need to add connection string to appsettings and then configure the project from Startup.cs.

Add following to appsettings.json;

          "ConnectionStrings": {
            "DefaultConnection": "server=SERVER_IP;port=3306;user=DB_USER;password=DB_PASS;database=DB_NAME"
          }
Then configure application from Startup.cs

            //CONNECTION SETTINGS
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            //DI - SERVICE LAYER
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ICityService, CityService>();

Now, we are all set. Let get data from HomeController with following code in order to use it in the application (GUI);
We should implement CityService and set it in the constructor. Then we can use the data..

        private readonly ICityService cityService;
        public HomeController(ICityService cityService)
        {
            this.cityService = cityService;
        }
        public IActionResult Index()
        {
            var city = cityService.GetCity(5);
            return View(city);
        }




