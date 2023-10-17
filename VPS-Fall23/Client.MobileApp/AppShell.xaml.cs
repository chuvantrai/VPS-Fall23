using Client.MobileApp.Views;

namespace Client.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VPS53), typeof(VPS53));
        }
    }
}