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
using Windows.UI.Core;
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
        private MapIcon centerElement;
        private Geolocator locator;

        public MainMapPage()
        {
            this.InitializeComponent();
            this.locator = new Geolocator();
        }

        private async void mainMap_Loaded(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Unspecified:
                    break;
                case GeolocationAccessStatus.Allowed:
                    var position = await locator.GetGeopositionAsync();

                    var center = new Geopoint(new BasicGeoposition { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude });

                    this.centerElement = new MapIcon();
                    centerElement.Location = center;
                    centerElement.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    centerElement.ZIndex = 0;
                    centerElement.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                    centerElement.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapCircle.png"));

                    // Add the MapIcon to the map.
                    this.mainMap.MapElements.Add(centerElement);

                    this.mainMap.Center = center;
                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                default:
                    break;
            }

            locator.PositionChanged += Locator_PositionChanged;
        }

        private async void Locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var position = await sender.GetGeopositionAsync();

            var center = new Geopoint(new BasicGeoposition { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude });

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                    {
                        this.centerElement.Location = center;
                    
                        this.mainMap.Center = center;
                    }
                );
        }

        private void mainMap_CenterChanged(MapControl sender, object args)
        {

        }
    }
}
