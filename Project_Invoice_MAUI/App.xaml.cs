using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Project_Invoice_MAUI.DTOContex;
using Project_Invoice_MAUI.Services;

namespace Project_Invoice_MAUI;

public partial class App : Application
{

    //static CompanyDataService database;

    //// Create the database connection as a singleton.
    //public static CompanyDataService Database
    //{
    //    get
    //    {
    //        if (database == null)
    //        {
    //            database = new CompanyDataService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CompanyData.db3"));
    //        }
    //        return database;
    //    }
    //}

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        //var db = new CompanyDataDbContex();

        //db.Database.Migrate();
    }

    protected override void OnStart()
    {
        //DatabaseFacade facade = new DatabaseFacade(new CompanyDataDbContex(CompanyDataDbContex.DB_PATH));
        //facade.EnsureCreated();

        //DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(CompanyDataDbContex.DB_PATH).Options;

        //using (CompanyDataDbContex dBContex = new CompanyDataDbContex(options))
        //{
        //    dBContex.Database.Migrate();
        //}
        base.OnStart();
    }
}
