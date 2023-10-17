using CommunityToolkit.Maui.Views;

namespace Client.MobileApp.Views;

public partial class LicenseInputPopup : Popup
{
	public LicenseInputPopup()
	{
		InitializeComponent();
	}

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void OkButton_Clicked(object sender, EventArgs e)
    {
        string licensePlate = LicensePlateEntry.Text;
    }
}