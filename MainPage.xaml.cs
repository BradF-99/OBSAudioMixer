using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace OBSAudioMixer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is Exception)
            {
                if (e.Parameter is AuthFailureException)
                {
                    FlyoutBase.ShowAttachedFlyout(tbPassword);
                }
                else
                {
                    FlyoutBase.ShowAttachedFlyout(tbIP);
                }
            }

            tbIP.IsEnabled = true;
            tbPort.IsEnabled = true;
            tbPassword.IsEnabled = true;
            btnLogin.IsEnabled = true;
        }

        private void btnLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tbIP.IsEnabled = false;
            tbPort.IsEnabled = false;
            tbPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;

            AuthClass _auth = new AuthClass(tbIP.Text, tbPort.Text, tbPassword.Text);
            Frame.Navigate(typeof(MixerPage), _auth);
        }
    }
}
