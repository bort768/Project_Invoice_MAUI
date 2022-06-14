using Newtonsoft.Json;
using Project_Invoice_MAUI.Models;
using Project_Invoice_MAUI.SaveFileHelper;
using Project_Invoice_MAUI.SavePathHelpers;
using Project_Invoice_MAUI.Services;
using Project_Invoice_MAUI.Singleton;
//using System.Collections.ObjectModel;

namespace Project_Invoice_MAUI.ViewModels
{ 
    public partial class InvoiceViewModel : BaseViewModel
    {
        #region zmienne
        Firma firma = Firma.GetInstance();

        ISaveToFile MetodOfSave;

        //public CommandBase SubmitInvoiceCommand { get; set; }
        //public CommandBase AddGoodToListCommand { get; set; }
        //public CommandBase RemoveGoodFromListCommand { get; set; }
        //public CommandBase EditGoodToListCommand { get; set; }

        public ObservableCollection<Goods> List_Of_Added_Goods { get; set; } = new();
        public ObservableCollection<Kontrahent> List_Of_Kontrahents { get; set; } = new();
        public ObservableCollection<Goods> Add_Remove_Goods_List { get; set; } = new();
        public ObservableCollection<Goods> Filtered_List { get; set; } = new();

        [ObservableProperty]
        private Kontrahent _selected_Kontrahent;


        public CompanyData companyData { get; set; }

