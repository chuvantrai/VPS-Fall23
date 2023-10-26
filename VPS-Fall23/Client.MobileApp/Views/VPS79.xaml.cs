using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;

namespace Client.MobileApp.Views;

public partial class VPS79 : ContentPage
{
    private readonly VPS79ViewModel _viewModel;
    public VPS79()
    {
        VPS79ViewModel viewModel = new();
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void loginButton_Clicked(object sender, EventArgs e)
    {

    }
}