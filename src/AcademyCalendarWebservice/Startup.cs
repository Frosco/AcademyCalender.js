﻿using System;
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

            services.AddDbContext<CalendarContext>(o =>
                o.UseSqlServer(connString));
            // Add framework services.
            services.AddMvc();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Room, RoomVM>();
                config.CreateMap<Booking, BookingVM>();
                config.CreateMap<BookingVM, Booking>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
