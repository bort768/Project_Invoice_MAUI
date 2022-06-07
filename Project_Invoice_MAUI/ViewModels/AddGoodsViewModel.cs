using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SaveFileHelper;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;

namespace Project_Invoice_MAUI.ViewModels
{
    [QueryProperty(nameof(Goods), "Goods")]
    public partial class AddGoodsViewModel : BaseViewModel
    {
        #region Zmienne


        Firma firma = Firma.GetInstance();

        public List<string> Vat_Combobox { get; set; }

        //[ObservableProperty]
        //Goods goods2;

        Goods _goods = Firma.Static_Goods;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Is_Not_Selected))]
        private bool _is_Selected;

        public bool Is_Not_Selected => !Is_Selected;
        

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

        [ObservableProperty]
        private double _VAT;


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


        [ObservableProperty]
        private string _product_Name;

        [ObservableProperty]
        private string _product_Code;

        [ObservableProperty]
        private int _product_ID;

        [ObservableProperty]
        private string _description;



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
        
        [ObservableProperty]
        private double _price_Netto;

        [ObservableProperty]
        private string _price_Brutto_To_String;

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

            if (_goods is not null)
            {
                _product_Name = _goods.Product_Name;
                _product_Code = _goods.Product_Code;
                _price_Netto = _goods.Price_Netto;
                _price_Brutto = _goods.Price_Brutto;
                _description = _goods.Description;
                _Vat_Selected_Item = _goods.VAT_String;
                _is_Selected = _goods.IsSelected;
            }
            else
            {
                _is_Selected = false;
            }



            //if (firma.goods != null)
            //{
            //    foreach (var Goods_Combobox in firma.goods)
            //    {
            //        LastVisetedGoods.Add(Goods_Combobox.ToString());
            //    }
            //}
            //else
            //{
            //    firma.goods = new();

            //}

        }


        #region Commands
        [ICommand]
        public async void DeleteGoods()
        {
            try
            {
                var good = new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item);

                await GoodsService.DeleteGoods(good);
                await Shell.Current.DisplayAlert("Towar usunięty", "Towar został usunięty", "ok");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "ok");
                
            }
            
        }

        [ICommand]
        public async void UpdateGoods()
        {
            try
            {
                var good = new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item);

                await GoodsService.UpdateGoods(good);
                await Shell.Current.DisplayAlert("Towar usunięty", "Towar został usunięty", "ok");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "ok");
                throw;
            }

        }

        [ICommand]
        public async void AddGoods()
        {
            //submit goods
            var containsConflict = firma.goods.Where(p => p.Product_Code == _product_Code);

            if (containsConflict.Any())
            {
                await Shell.Current.DisplayAlert("Błąd", "towar z tym kodem jest już w bazie", "ok");
            }
            else
            {
                await GoodsService.AddGoods(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item));
                firma.goods.Add(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item));
                await ToastSaveSucces();
            }



            //firma.goods.Add(new Goods(_product_Name, _product_Code, _description, _price_Netto, _price_Brutto, _VAT, _Vat_Selected_Item));


            //MessageBox.Show("Towar/Usługa został dodany", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            //TO DO: sprawdz czy już istnieje // done
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
