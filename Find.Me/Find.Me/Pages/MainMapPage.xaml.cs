using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
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

            //this.locator.PositionChanged += Locator_PositionChanged;

            this.SetInitialPosition();
        }

        private async Task SetInitialPosition()
        {
            var position = await locator.GetPositionAsync(10000);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromKilometers(5)));
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            var position = e.Position;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromKilometers(5)));
        }

        private async Task RequestPermissions()
        {
            await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Location });
        }
    }
}
