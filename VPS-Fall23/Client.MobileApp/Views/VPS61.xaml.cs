using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Maui.Views;

namespace Client.MobileApp.Views;

public partial class VPS61 : Popup
{
    private readonly VPS61ViewModel _viewModel;
    public VPS61()
    {
        VPS61ViewModel viewModel = new();
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = AreaCodeEntry.Text + "-" + LicensePlateEntry.Text;

            if (!String.IsNullOrEmpty(licensePlate))
            {
                var checkLicensePlate = new LicensePlateInput
                {
                    LicensePlate = licensePlate,
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                string response_1 = await _viewModel.CheckLicensePLate(checkLicensePlate);

                if (response_1 == Constant.CHECKOUT_CONFIRM)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var answer = await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                        if (answer.ToString() == Constant.ACCEPT)
                        {
                            string response_2 = await _viewModel.CheckOutConfirm(checkLicensePlate);

                            await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_2, Constant.CANCEL);
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.CANCEL);
                        }
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
                });
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });
        }
    }

    private async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        await LicensePlateEntry.HideKeyboardAsync(CancellationToken.None);
        await AreaCodeEntry.HideKeyboardAsync(CancellationToken.None);

        LicensePlateEntry.Unfocus();
        AreaCodeEntry.Unfocus();
    }
}