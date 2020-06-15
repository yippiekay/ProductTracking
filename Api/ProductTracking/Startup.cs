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
using System;
using System.Security.Cryptography;
using System.Text;

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
            services.AddTransient<IService<RoleDTO>, RoleService>();
            services.AddTransient<IService<TaskDTO>, TaskService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,                      // валидия издателя при валидации токена
                            ValidIssuer = AuthOptions.ISSUER,           // строка, представляющая издателя
                            ValidateAudience = true,                    // будет ли валидироваться потребитель токена
                            ValidAudience = AuthOptions.AUDIENCE,       // установка потребителя токена
                            ValidateLifetime = true,                    // будет ли валидироваться время существования
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),  // установка ключа безопасности
                            ValidateIssuerSigningKey = true,            // валидация ключа безопасности
                        };
                    });
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();
            
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }

        private static MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {

                var salt = GenerateSaltForPassword();
                cfg.CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
                cfg.CreateMap<UserDTO, User>()
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role { Name = src.Role }));
                cfg.CreateMap<RegistrationModel, UserDTO>()
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => ComputePasswordHash(src.Password, salt)))
                    .ForMember(dest => dest.Salt, opt => opt.MapFrom(src => salt));
                cfg.CreateMap<UserViewModel, UserDTO>();
                cfg.CreateMap<UserDTO, UserViewModel>();
                cfg.CreateMap<Task, TaskDTO>();
                cfg.CreateMap<TaskDTO, Task>();
                cfg.CreateMap<TaskDTO, TaskViewModel>();
                cfg.CreateMap<TaskViewModel, TaskDTO>();
                cfg.CreateMap<CreateTaskModel, TaskDTO>();
            });
            return config; 
        }

        // salt generator
        private static int GenerateSaltForPassword()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[4];
            rng.GetNonZeroBytes(saltBytes);
            return (saltBytes[0] << 24) + (saltBytes[1] << 16) + (saltBytes[2] << 8) + saltBytes[3];
        }

        // hashing
        private static byte[] ComputePasswordHash(string password, int salt)
        {
            byte[] saltBytes = new byte[4];
            saltBytes[0] = (byte)(salt >> 24);
            saltBytes[1] = (byte)(salt >> 16);
            saltBytes[2] = (byte)(salt >> 8);
            saltBytes[3] = (byte)(salt);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] preHashed = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, preHashed, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, preHashed, passwordBytes.Length, saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(preHashed);
        }
    }
}