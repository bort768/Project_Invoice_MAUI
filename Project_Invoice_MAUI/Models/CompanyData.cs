using SQLite;

namespace Project_Invoice_MAUI.Models

{
    public class CompanyData
    {

        public string Full_Name { get; set; }
        [PrimaryKey]
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Street { get; set; }
        public string House_Number { get; set; }
        //public string? Apartment_Number { get; }
        public string ZIP_Code { get; set; }
        public string Town { get; set; }

        //public CompanyData(string full_Name, string nIP, string rEGON, string street, string house_Number, string zIP_Code, string town)
        //{
        //    Full_Name = full_Name;
        //    NIP = nIP;
        //    REGON = rEGON;
        //    Street = street;
        //    House_Number = house_Number;
        //    ZIP_Code = zIP_Code;
        //    Town = town;
        //}




        public bool ValidateNip(string nip)
        {
            nip = nip.Replace("-", string.Empty);

            if (nip.Length != 10 || nip.Any(chr => !char.IsDigit(chr))) return false;

            int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7, 0 };
            int sum = nip.Zip(weights, (digit, weight) => (digit - '0') * weight).Sum();

            return (sum % 11) == (nip[9] - '0');

        }
    }
}
