using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Find.Me
{
    public partial class MainMapPage : ContentPage
    {
        private IGeolocator locator;

        public MainMapPage()
        {
            RequestPermissions().Wait();

            InitializeComponent();

            this.locator = CrossGeolocator.Current;

            this.locator.PositionChanged += Locator_PositionChanged;

            this.SetInitialPosition();

            this.locator.StartListeningAsync(1000, 10);
        }

        private async Task SetInitialPosition()
        {
            var position = await locator.GetPositionAsync(10000);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromKilometers(5)));
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            try
            {
                var position = e.Position;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), map.VisibleRegion.Radius));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private async Task RequestPermissions()
        {
            await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Location });
        }
    }
}
