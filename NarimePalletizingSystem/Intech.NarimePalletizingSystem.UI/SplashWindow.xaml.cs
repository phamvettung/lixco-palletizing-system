using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Intech.NarimePalletizingSystem.UI
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        Thread loadingThread;
        Storyboard Showboard;
        Storyboard Hideboard;

        private delegate void ShowDelegate(string txt);
        private delegate void HideDelegate();
        ShowDelegate OnShow;
        HideDelegate OnHide;

        public SplashWindow()
        {
            InitializeComponent();
            Loaded += SplashWindow_Loaded;
            OnShow = new ShowDelegate(this.ShowText);
            OnHide = new HideDelegate(this.HideText);
            Showboard = this.Resources["showStoryBoard"] as Storyboard;
            Hideboard = this.Resources["hideStoryBoard"] as Storyboard;
        }

        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadingThread = new Thread(Load);
            loadingThread.Start();
        }

        private void Load(object? obj)
        {
            Thread.Sleep(1000);
            this.Dispatcher.Invoke(OnShow, "Welcome to Palletizing Systems.");
            Thread.Sleep(2000);
            this.Dispatcher.Invoke(OnHide);

            this.Dispatcher.Invoke(DispatcherPriority.Normal,
            (Action)delegate () { Close(); });
        }

        private void ShowText(string txt)
        {
            txtLoading.Text = txt;
            BeginStoryboard(Showboard);
        }
        private void HideText()
        {
            BeginStoryboard(Hideboard);
        }
    }
}
