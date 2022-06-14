using Project_Invoice_MAUI.DTOContex;
using Project_Invoice_MAUI.Models;
using SQLite;

namespace Project_Invoice_MAUI.Services
{
    public class KontrahentService
    {

        // przydałoby się zrobić jedną instancje tego
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "CompanyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<GoodsDTO>();
            await db.CreateTableAsync<KontrahentsDTO>();
        }

        public static async Task<List<Kontrahent>> GetKontrahents()
        {
            await Init();

            var kontrahentDTOs = await db.Table<KontrahentsDTO>().ToListAsync();
            var kontrahent = new List<Kontrahent>();
            kontrahentDTOs.ForEach(r => kontrahent.Add(ToKontrahent(r)));
            //return (List<Goods>)goodsDTOs.Select(r => ToGoods(r));
            return kontrahent;
        }

        public static async Task UpdateKontrahent(Kontrahent kontrahent)
        {
            await Init();
            KontrahentsDTO kontrahentDTOs = ToKontrahentDTO(kontrahent);
            await db.UpdateAsync(kontrahentDTOs);
        }

        public static async Task Addkontrahent(Kontrahent kontrahent)
        {
            await Init();
            var kontrahentDTOs = ToKontrahentDTO(kontrahent);
            await db.InsertAsync(kontrahentDTOs);

        }

        public static async Task DeleteKontrahent(Kontrahent kontrahent)
        {
            await Init();
            var kontrahentDTOs = ToKontrahentDTO(kontrahent);
            await db.DeleteAsync<KontrahentsDTO>(kontrahentDTOs.ID);           
        }


        private static KontrahentsDTO ToKontrahentDTO(Kontrahent kontrahent)
        {
            return new KontrahentsDTO
            {
                Account_Number = kontrahent.Account_Number,
                BankAccount_Name = kontrahent.BankAccount_Name,
                Full_Name = kontrahent.Company.Full_Name,
                NIP = kontrahent.Company.NIP,
                REGON = kontrahent.Company.REGON,
                House_Number = kontrahent.Company.House_Number,
                Street = kontrahent.Company.Street,
                ZIP_Code = kontrahent.Company.ZIP_Code,
                Town = kontrahent.Company.Town,
            };
        }

        private static Kontrahent ToKontrahent(KontrahentsDTO dto)
        {
            var companydata = new CompanyData()
            {
                Full_Name = dto.Full_Name,
                NIP = dto.NIP,
                REGON = dto.REGON,
                House_Number = dto.House_Number,
                Street = dto.Street,
                ZIP_Code = dto.ZIP_Code,
                Town = dto.Town,
            };
            return new Kontrahent( dto.BankAccount_Name, dto.Account_Number, companydata);
            
        }

        public static async Task<Kontrahent> GetConflictingGoods(Kontrahent kontrahent)
        {
            var kontrahentDTO = await db.Table<KontrahentsDTO>()
                .Where(g => g.BankAccount_Name == kontrahent.BankAccount_Name)
                .Where(g => g.Account_Number == kontrahent.Account_Number)
                //.Where(g => g.Company_ID == kontrahent.Company.CompanyID)// idk czy to bedzie działać
                .Where(g => g.ID == kontrahent.ID)// idk czy to bedzie działać
                .FirstOrDefaultAsync();

            if (kontrahentDTO == null)
            {
                return null;
            }

            return ToKontrahent(kontrahentDTO);
        }
    }
}

