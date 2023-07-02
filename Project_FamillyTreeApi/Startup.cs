using BusinessObject.DataAccess;
using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Project_FamillyTreeApi.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_FamillyTreeApi
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:36131")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            
            services.AddDbContext<PRN231FamilyTreeContext>();
            services.AddAutoMapper(typeof(MapperProfile));
         
            services.AddScoped<LoginDAO>();
            services.AddTransient<ILoginRepository, LoginDAO>();
            
            services.AddScoped<AccountRepository>();
            services.AddScoped<AlbumRepository>();
            services.AddScoped<StudyPromotionRepository>();
            services.AddScoped<RelationshipRepository>();
            services.AddScoped<RelativeRepository>();
            services.AddScoped<ActivitiesRepository>();
            services.AddScoped<FamilyRepository>();
            services.AddScoped<FamilyMemberRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTRefreshTokens", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "This site uses Bearer token and you have to pass" +
                    "it as Bearer<<space>>Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Scheme="oauth2",
                        Name="Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                    }
                });
            });
            var modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntitySet<Account>("Accounts");
            modelBuilder.EntitySet<Activity>("Activities");
            modelBuilder.EntitySet<Album>("Albums");
            modelBuilder.EntitySet<Family>("Families");
            modelBuilder.EntitySet<FamilyMember>("FamilyMembers");
            modelBuilder.EntitySet<Relationship>("Relationships");
            modelBuilder.EntitySet<Relative>("Relatives");
            modelBuilder.EntitySet<StudyPromotion>("StudyPromotions");
           

            //var participatingProjects = modelBuilder.EntitySet<ParticipatingProject>("ParticipatingProjects");

            //participatingProjects.EntityType.HasKey(pp => new { pp.EmployeeID, pp.CompanyProjectID });

            //modelBuilder.EntityType<Employee>().HasMany(e => e.ParticipatingProjects);
            //modelBuilder.EntityType<CompanyProject>().HasMany(cp => cp.ParticipatingProjects);

            services.AddControllers().AddOData(options =>
            {
                options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
                    "odata",
                    modelBuilder.GetEdmModel());


            });




        }

    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project_FamillyTreeApi v1"));
            }
            app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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
