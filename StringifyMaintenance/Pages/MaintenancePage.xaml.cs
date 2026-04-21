using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using StringifyMaintenance.Models;
using StringifyMaintenance.Services;

namespace StringifyMaintenance.Pages;

public partial class MaintenancePage : Page, INotifyPropertyChanged
{
    private readonly MaintenanceRepository _repository;
    private readonly MainWindow _mainWindow;

    public ObservableCollection<Termek> Products { get; } = new();
    public ObservableCollection<User> Users { get; } = new();
    public ObservableCollection<Rendeles> Orders { get; } = new();

    private Termek? _selectedProduct;
    public Termek? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (_selectedProduct != value)
            {
                _selectedProduct = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedProduct));
                if (_selectedProduct == null)
                {
                    SelectedProductImages = CreateNewProductImages();
                }
                else
                {
                    LoadSelectedProductImagesAsync(_selectedProduct.Id);
                }
            }
        }
    }

    private TermekKepek _selectedProductImages = CreateNewProductImages();
    public TermekKepek SelectedProductImages
    {
        get => _selectedProductImages;
        set
        {
            if (_selectedProductImages != value)
            {
                _selectedProductImages = value;
                OnPropertyChanged();
            }
        }
    }

    private User? _selectedUser;
    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser != value)
            {
                _selectedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedUser));
            }
        }
    }

    private Rendeles? _selectedOrder;
    public Rendeles? SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            if (_selectedOrder != value)
            {
                _selectedOrder = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedOrder));
            }
        }
    }

    private Termek _newProduct = CreateNewProduct();
    public Termek NewProduct
    {
        get => _newProduct;
        set
        {
            if (_newProduct != value)
            {
                _newProduct = value;
                OnPropertyChanged();
            }
        }
    }

    private TermekKepek _newProductImages = CreateNewProductImages();
    public TermekKepek NewProductImages
    {
        get => _newProductImages;
        set
        {
            if (_newProductImages != value)
            {
                _newProductImages = value;
                OnPropertyChanged();
            }
        }
    }

    private User _newUser = CreateNewUser();
    public User NewUser
    {
        get => _newUser;
        set
        {
            if (_newUser != value)
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }
    }

    private Rendeles _newOrder = CreateNewOrder();
    public Rendeles NewOrder
    {
        get => _newOrder;
        set
        {
            if (_newOrder != value)
            {
                _newOrder = value;
                OnPropertyChanged();
            }
        }
    }

    public string CurrentUserName => Session.CurrentUser?.Nev ?? "Unknown";

    public bool HasSelectedProduct => SelectedProduct != null;
    public bool HasSelectedUser => SelectedUser != null;
    public bool HasSelectedOrder => SelectedOrder != null;

    public MaintenancePage()
    {
        InitializeComponent();
        _repository = App.Services.GetRequiredService<MaintenanceRepository>();
        _mainWindow = (MainWindow)Application.Current.MainWindow;
        DataContext = this;
        Loaded += MaintenancePage_Loaded;
    }

    private async void MaintenancePage_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        try
        {
            await LoadProductsAsync();
            await LoadUsersAsync();
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Bet�lt�si hiba: {ex.Message}\n\n{ex.InnerException?.Message}",
                            "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadProductsAsync()
    {
        var products = await _repository.GetProductsAsync();
        await Dispatcher.InvokeAsync(() =>
        {
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
            OnPropertyChanged(nameof(Products));
        });
    }

    private async Task LoadUsersAsync()
    {
        Users.Clear();
        foreach (var user in await _repository.GetUsersAsync())
            Users.Add(user);
    }

    private async Task LoadOrdersAsync()
    {
        Orders.Clear();
        foreach (var order in await _repository.GetOrdersAsync())
            Orders.Add(order);
    }

    private async void AddProduct_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NewProduct.Nev) || string.IsNullOrWhiteSpace(NewProduct.Leiras))
        {
            MessageBox.Show("Name and description are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidateImages(NewProductImages, out var imageError))
        {
            MessageBox.Show(imageError, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (NewProduct.Letrehozva == null)
        {
            NewProduct.Letrehozva = DateTime.Now;
        }

        var product = await _repository.AddProductAsync(NewProduct);
        NewProductImages.TermekId = product.Id;
        await _repository.UpsertProductImagesAsync(product.Id, NewProductImages);
        NewProduct = CreateNewProduct();
        NewProductImages = CreateNewProductImages();
        await LoadProductsAsync();
    }

    private async void RefreshProducts_Click(object sender, RoutedEventArgs e)
    {
        await LoadProductsAsync();
    }

    private async void UpdateProduct_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedProduct == null)
        {
            return;
        }

        if (!ValidateImages(SelectedProductImages, out var imageError))
        {
            MessageBox.Show(imageError, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await _repository.UpdateProductAsync(SelectedProduct);
        SelectedProductImages.TermekId = SelectedProduct.Id;
        await _repository.UpsertProductImagesAsync(SelectedProduct.Id, SelectedProductImages);
        await LoadProductsAsync();
    }

    private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedProduct == null)
        {
            return;
        }

        var result = MessageBox.Show("Delete selected product?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        await _repository.DeleteProductAsync(SelectedProduct.Id);
        SelectedProduct = null;
        await LoadProductsAsync();
    }

    private void ClearNewProduct_Click(object sender, RoutedEventArgs e)
    {
        NewProduct = CreateNewProduct();
        NewProductImages = CreateNewProductImages();
    }

    private async void AddUser_Click(object sender, RoutedEventArgs e)
    {
        string password = NewUserPasswordBox.Password;
        if (string.IsNullOrWhiteSpace(NewUser.Nev) || string.IsNullOrWhiteSpace(NewUser.Email))
        {
            MessageBox.Show("Name and email are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Password is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await _repository.AddUserAsync(NewUser, password);
        NewUserPasswordBox.Password = string.Empty;
        NewUser = CreateNewUser();
        await LoadUsersAsync();
    }

    private async void UpdateUser_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedUser == null)
        {
            return;
        }

        string newPassword = UpdateUserPasswordBox.Password;
        await _repository.UpdateUserAsync(SelectedUser, string.IsNullOrWhiteSpace(newPassword) ? null : newPassword);
        UpdateUserPasswordBox.Password = string.Empty;
        await LoadUsersAsync();
    }

    private async void DeleteUser_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedUser == null)
        {
            return;
        }

        var result = MessageBox.Show("Delete selected user?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        await _repository.DeleteUserAsync(SelectedUser.Id);
        SelectedUser = null;
        await LoadUsersAsync();
    }

    private void ClearNewUser_Click(object sender, RoutedEventArgs e)
    {
        NewUserPasswordBox.Password = string.Empty;
        NewUser = CreateNewUser();
    }

    private async void AddOrder_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NewOrder.Status))
        {
            MessageBox.Show("Status is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (NewOrder.Datum == null)
        {
            NewOrder.Datum = DateTime.Now;
        }

        await _repository.AddOrderAsync(NewOrder);
        NewOrder = CreateNewOrder();
        await LoadOrdersAsync();
    }

    private async void UpdateOrder_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedOrder == null)
        {
            return;
        }

        await _repository.UpdateOrderAsync(SelectedOrder);
        await LoadOrdersAsync();
    }

    private async void DeleteOrder_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedOrder == null)
        {
            return;
        }

        var result = MessageBox.Show("Delete selected order?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        await _repository.DeleteOrderAsync(SelectedOrder.Id);
        SelectedOrder = null;
        await LoadOrdersAsync();
    }

    private void ClearNewOrder_Click(object sender, RoutedEventArgs e)
    {
        NewOrder = CreateNewOrder();
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        Session.CurrentUser = null;
        _mainWindow.NavigateToLogin();
    }

    private static Termek CreateNewProduct()
    {
        return new Termek
        {
            Elerheto = true,
            Letrehozva = DateTime.Now
        };
    }

    private static TermekKepek CreateNewProductImages()
    {
        return new TermekKepek
        {
            Kep1 = string.Empty,
            Kep2 = string.Empty,
            Kep3 = string.Empty,
            Kep4 = string.Empty,
            Kep5 = string.Empty
        };
    }

    private static User CreateNewUser()
    {
        return new User
        {
            Jogosultsag = 1,
            Aktiv = true
        };
    }

    private static Rendeles CreateNewOrder()
    {
        return new Rendeles
        {
            Status = "New",
            Datum = DateTime.Now
        };
    }

    private async void LoadSelectedProductImagesAsync(int productId)
    {
        try
        {
            SelectedProductImages = await _repository.GetProductImagesAsync(productId) ?? CreateNewProductImages();
            SelectedProductImages.TermekId = productId;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Image loading error: {ex.Message}\n\n{ex.InnerException?.Message}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            SelectedProductImages = CreateNewProductImages();
        }
    }

    private static bool ValidateImages(TermekKepek images, out string errorMessage)
    {
        NormalizeImages(images);
        int count = CountImages(images);

        if (count < 1)
        {
            errorMessage = "Please provide at least 1 image URL.";
            return false;
        }

        if (count > 5)
        {
            errorMessage = "You can provide up to 5 image URLs.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static int CountImages(TermekKepek images)
    {
        return new[] { images.Kep1, images.Kep2, images.Kep3, images.Kep4, images.Kep5 }
            .Count(value => !string.IsNullOrWhiteSpace(value));
    }

    private static void NormalizeImages(TermekKepek images)
    {
        images.Kep1 = NormalizeImageUrl(images.Kep1);
        images.Kep2 = NormalizeImageUrl(images.Kep2);
        images.Kep3 = NormalizeImageUrl(images.Kep3);
        images.Kep4 = NormalizeImageUrl(images.Kep4);
        images.Kep5 = NormalizeImageUrl(images.Kep5);
    }

    private static string NormalizeImageUrl(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
