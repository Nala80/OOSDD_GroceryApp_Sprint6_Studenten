using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        Client client;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int stock;

        [ObservableProperty]
        private DateTime shelfLife = DateTime.Now;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string errorMessage;

        public NewProductViewModel(IProductService productService, GlobalViewModel global)
        {
            _productService = productService;
            client = global.Client;
        }

        [RelayCommand]
        private void AddProduct()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Productnaam is verplicht";
                return;
            }
            if (Stock < 0)
            {
                ErrorMessage = "Voorraad moet groter zijn dan 0";
                return;
            }
            if (Price < 0)
            {
                ErrorMessage = "Prijs moet groter zijn dan 0.";
                return;
            }
            DateOnly ShelfLife = DateOnly.FromDateTime(this.ShelfLife);
            if (ShelfLife < DateOnly.FromDateTime(DateTime.Now))
            {
                ErrorMessage = "Voorraad kan niet in het verleden zijn";
                return;
            }

            // try to add an id that is one higher than the current highest product id
            var products = _productService.GetAll();
            int id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;

            Product newProduct = new(id, Name, Stock, ShelfLife, Price);
            _productService.Add(newProduct);
            if (newProduct == null)
            {
                ErrorMessage = "Fout bij opslaan.";
            }
        }
    }
}
