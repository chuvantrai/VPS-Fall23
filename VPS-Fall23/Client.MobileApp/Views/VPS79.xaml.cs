using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Core.Platform;

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
        CheckLoginStatus();
    }

    private async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        await UserName.HideKeyboardAsync(CancellationToken.None);
        await Password.HideKeyboardAsync(CancellationToken.None);

        UserName.Unfocus();
        Password.Unfocus();
    }

    private async void CheckLoginStatus()
    {
        var storedToken = await SecureStorage.GetAsync("UserToken");

        if (!string.IsNullOrEmpty(storedToken))
        {
            await Shell.Current.GoToAsync(nameof(VPS53));
        }
    }

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        string response = String.Empty;
        if (!String.IsNullOrEmpty(UserName.Text) && !String.IsNullOrEmpty(Password.Text))
        {
            LoginRequest loginRequest = new() { Username = UserName.Text, Password = Password.Text };

            if (Remember.IsChecked == true)
            {
                response = await _viewModel.CheckAccount(loginRequest, true);
            }
            else
            {
                response = await _viewModel.CheckAccount(loginRequest, false);
            }

            if (response != null)
            {
                if (response.Equals(Constant.LOGIN_SUCCESS))
                {
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