using Camera.MAUI;
using Client.MobileApp.ViewModels;
using Client.MobileApp.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;


#if ANDROID
[assembly: Android.App.UsesPermission(Android.Manifest.Permission.Camera)]
#endif
namespace Client.MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterViewModels()
                .RegisterViews()
                .UseMauiCameraView()
                .RegisterAppServices()
                .UseSkiaSharp()
                ;
            builder.Services.AddSingleton<IBrowser>(Browser.Default);
            builder.Services.AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            _ = mauiAppBuilder.Services.AddSingleton<AppShell>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<VPS53>();
            builder.Services.AddTransient<VPS79>();

            Routing.RegisterRoute(nameof(VPS53), typeof(VPS53));
            Routing.RegisterRoute(nameof(VPS79), typeof(VPS79));

            return builder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<VPS53ViewModel>();
            builder.Services.AddTransient<VPS79ViewModel>();

            return builder;
        }
    }
}