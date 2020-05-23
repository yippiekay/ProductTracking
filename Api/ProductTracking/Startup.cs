using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductTracking.Authentication;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.BLL.Services;
using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using ProductTracking.DAL.Repository;
using ProductTracking.ViewModels;

namespace ProductTracking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ProductTrackingDbContext>(options => options.UseSqlServer(connection));
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IMapper>(m => new Mapper(CreateConfiguration()));
            services.AddTransient<IService<UserDTO>, UserService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,                      // ������� �������� ��� ��������� ������
                            ValidIssuer = AuthOptions.ISSUER,           // ������, �������������� ��������
                            ValidateAudience = true,                    // ����� �� �������������� ����������� ������
                            ValidAudience = AuthOptions.AUDIENCE,       // ��������� ����������� ������
                            ValidateLifetime = true,                    // ����� �� �������������� ����� �������������
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),  // ��������� ����� ������������
                            ValidateIssuerSigningKey = true,            // ��������� ����� ������������
                        };
                    });


            services.AddControllers();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                
                cfg.CreateMap<UserViewModel, UserDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
                
            });
            return config; 
        }
    }
}
