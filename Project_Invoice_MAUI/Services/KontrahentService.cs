using Project_Invoice_MAUI.DbContexs;
using Project_Invoice_MAUI.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Invoice_MAUI.Services
{
    public class KontrahentService
    {
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
            await db.DeleteAsync<CompanyData>(kontrahentDTOs.ID);
        }


        private static KontrahentsDTO ToKontrahentDTO(Kontrahent kontrahent)
        {
            return new KontrahentsDTO
            {
                Account_Number = kontrahent.Account_Number,
                BankAccount_Name = kontrahent.BankAccount_Name,
                Company = kontrahent.Company,
            };
        }

        private static Kontrahent ToKontrahent(KontrahentsDTO dto)
        {
            return new Kontrahent( dto.BankAccount_Name, dto.Account_Number, dto.Company);
            
        }

        public static async Task<Kontrahent> GetConflictingGoods(Kontrahent kontrahent)
        {
            var kontrahentDTO = await db.Table<KontrahentsDTO>()
                .Where(g => g.BankAccount_Name == kontrahent.BankAccount_Name)
                .Where(g => g.Account_Number == kontrahent.Account_Number)
                .Where(g => g.Company == kontrahent.Company)// idk czy to bedzie działać
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

