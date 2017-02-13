using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;

namespace Find.Me.Xamarin.Droid.Common
{
    public class OnMapReady : Java.Lang.Object, IOnMapReadyCallback
    {
        public GoogleMap Map { get; private set; }

        public event Action<GoogleMap> MapReadyAction;

        void IOnMapReadyCallback.OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;

            MapReadyAction?.Invoke(Map);
        }
    }
}