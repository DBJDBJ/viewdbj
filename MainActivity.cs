

namespace dbjorgview
{
	using System;
	using System.Threading ;
	using Android.App;
	using Android.Content;
	using Android.Runtime;
	using Android.Views;
	using Android.Widget;
	using Android.OS;
	using Android.Webkit;

	[Activity (Label = "dbjorgview", Theme = "@android:style/Theme.NoTitleBar")]
	public class MainActivity : Activity
	{
		WebView web_view;
		/*
		 * vibrating as propery set
		*/
		static Vibrator vibro_ = null ;
		public static Vibrator vibro ( long value = 0 ) {
			if (null == vibro_ ) {
				vibro_ = 
					Android.OS.Vibrator.FromContext ( Android.App.Application.Context );
				} ;
			if (value > 0) vibro_ .Vibrate ( (long)value ); 
			return vibro_;
		} 


		string home_url { 
			get { return GetString(Resource.String.home_url ); }
			set { }
		}


		protected override void OnCreate (Bundle bundle)
		{
			try {
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			web_view = FindViewById<WebView> (Resource.Id.webview);
			web_view.Settings.JavaScriptEnabled = true;
			web_view.LoadUrl ( this.home_url);

			web_view.SetWebViewClient (new dbjorgviewClient ());
			} catch ( SystemException x ) {
				Toast.MakeText ( this, x.Message, ToastLength.Long).Show() ;
				#if DEBUG
				Android.Util.Log.Error (GetString (Resource.String.app_name), x.StackTrace);
				#endif
			}
		}

		public override bool OnKeyDown (Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
		{
			if (keyCode == Keycode.Back && web_view.CanGoBack ()) {
				web_view.GoBack ();
				return true;
			}

			return base.OnKeyDown (keyCode, e);
		}

		private class dbjorgviewClient : WebViewClient
		{
			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				view.LoadUrl (url);
				return true;
			}

			public override void OnPageFinished (WebView view, string url)
			{
				try {
				base.OnPageFinished (view, url);
					SplashActivity.ON = false ;
			} catch ( SystemException x ) {
				#if DEBUG
					Android.Util.Log.Error ("dbj view web view client", x.StackTrace);
				#endif
			}
			}

		}
	}
	/*
	 * splash screen (as an activity)
	*/
	[Activity(Theme = "@style/Theme.Splash",  MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		static bool on_ = false ;
		public static bool ON {
			get { return on_ ; }
			set { 
				if ( on_ == false && value == true ) MainActivity.vibro (1000);
				on_ = value; 
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SplashActivity.ON = true;
			StartActivity(typeof(MainActivity));
		}
	}
}


