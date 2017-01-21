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
            
            try
            {
                var position = locator.GetPositionAsync(10000).Result;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromKilometers(5)));
            }
            catch (Exception ex)
            {
                var b = 5;
            }
        }

        private async Task RequestPermissions()
        {
            await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { Permission.Location });
        }
    }
}
