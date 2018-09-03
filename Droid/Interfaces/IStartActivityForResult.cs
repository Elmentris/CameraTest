using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CameraTest.Droid.Interfaces
{
	public interface IStartActivityForResult
	{
		Task<ActivityResult> StartActivityForResultAsync(Intent intent);
	}

	public class ActivityResult
	{
		public Result Result { get; set; }
		public Intent Intent { get; set; }
	}
}