using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace HandyTools.Control
{
    public sealed partial class RateControl : UserControl
    {
        
        public static readonly DependencyProperty RateProperty = DependencyProperty.Register("Rate", typeof(int), typeof(RateControl), null);
        public int Rate
        {
            get { return (int)GetValue(RateProperty); }
            set { SetValue(RateProperty, value); }
        }
        
        
        public RateControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Rate == 0)
                this.Visibility = Visibility.Collapsed;
            RateStackPanel.Children.Clear();
            for (int i = 0; i < Rate; i++)
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:/Assets/imgs/rate.png", UriKind.Absolute)),
                    Stretch = Stretch.None,
                    Margin = new Thickness(0,0,5,0)
                }; 
                RateStackPanel.Children.Add(image);
            }
        }
    }
}
