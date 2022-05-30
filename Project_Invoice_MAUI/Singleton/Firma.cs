using Project_Invoice_MAUI.Models;

namespace Project_Invoice_MAUI.Singleton
{
    public sealed class Firma
    {
        private Firma() { }

        private static Firma _instance;

        public static Firma GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Firma();
            }
            return _instance;
        }


        public string Name { get; }

        public int NumerFaktury { get; set; }

        public BankAccount BankAccount { get; set; }

        public BossData BossData { get; set; }

        public CompanyData CompanyData { get; set; }

        public DocumentNumbering DocumentNumbering { get; set; }

        public List<Kontrahent> kontrahents { get; set; } = new();

        public List<Goods> goods { get; set; } = new();

    }
}
