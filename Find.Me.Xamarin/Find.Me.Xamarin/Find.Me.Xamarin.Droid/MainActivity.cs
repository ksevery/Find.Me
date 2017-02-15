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
using Android.Gms.Location;
using Find.Me.Xamarin.Droid.Common;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;

using Android.Support.Design.Widget;
using Android.Gms.Common;
using Android.Gms.Common.Apis;

using static Android.Resource;
using static Android.Gms.Common.Apis.GoogleApiClient;
using System.Threading.Tasks;

namespace Find.Me.Xamarin.Droid
{
    [Activity(Label = "Find.Me.Xamarin.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IConnectionCallbacks, IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {
        private const int RequestLocationPermissionId = 1;
        private readonly string[] PermissionsLocation =
        {
          Manifest.Permission.AccessCoarseLocation,
          Manifest.Permission.AccessFineLocation
        };

        const int RequestLocationId = 0;

        private GoogleMap map;

        private GoogleApiClient apiClient;

        private LocationRequest locRequest;

        private Location lastKnownLocation;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                RequestLocationPermission();
            }

            apiClient = new Builder(this, this, this)
                .AddApi(LocationServices.API)
                .Build();

            apiClient.Connect();
            locRequest = new LocationRequest();
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
        }

        private void OnMapReady(GoogleMap newMap)
        {
            this.map = newMap;

            if (apiClient.IsConnected)
            {
                var location = LocationServices.FusedLocationApi.GetLastLocation(this.apiClient);
                if (location != null)
                {
                    this.lastKnownLocation = location;
                    this.map.MyLocationEnabled = true;
                    this.map.UiSettings.ZoomControlsEnabled = true;
                    this.map.UiSettings.ZoomGesturesEnabled = true;
                    var cameraUpdate = CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 20);
                    this.map.MoveCamera(cameraUpdate);
                }
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            var mapFrag = FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;
            var mapReady = new OnMapReady();

            mapReady.MapReadyAction += OnMapReady;

            mapFrag.GetMapAsync(mapReady);

            locRequest.SetPriority(100);
            locRequest.SetFastestInterval(100);
            locRequest.SetInterval(500);
            
            LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
            
        }

        public void OnConnectionSuspended(int cause)
        {
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
        }

        public void OnLocationChanged(Location location)
        {
            if (this.lastKnownLocation.Longitude != location.Longitude || this.lastKnownLocation.Latitude != location.Latitude)
            {
                var cameraUpdate = CameraUpdateFactory.NewLatLng(new LatLng(location.Latitude, location.Longitude));
                this.map.AnimateCamera(cameraUpdate);

                this.lastKnownLocation = location;
            }
        }
    }
}


