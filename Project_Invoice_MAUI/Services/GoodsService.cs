
using Project_Invoice_MAUI.DTOContex;
using Project_Invoice_MAUI.Models;
using SQLite;

namespace Project_Invoice_MAUI.Services
{
   

    public class GoodsService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "CompanyDataBase.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<GoodsDTO>();
            await db.CreateTableAsync<KontrahentsDTO>();
        }

        public static async Task<List<Goods>> GetGoods()
        {
            await Init();

            var goodsDTOs = await db.Table<GoodsDTO>().ToListAsync();
            var goods = new List<Goods>();
            goodsDTOs.ForEach(r => goods.Add(ToGoods(r)));
            //return (List<Goods>)goodsDTOs.Select(r => ToGoods(r));
            return goods;
        }

        public static async Task UpdateGoods(Goods goods)
        {
            await Init();
            GoodsDTO goodsDTOs = ToGoodsDTO(goods);
            await db.UpdateAsync(goodsDTOs);
        }

        public static async Task AddGoods(Goods goods)
        {
            await Init();
            var goodsDTOs = ToGoodsDTO(goods);
            await db.InsertAsync(goodsDTOs);

        }

        public static async Task DeleteGoods(Goods goods)
        {
            await Init();
            var goodsDTOs = ToGoodsDTO(goods);
            await db.DeleteAsync<GoodsDTO>(goodsDTOs.Product_Code);
        }


        private static GoodsDTO ToGoodsDTO(Goods goods)
        {
            return new GoodsDTO
            {
                Product_Code = goods.Product_Code,
                Product_Name = goods.Product_Name,
                Price_Brutto = goods.Price_Brutto,
                Price_Netto = goods.Price_Netto,
                Product_Id = goods.Product_Id,
                Description = goods.Description,
                VAT = goods.VAT,
                VAT_String = goods.VAT_String,                
            };
        }

        private static Goods ToGoods(GoodsDTO dto)
        {
            return new Goods(dto.Product_Name, dto.Product_Code, dto.Description,
                dto.Price_Netto, dto.Price_Brutto, dto.VAT, dto.VAT_String);
        }

        public static async Task<Goods> GetConflictingGoods(Goods goods)
        {
            GoodsDTO goodsDTO = await db.Table<GoodsDTO>()
                .Where(g => g.Product_Id == goods.Product_Id)
                .Where(g => g.Price_Brutto == goods.Price_Brutto)
                .Where(g => g.Price_Netto == goods.Price_Netto)
                .Where(g => g.Product_Name == goods.Product_Name)
                .Where(g => g.Product_Code == goods.Product_Code)
                .Where(g => g.VAT == goods.VAT)
                .Where(g => g.VAT_String == goods.VAT_String)
                .FirstOrDefaultAsync();

            if (goodsDTO == null)
            {
                return null;
            }

            return ToGoods(goodsDTO);
        }
    }
}
