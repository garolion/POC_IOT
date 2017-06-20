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
using Windows.UI.Xaml.Navigation;

using POC_IOT.Helper;

namespace POC_IOT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ConfigHelper.Init(false);

            ParametragePane.RegisterAccueilPane(AccueilPane);
            ParametragePane.Visibility = Visibility.Collapsed;
        }

        private void AccueilButton_Click(object sender, RoutedEventArgs e)
        {
            AccueilPane.Visibility = Visibility.Visible;
            ParametragePane.Visibility = Visibility.Collapsed;
        }

        private void ParametrageButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            AccueilPane.Visibility = Visibility.Collapsed;
            ParametragePane.Visibility = Visibility.Visible;
        }
    }
}
