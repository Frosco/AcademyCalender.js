using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AcademyCalendarWebservice.Models.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AcademyCalendarWebservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace AcademyCalendarWebservice
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = @"Data Source=localhost;Initial Catalog=Frank;Integrated Security=True";

            // Enable Cross origin requests, because the AJAX POST calls on the front end require this.
            // The policies are used as decorators on Controllers or Actions.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowHeaders",
                    builder => builder.WithOrigins("http://localhost:3263")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                        );
                options.AddPolicy("AllOrigins",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithMethods("GET")
                        );
            });

            services.AddDbContext<CalendarContext>(o =>
                o.UseSqlServer(connString));

            // Add framework services.
            services.AddMvc();


            Mapper.Initialize(config =>
            {
                config.CreateMap<Room, RoomVM>();
                config.CreateMap<Booking, BookingVM>();
                config.CreateMap<BookingCreate, Booking>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Allow all origins for cross origin calls
            app.UseMvc();
        }
    }
}
