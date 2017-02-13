using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;
using Find.Me.Xamarin.Droid.Common;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using static Android.Resource;
using Android.Support.Design.Widget;

namespace Find.Me.Xamarin.Droid
{
    [Activity(Label = "Find.Me.Xamarin.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const int RequestLocationPermissionId = 1;
        private readonly string[] PermissionsLocation =
        {
          Manifest.Permission.AccessCoarseLocation,
          Manifest.Permission.AccessFineLocation
        };

        const int RequestLocationId = 0;

        private LocationManager locationManager;

        private GoogleMap map;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                RequestLocationPermission();
            }
        }

        private void RequestLocationPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                //Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                Snackbar.Make(this.FindViewById(Resource.Id.map), Resource.String.location_permission_message,
                    Snackbar.LengthIndefinite).SetAction("ok", new Action<View>(delegate (View obj)
                    {
                        ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestLocationPermissionId);
                    })).Show();
            }
            else
            {
                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(this, PermissionsLocation, RequestLocationPermissionId);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            var mapFrag = FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;
            var mapReady = new OnMapReady();

            mapReady.MapReadyAction += OnMapReady;

            mapFrag.GetMapAsync(mapReady);
        }

        private void OnMapReady(GoogleMap newMap)
        {
            this.map = newMap;

            locationManager = GetSystemService(Context.LocationService) as LocationManager;

            var provider = LocationManager.GpsProvider;

            if (locationManager.IsProviderEnabled(provider))
            {
                var location = locationManager.GetLastKnownLocation(provider);
                var cameraUpdate = CameraUpdateFactory.NewLatLng(new LatLng(location.Latitude, location.Longitude));
                this.map.MoveCamera(cameraUpdate);
            }
        }
    }
}


