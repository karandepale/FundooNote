using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepoLayer.Context;
using RepoLayer.Interfaces;
using RepoLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;



namespace FundooNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //RADDIS CONFIGURATION :-
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });


            // IMPLEMENT CLOUDINARY:-
            //cloudinary
            IConfigurationSection configurationSection = Configuration.GetSection("CloudinaryConnection");
            Account cloudinaryAccount = new Account(
                configurationSection["cloud_name"],
                configurationSection["cloud_api_key"],
                configurationSection["cloud_api_secret"]
                );
            Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);


            services.AddControllers();
            services.AddDbContext<FundooContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:FundooDB"]));

            //DI FOR SCOPED SERVICE:-(Scoped service is created per client HttpRequest)
            services.AddScoped<IScopedUserIdService, ScopedUserIdService>();



            // USERS TABLE CONFIGURATION:-
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IUserBusiness, UserBusiness>();

            // NOTES TABLE CONFIGURATION:-
            services.AddTransient<INotesRepo, NotesRepo>();
            services.AddTransient<INotesBusiness, NotesBusiness>();

            // Upload image on Cloudinary:-
            services.AddTransient<FileService, FileService>();


            //  COLLAB TABLE CONFIGURATION:-
            services.AddTransient<ICollabRepo, CollabRepo>();
            services.AddTransient<ICollabBusiness, CollabBusiness>();

            //  LABELS TABLE CONFIGURATION :-
            services.AddTransient<ILabelRepo, LabelsRepo>();
            services.AddTransient<ILabelBusiness, LabelBusiness>();

            // SWAGGER IMPLEMENTATION:-
            //swagger:-
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fundoo App",
                    Version = "v1",
                    Description = "API's for Fundoo Application",
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                        securitySchema, new[] { "Bearer" } }
                });
            });



            // JWT:-
            // Configure JWT authentication
            var jwtSettings = Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //SWAGGGER IMPLEMENTATION:-
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Register v1");
            });



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}