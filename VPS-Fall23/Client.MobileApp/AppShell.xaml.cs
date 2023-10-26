using Client.MobileApp.Views;

namespace Client.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VPS53), typeof(VPS53));
            Routing.RegisterRoute(nameof(VPS61), typeof(VPS61));
            Routing.RegisterRoute(nameof(VPS79), typeof(VPS79));
        }
    }
}