using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OBSAudioMixer
{
    class SourceClass
    {
        public SourceClass(SceneItem sceneItem, Slider slider)
        {
            this.sceneItem = sceneItem;
            this.slider = slider;
        }

        public SceneItem sceneItem { get; set; }
        public Slider slider { get; set; }
    }
}
