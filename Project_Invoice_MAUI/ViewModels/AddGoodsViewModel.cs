using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SaveFileHelper;
using Project_Invoice_MAUI.Singleton;

namespace Project_Invoice_MAUI.ViewModels
{
    public partial class AddGoodsViewModel : BaseViewModel
    {
        #region Zmienne


        Firma firma = Firma.GetInstance();

        public List<string> Vat_Combobox { get; set; }

        private string _Vat_Selected_Item;
        public string Vat_Selected_Item
        {
            get
            {
                return _Vat_Selected_Item;
            }
            set
            {
                _Vat_Selected_Item = value;
                AssignToVAT(value);
                Price_Brutto = _price_Netto * _VAT;
                Price_Brutto_To_String = Math.Round((_price_Netto * _VAT), 2).ToString();
                OnPropertyChanged();
            }
        }

        private double _VAT;
        public double VAT
        {
            get
            {
                return _VAT;
            }
            set
            {
                _VAT = value;
                OnPropertyChanged();
            }
        }

        private string _selected_Item;
        public string Selected_Item
        {
            get
            {
                return _selected_Item;
            }
            set
            {
                int index = 0;
                foreach (var goods in firma.goods)
                {
                    if (goods.Product_Name == value)
                    {
                        SaveToLocal(index);
                    }
                    index++;
                }
                _selected_Item = value;
                OnPropertyChanged();
            }
        }
        //private static int Product_ID;

        public ObservableCollection<string> LastVisetedGoods { get; set; }



        private string _product_Name;
        public string Product_Name
        {
            get
            {
                return _product_Name;
            }
            set
            {
                _product_Name = value;
                OnPropertyChanged();
            }
        }

        private string _product_Code;
        public string Product_Code
        {
            get
            {
                return _product_Code;
            }
            set
            {
                _product_Code = value;
                OnPropertyChanged();
            }
        }

        private int _product_ID;
        public int Product_ID
        {
            get
            {
                return _product_ID;
            }
            set
            {
                _product_ID = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }


        private string _price_Netto_To_String;
        public string Price_Netto_To_String
        {
            get
            {
                return _price_Netto_To_String;
            }
            set
            {
                _price_Netto_To_String = value;
                try
                {
                    if (Convert.ToDouble(value) > 0)
                    {
                        Price_Netto = Convert.ToDouble(value);
                        AssignToVAT(_Vat_Selected_Item);
                        Price_Brutto = Math.Round(_price_Netto * _VAT, 2);
                        Price_Brutto_To_String = Math.Round((_price_Netto * _VAT), 2).ToString();
                    }
                    else
                    {
                        //MessageBox.Show("Wartość jest zerowa lub ujemna", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                catch (Exception)
                {
                    //MessageBox.Show("Wartość nie jest liczbą", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                OnPropertyChanged();
            }
        }
        //
        //czy jest potrzeba propchange na to uzywać?
        private double _price_Netto;
        public double Price_Netto
        {
            get
            {
                return _price_Netto;
            }
            set
            {
                _price_Netto = value;
                OnPropertyChanged();
            }
        }

        private string _price_Brutto_To_String;
        public string Price_Brutto_To_String
        {
            get
            {
                return _price_Brutto_To_String;
            }
            set
            {
                _price_Brutto_To_String = value;

                //w sumie nie potrzebne jak jest disable
                //try
                //{
                //    Price_Brutto = Convert.ToDouble(value);
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Wartość nie jest liczbą", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                OnPropertyChanged();
            }
        }
        //
        private double _price_Brutto;
        public double Price_Brutto
        {
            get
            {
                return _price_Brutto;
            }
            set
            {
                _price_Brutto = Math.Round(value, 2);
                //Price_Brutto_To_String = value.ToString();
                OnPropertyChanged();
            }
        }


        #endregion


        public AddGoodsViewModel()
        {
            Vat_Combobox = new();
            Vat_Combobox.Add(Vat_Helper.VAT_23_String);
            Vat_Combobox.Add(Vat_Helper.VAT_7_String);
            Vat_Combobox.Add(Vat_Helper.VAT_6_String);
            Vat_Combobox.Add(Vat_Helper.VAT_3_String);
            Vat_Combobox.Add(Vat_Helper.VAT_0_String);

            LastVisetedGoods = new();

            //Hardcode dane



            if (firma.goods != null)
            {
                foreach (var Goods_Combobox in firma.goods)
                {
                    LastVisetedGoods.Add(Goods_Combobox.ToString());
                }
            }
            else
            {
                firma.goods = new();
                firma.goods.Add(new Goods("Produkt", "23", "Tak", 23, 28.29, Vat_Helper.VAT_23, Vat_Helper.VAT_23_String));
                firma.goods.Add(new Goods("Taśma", "78", "Tak", 40, 49.2, Vat_Helper.VAT_7, Vat_Helper.VAT_7_String));
                firma.goods.Add(new Goods("UwU Shrek UwU", "69", "Tak", 100, 123, Vat_Helper.VAT_3, Vat_Helper.VAT_3_String));
            }





        }


        #region Commands
        [ICommand]
        public void DeleteGoodsCommand()
        {
            if (firma.goods.Contains(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item)))
            {
                firma.goods.Remove(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item));

                //MessageBox.Show("Towar/Usługa została usunięta", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //MessageBox.Show("Coś poszło nie tak", ":/", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [ICommand]
        public void GetGoodsCommand()
        {
            //submit goods
            firma.goods.Add(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item));
            //MessageBox.Show("Towar/Usługa został dodany", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            //TO DO: sprawdz czy już istnieje
        }
        #endregion

        #region Metody

        private void SaveToLocal(int index)
        {
            Product_Code = firma.goods[index].Product_Code;
            Product_Name = firma.goods[index].Product_Name;
            Description = firma.goods[index].Description;
            Price_Netto = firma.goods[index].Price_Netto;
            Price_Brutto = firma.goods[index].Price_Brutto;
            Product_ID = firma.goods[index].Product_Id;

            Price_Netto_To_String = Price_Netto.ToString();


        }

        private void AssignToVAT(string value)
        {
            if (value == Vat_Helper.VAT_23_String)
            {
                VAT = Vat_Helper.VAT_23;
            }
            if (value == Vat_Helper.VAT_7_String)
            {
                VAT = Vat_Helper.VAT_7;
            }
            if (value == Vat_Helper.VAT_6_String)
            {
                VAT = Vat_Helper.VAT_6;
            }
            if (value == Vat_Helper.VAT_3_String)
            {
                VAT = Vat_Helper.VAT_3;
            }
            if (value == Vat_Helper.VAT_0_String)
            {
                VAT = Vat_Helper.VAT_0;
            }
        }
        #endregion
    }
}