        public static int Invoice_Number = 1;

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                var SelectedItem = from x in List_Of_Added_Goods where (x.IsSelected == true) select x;
                Selected_Goods = (Goods)SelectedItem;
                OnPropertyChanged();
            }
        }


        public string Town { get; set; }


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
                        break;
                    }
                    index++;
                }
                _selected_Item = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private int _quantity2 = 1;


        private int _quantity = 1;
        public string Quantity
        {
            get
            {
                return _quantity.ToString();
            }
            set
            {
                if (value != null && value != "")
                {
                    _quantity = Convert.ToInt32(value!);
                    if (Selected_Goods != null)
                    {
                        Selected_Goods.Quantity = _quantity;
                        Selected_Goods.Sum = (_quantity * Selected_Goods.Price_Brutto);
                    }

                }

                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private Goods _selected_Goods;

        [ObservableProperty]
        private string _searchText;

        partial void OnSearchTextChanged(string value)
        {
            SearchBar(value);
        }



        public Kontrahent kontrahent { get; set; }


        private DateTime _dataWystawienia = DateTime.Now;
        public DateTime DataWystawienia
        {
            get
            {
                return _dataWystawienia;
            }
            set
            {
                _dataWystawienia = value;
                Wypisanie_Invoice_Format();
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private DateTime _dataDostawy = DateTime.Now;



        [ObservableProperty]
        private string _invoice_Format;



        #endregion

        public InvoiceViewModel()
        {
            if (firma.CompanyData != null)
            {
                Town = firma.CompanyData.Town;
                companyData = firma.CompanyData;
            }

            GetCompanyData();



            if (firma.goods != null)
            {
                //Add_Remove_Goods_List = firma.goods;
                foreach (var goods in firma.goods)
                {
                    Filtered_List.Add(goods);
                }
            }
            else
            {
                //Task load = GetGoodsAsync();
            }

            //foreach (var goods in firma.goods)
            //{
            //    Filtered_List.Add(goods);
            //}

            if (firma.kontrahents == null)
            {
                //Task loadKontrahents = GetKontrahents();
            }

            Wypisanie_Invoice_Format();

            //dodawanie i towrzenie listy kontrahentów do comobobox

            foreach (var kontrahent in firma.kontrahents!)
            {
                List_Of_Kontrahents.Add(kontrahent);
            }

            //Add_Remove_Goods_List = new();

        }


        #region Commands
        [ICommand]
        public void AddGoodToList(Goods goods)
        {
            Add_Goods_To_List(goods);
        }

        [ICommand]
        public void EditGoodToList(Goods goods)
        {
            Edit_Goods_To_List(goods);
        } // zmien tu trochę

        [ICommand]
        public void RemoveGoodFromList(Goods goods)
        {
            List_Of_Added_Goods.Remove(goods);
        }

        [ICommand]
        public void SubmitInvoice()
        {
            MetodOfSave = new SaveAsPdf(firma.DocumentNumbering, Selected_Kontrahent, firma.CompanyData!, firma.BossData, firma.BankAccount,
                List_Of_Added_Goods.ToList(), DataWystawienia, Invoice_Format, Invoice_Number);
            MetodOfSave.SaveToFile();
            Invoice_Number++;
            Wypisanie_Invoice_Format();
        }

        [ICommand]
        void SearchBar(string text)
        {
            List<Goods> filteredList = firma.goods.Where(r => r.Product_Name.ToLower().Contains(text.ToLower())).ToList();
            firma.goods.Count();
            //czyszczenie list ponieważ robią się duplikaty
            Filtered_List.Clear();
            filteredList.ForEach(goods => Filtered_List.Add(goods));
            //Add_Remove_Goods_List = Filtered_List;
                //kontrahents.ForEach(kontrahent => List_of_Kontrahents.Add(kontrahent));

        }

        [ICommand]
        void SelectObject(Goods goods)
        {
            var selectedGoods = List_Of_Added_Goods.Where(x => x.Equals(goods));
        }

        #endregion



        #region Metody


        private void Edit_Goods_To_List(Goods goods_To_Edit)
        {
            var find_Item = List_Of_Added_Goods.Where(r => r.Product_Code == goods_To_Edit.Product_Code);
            foreach (var item in find_Item)
            {
                List_Of_Added_Goods.Remove(item);
            }


        }

        private void Add_Goods_To_List(Goods goods_To_Add)
        {
           goods_To_Add.Quantity= _quantity2;
           goods_To_Add.Sum = (_quantity2 * goods_To_Add.Price_Brutto);
           List_Of_Added_Goods.Add(goods_To_Add);
        }



        private void SaveToLocal(int index)
        {
            Selected_Goods = firma.goods[index];
        }

        private void Wypisanie_Invoice_Format()
        {
            if (firma.DocumentNumbering != null)
            {
                if (firma.DocumentNumbering.Broken_By_Year && firma.DocumentNumbering.Broken_By_Mounth)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("MM")}/{DataWystawienia.ToString("yyyy")}";
                }
                else if (firma.DocumentNumbering.Broken_By_Year)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("yyyy")}";
                }
                else if (firma.DocumentNumbering.Broken_By_Mounth)
                {
                    Invoice_Format = $"FS {Invoice_Number}/{DataWystawienia.ToString("MM")}";
                }
                else
                {
                    Invoice_Format = $"FS {Invoice_Number}";
                }
            }
            else
            {
                Invoice_Format = $"FS {Invoice_Number}";
            }

        }


        public async void LoadData()
        {
            await GetGoodsAsync();
            GetCompanyData();
            await GetKontrahents();

        }

        void GetCompanyData()
        {
            var stream = FileSystem.Current.AppDataDirectory + JsonFilesPath.COMPANY_DATA;
            if (File.Exists(stream))
            {
                firma.CompanyData = JsonConvert.DeserializeObject<CompanyData>(File.ReadAllText(stream));
                Town = firma.CompanyData.Town;
                companyData = firma.CompanyData;
            }

            var streamDN = FileSystem.Current.AppDataDirectory + JsonFilesPath.DOCUMENT_NUMBERING;
            if (File.Exists(streamDN))
            {
                firma.DocumentNumbering = JsonConvert.DeserializeObject<DocumentNumbering>(File.ReadAllText(streamDN));
            }

            var streamBA = FileSystem.Current.AppDataDirectory + JsonFilesPath.BANK_ACCOUNT;
            if (File.Exists(streamBA))
            {
                firma.BankAccount = JsonConvert.DeserializeObject<BankAccount>(File.ReadAllText(streamBA));
            }
        }

        async Task GetGoodsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var Goods = await GoodsService.GetGoods();

                if (firma.goods.Count != 0)
                    firma.goods.Clear();

                //no nie jest to optymalne
                firma.goods = Goods;

                foreach (var goods in firma.goods)
                {
                    Filtered_List.Add(goods);
                }

                // wywala bład// juz nie
                //Goods.ForEach(Goods => List_of_good.Add(Goods));


            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        async Task GetKontrahents()
        {

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var kontrahents = await KontrahentService.GetKontrahents();

                //AddRadomData();
                //no nie jest to optymalne
                firma.kontrahents = kontrahents;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
