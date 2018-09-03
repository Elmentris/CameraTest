using System;
using Android.App;
using Android.Util;

namespace CameraTest.Droid.Helpers
{
	public static class PixelsConverter
	{
		public static int DpToPx(double dp)
		{
			DisplayMetrics displayMetrics = Application.Context.Resources.DisplayMetrics;
			double result = Math.Round(dp * (displayMetrics.Xdpi / (double)Android.Util.DisplayMetricsDensity.Default));
			return (int)result;
		}
	}
}