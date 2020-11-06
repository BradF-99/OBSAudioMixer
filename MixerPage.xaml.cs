using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OBSAudioMixer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MixerPage : Page
    {
        private OBSWebsocket _obs;
        private Dictionary<string,SourceClass> currentSources;

        public MixerPage()
        { 
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            OBSWebsocket _obs = new OBSWebsocket();
            Dictionary<string,SourceClass> currentSources = new Dictionary<string,SourceClass>();

            this._obs = _obs;
            this.currentSources = currentSources;

            _obs.Disconnected += _obs_Disconnected;
            _obs.SourceVolumeChanged += _obs_SourceVolumeChanged;

            AuthClass _auth = (AuthClass)e.Parameter;

            if (!_obs.IsConnected)
            {
                try
                {
                    _obs.Connect($"ws://{_auth.IP}:{_auth.Port}", _auth.Password);
                }
                catch (Exception ex)
                {
                    Frame.Navigate(typeof(MainPage), ex);
                    return;
                }
            }
            else
            {
                Frame.Navigate(typeof(MainPage), null);
                return;
            }

            SliderInit();
            MixerInit();
        }

        private void _obs_Disconnected(object sender, EventArgs e)
        {
            _obs = null;
            Frame.Navigate(typeof(MainPage));
        }

        private async void _obs_SourceVolumeChanged(OBSWebsocket sender, string sourceName, float volume)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                currentSources[sourceName].slider.Value = AudioConversionHelper.CubicdBtoDef(_obs.GetVolume(sourceName, true).Volume);
            });
        }

        private void SliderInit()
        {
            List<SceneItem> sceneItems = _obs.GetCurrentScene().Items;
            foreach (SceneItem item in sceneItems)
            {
                Slider _slider = new Slider();
                _slider.Name = item.SourceName;
                _slider.Orientation = Orientation.Vertical;
                _slider.Maximum = 1.0;
                _slider.Minimum = 0.0;
                _slider.StepFrequency = 0.001;
                _slider.IsThumbToolTipEnabled = false;
                _slider.Value = AudioConversionHelper.CubicdBtoDef(_obs.GetVolume(item.SourceName,true).Volume);
                _slider.ValueChanged += new RangeBaseValueChangedEventHandler((sender, e) => Slider_ValueChanged(sender, e, item.SourceName));
                SourceClass _source = new SourceClass(item, _slider);
                this.currentSources.Add(item.SourceName,_source);
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e, string sourceName)
        {
            Slider slider = sender as Slider;
            _obs.SetVolume(sourceName, AudioConversionHelper.CubicDefTodB((float)slider.Value), true);
        }

        
        private void MixerInit()
        {
            foreach (SourceClass source in this.currentSources.Values)
            {
                Grid _grid = new Grid();
                _grid.Background = ColourHelper.RandomColour();
                
                _grid.Height = spMixer.Height;
                _grid.Width = 150;
                _grid.Margin = new Thickness(10,10,10,10);

                ColumnDefinition _col = new ColumnDefinition();
                _grid.ColumnDefinitions.Add(_col);

                RowDefinition _row0 = new RowDefinition();
                _row0.Height = new GridLength(1,GridUnitType.Auto);
                _grid.RowDefinitions.Add(_row0);

                Rectangle _rect = new Rectangle();
                _rect.Fill = new SolidColorBrush(Color.FromArgb(255,73,73,73));
                _grid.Children.Add(_rect);
                Grid.SetRow(_rect, 0);

                TextBlock _tbSourceName = new TextBlock();
                _tbSourceName.Text = source.sceneItem.SourceName;
                _tbSourceName.Width = Double.NaN;
                _tbSourceName.HorizontalAlignment = HorizontalAlignment.Center;
                _grid.Children.Add(_tbSourceName);
                Grid.SetRow(_tbSourceName, 0);

                RowDefinition _row1 = new RowDefinition();
                _row1.Height = new GridLength(2, GridUnitType.Star);
                _grid.RowDefinitions.Add(_row1);

                Slider slider = source.slider;
                _grid.Children.Add(slider);
                Grid.SetRow(slider, 1);

                spMixer.Children.Add(_grid);
            }
        }
    }
}
