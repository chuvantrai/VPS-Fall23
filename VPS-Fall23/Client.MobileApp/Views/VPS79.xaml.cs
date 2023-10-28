using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;
using static Google.Rpc.Context.AttributeContext.Types;

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

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(UserName.Text) && !String.IsNullOrEmpty(Password.Text))
        {
            LoginRequest loginRequest = new() { Username = UserName.Text, Password = Password.Text };

            var response = await _viewModel.CheckAccount(loginRequest);

            if (response != null)
            {
                if (response.Equals(Constant.LOGIN_SUCCESS))
                {
                    await DisplayAlert(Constant.NOTIFICATION, response, Constant.CANCEL);
                    await Task.Delay(1000);
                    await Shell.Current.GoToAsync(nameof(VPS53));
                }
                else
                {
                    await DisplayAlert(Constant.ALERT, response, Constant.CANCEL);
                }
            }
        }
        else
        {
            await DisplayAlert(Constant.ALERT, Constant.LOGIN_FAILED, Constant.CANCEL);
        }
    }
}