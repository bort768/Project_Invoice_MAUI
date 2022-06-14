using Microsoft.EntityFrameworkCore;


namespace Project_Invoice_MAUI.DbContex
{
    public class CompanyDataDbContex : DbContext
    {
        //public const string DB_PATH = "CompanyData.db";

        //private string _dbPath;
        //public CompanyDataDbContex(DbContextOptions options) : base(options)
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite($"Data Source = {DB_PATH}");
        //}


        //public DbSet<Models.CompanyDataDTO> companyDatas { get; set; }
        //public DbSet<Goods> goods { get; set; }
        //public DbSet<Kontrahent> kontrahents { get; set; }

        //private const string DatabaseName = "myItems.db";

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //String databasePath;
        //    //switch (Device.RuntimePlatform)
        //    //{
        //    //    case Device.iOS:
        //    //        databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
        //    //        break;
        //    //    case Device.Android:
        //    //        databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
        //    //        break;
        //    //    default:
        //    //        throw new NotImplementedException("Platform not supported");
        //    //}
        //    //optionsBuilder.UseSqlite($"Filename={databasePath}");

        //}
    }
}
