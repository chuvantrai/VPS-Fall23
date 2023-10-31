
/* Unmerged change from project 'Client.MobileApp (net7.0-ios)'
Before:
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Handlers.Compatibility;


#if ANDROID
After:
#if ANDROID
*/

/* Unmerged change from project 'Client.MobileApp (net7.0-maccatalyst)'
Before:
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Handlers.Compatibility;


#if ANDROID
After:
#if ANDROID
*/

/* Unmerged change from project 'Client.MobileApp (net7.0-windows10.0.19041.0)'
Before:
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Handlers.Compatibility;


#if ANDROID
After:
#if ANDROID
*/
#if ANDROID
using Microsoft.Maui.Graphics;
using Android.Content;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Android.Views;
using Android.Widget;
namespace Client.MobileApp
{
    public partial class BorderEntry : Entry
    {
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(BorderEntry), 1);
        //public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BorderEntry), Color.Black);

        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        //public Color BorderColor
        //{
        //    get => (Color)GetValue(BorderColorProperty);
        //    set => SetValue(BorderColorProperty, value);
        //}
    }
}
#endif
