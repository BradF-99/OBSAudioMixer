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
        private OBSWebsocket _obs;

        public MainPage()
        {
            this.InitializeComponent();
            _obs = new OBSWebsocket();

            _obs.Connected += OnConnect;
        }

        private void btnLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tbIP.IsEnabled = false;
            tbPort.IsEnabled = false;
            tbPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;

            if (!_obs.IsConnected)
            {
                try
                {
                    _obs.Connect($"ws://{tbIP.Text}:{tbPort.Text}", tbPassword.Text);
                }
                catch (Exception ex)
                {
                    if (ex is AuthFailureException)
                    {
                        FlyoutBase.ShowAttachedFlyout(tbPassword);
                    }
                    else
                    {
                        FlyoutBase.ShowAttachedFlyout(tbIP);
                    }
                }
            }

            _obs.Disconnect();

            tbIP.IsEnabled = true;
            tbPort.IsEnabled = true;
            tbPassword.IsEnabled = true;
            btnLogin.IsEnabled = true;
        }

        private void OnConnect(object sender, EventArgs e)
        {
            Frame.Navigate(typeof(MixerPage), _obs);
        }
    }
}
