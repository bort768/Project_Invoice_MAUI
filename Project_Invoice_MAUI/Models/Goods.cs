using Project_Invoice_MAUI.SaveFileHelper;

namespace Project_Invoice_MAUI.Models
{
    public class Goods
    {
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }
        public int Product_Id { get; set; }
        public string Description { get; set; }
        public double Price_Netto { get; set; }
        public double Price_Brutto { get; set; }
        public double Sum { get; set; }
        public double VAT { get; set; }
        private string _Vat_string;
        public string VAT_String { get => _Vat_string; 
                set 
            {
                _Vat_string = value;
                VAT = Vat_Helper.Vat_dictionary[VAT_String];
                if (Price_Netto != 0)
                {                   
                    Price_Brutto = Math.Round(Price_Netto * VAT, 2);
                }

            } }
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }

        // można dodać walute i inne parametry

        public Goods(string product_Name, string product_Code, string description,
            double price_Netto, double price_Brutto, double vAT, string _vAT_String)
        {
            Product_Name = product_Name;
            Product_Code = product_Code;
            //Product_Id = product_Id;
            Description = description;
            Price_Netto = price_Netto;
            Price_Brutto = price_Brutto;
            VAT = vAT;
            VAT_String = _vAT_String;
        }

        public override string ToString()
        {
            return $"{Product_Name}";
        }
    }
}
