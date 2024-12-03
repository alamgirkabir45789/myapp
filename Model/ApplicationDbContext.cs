using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myportfolio.Model.Entity.AboutUs;
using myportfolio.Model.Entity.Communication;
using myportfolio.Model.Entity.AboutUs;
using myportfolio.Model.Entity.Career;
using myportfolio.Model.Entity.Catalogs;
using myportfolio.Model.Entity.Communication;
using myportfolio.Model.Entity.Designes;
using myportfolio.Model.Entity.GroupRecources;
using myportfolio.Model.Entity.NewsAndEvents;
using myportfolio.Model.Entity.Resources;
using myportfolio.Model.Entity.Shared;
using myportfolio.Model.Entity.Styles;
using myportfolio.Model.Entity.Team;
using myportfolio.Model.Entity.DailyReport;
using myportfolio.Model.Entity.LeaveRegister;
using myportfolio.Pages.Admin.DailyReport;
using myportfolio.Model.Entity.DailyTask;
using myportfolio.Pages.Admin.DailyTask;
using myportfolio.Model.Entity.User;
using myportfolio.Pages.Admin.LeaveRegister;
using myportfolio.Model.Entity.ReportedProject;
using myportfolio.Pages.Admin.ReportedProject;
using myportfolio.Model.Entity.Holiday;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace myportfolio.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #region Career
        public DbSet<JobCircular> JobCirculars { get; set; }
        public DbSet<DevelopmentProgram> DevelopmentPrograms { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        #endregion 
        
        #region Team
        public DbSet<Team> Teams { get; set; }

        #endregion

        #region Communication
        public DbSet<Subscriber> Subscribers { get; set; }

        #endregion

        #region NewsAndEvents
        public DbSet<CSRProgram> CSRPrograms { get; set; }
        public DbSet<SafetyAwareness> SafetyAwareness { get; set; }
        public DbSet<DayToDayActivity> DayToDayActivities { get; set; }
        public DbSet<BGMEACircular> BGMEACirculars { get; set; }
        #endregion


        #region Resources
        public DbSet<MediaGallery> MediaGallery { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        #endregion

        #region Shared
        public DbSet<FileGroup> FileGroups { get; set; }
        #endregion

        #region Designes
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<HeadLine> HeadLines { get; set; }
        #endregion

        #region Catalog
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Campaigning> Campaignings { get; set; }
        #endregion

        #region Group Resource
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        #endregion

        #region Designes
        public DbSet<Query> Queries { get; set; }
        #endregion

        #region AboutUs
        public DbSet<Compliance> Compliances { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Mission> Missions { get; set; }
        #endregion
        
        #region Styles
        public DbSet<Style> Styles { get; set; }
        public DbSet<StyleCategory> StyleCategories { get; set; }
        #endregion


        #region Daily Report
        public DbSet<DailyReport> DailyReports { get; set; }
        #endregion

        #region Daily Task
        public DbSet<DailyTask> DailyTasks { get; set; }
        #endregion


        #region Leave Register
        public DbSet<LeaveRegister> LeaveRegisters { get; set; }
        #endregion

        #region Reported Project
        public DbSet<ReportedProject> ReportedProjects { get; set; }
        #endregion

        #region Reported Project
        public DbSet<myportfolio.Pages.Admin.ReportedProject.ReportedProjectRequested> ReportedProjectRequested { get; set; }
        #endregion

        #region Leave Type
        public DbSet<EmpLeaveType> EmpLeaveTypes { get; set; }
        #endregion


        #region Leave 
        public DbSet<EmpLeave> EmpLeaves { get; set; }
        #endregion
        #region Holiday 
        public DbSet<Holiday> Holidays { get; set; }
        #endregion


       // protected override void OnModelCreating(ModelBuilder builder)
       // {
       //     builder.Entity<LeaveRegister>()
       //.HasOne(e => e.User)
       //.WithOne(e => e.LeaveRegister)
       //.HasForeignKey<LeaveRegister>(e => e.UserId)
       //.IsRequired();

       //     builder.Entity<ApplicationUser>()
       // .HasOne(e => e.LeaveRegister)
       // .WithOne(e => e.User)
       // .HasForeignKey<LeaveRegister>(e => e.UserId)
       // .IsRequired();
       // }
    }
}
