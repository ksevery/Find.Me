using Find.Me.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Find.Me.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMapPage : Page
    {
        public MainMapPage()
        {
            this.InitializeComponent();
        }

        private async void mainMap_Loaded(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Unspecified:
                    break;
                case GeolocationAccessStatus.Allowed:
                    var locator = new Geolocator();
                    var position = await locator.GetGeopositionAsync();

                    var center = new Geopoint(new BasicGeoposition { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude });

                    MapIcon centerIcon = new MapIcon();
                    centerIcon.Location = center;
                    centerIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    centerIcon.ZIndex = 0;
                    centerIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                    centerIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapCircle.png"));

                    // Add the MapIcon to the map.
                    this.mainMap.MapElements.Add(centerIcon);
                    

                    this.mainMap.Center = center;
                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                default:
                    break;
            }
        }
    }
}
