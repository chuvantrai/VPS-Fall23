#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace Client.MobileApp
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = appShell;

#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            });
#endif
        }
    }
}
