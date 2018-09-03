using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Constraints;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using CameraTest.Core.ViewModels.Abstract;
using CameraTest.Droid.Helpers;
using CameraTest.Droid.Interfaces;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.ViewModels;
using Plugin.Permissions;

namespace CameraTest.Droid.Views.Abstract
{
	[Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public abstract class BaseActivity<TViewModel> : MvxAppCompatActivity<TViewModel>, IStartActivityForResult where TViewModel : BaseViewModel, IMvxViewModel
	{
		readonly Dictionary<int, TaskCompletionSource<ActivityResult>> _activityResultRegistrations = new Dictionary<int, TaskCompletionSource<ActivityResult>>();
		int _activityResultRegistrationCounter = 10000;
		DisplayMetrics _displayMetrics;
		public ConstraintLayout BaseMainConstraintLayout;
		public ConstraintLayout BaseElementsConstraintLayout;
		public ConstraintLayout BasePreLoaderConstraintLayout;
		public ProgressBar BaseProgressBar;
		public ScrollView BaseScrollView;
		private readonly List<EditText> _checkedEditTexts = new List<EditText>();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetDisplayOptions();
			AddBaseViews();
			CreateBaseActivityBinding();

            //StatusBarColor = Color.ParseColor(Resources.GetString(Resource.Color.gradient_start));
		}
		protected void CreateBaseActivityBinding()
		{
			var set = this.CreateBindingSet<BaseActivity<TViewModel>, TViewModel>();
			set.Apply();
		}

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel?.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
			ViewModel?.OnPause();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ViewModel?.OnDestroy();

			foreach (var editText in _checkedEditTexts)
			{
				editText.TextChanged -= TextChanged;
			}
		}

		protected void SetEditTextForCheckCursor(EditText editText)
		{
			_checkedEditTexts.Add(editText);
			editText.TextChanged += TextChanged;
		}

		private void TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			var editText = (EditText)sender;
			editText?.SetSelection(editText.Length());
		}

		public Color StatusBarColor
		{
			set
			{
				if (value == null)
					return;

				if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
				{
					Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
					Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
					Window.SetStatusBarColor(value);
				}
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public Task<ActivityResult> StartActivityForResultAsync(Intent intent)
		{
			int requestCode = _activityResultRegistrationCounter++;
			var completionSource = new TaskCompletionSource<ActivityResult>();
			this._activityResultRegistrations[requestCode] = completionSource;
			this.StartActivityForResult(intent, requestCode);
			return completionSource.Task;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			TaskCompletionSource<ActivityResult> completionSource;
			if (this._activityResultRegistrations.TryGetValue(requestCode, out completionSource))
			{
				this._activityResultRegistrations.Remove(requestCode);
				completionSource.TrySetResult(new ActivityResult { Result = resultCode, Intent = data });
			}
		}

		public virtual void HideKeyboard()
		{
			var imm = (InputMethodManager)GetSystemService(InputMethodService);

			if (imm.IsAcceptingText && CurrentFocus?.WindowToken != null)
				imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
		}

		private void CreateMainConstraintSet()
		{
			var constraintSet = new ConstraintSet();
			constraintSet.Clone(BaseMainConstraintLayout);

			constraintSet.ApplyTo(BaseMainConstraintLayout);
		}

		private void CreateBasePreLoaderConstraintSet()
		{
			ConstraintSet BasePreLoaderConstraintSet = new ConstraintSet();
			BasePreLoaderConstraintSet.Clone(BasePreLoaderConstraintLayout);

			BasePreLoaderConstraintSet.Connect(BaseProgressBar.Id, ConstraintSet.Top, BasePreLoaderConstraintLayout.Id, ConstraintSet.Top, PixelsConverter.DpToPx(300));
			BasePreLoaderConstraintSet.Connect(BaseProgressBar.Id, ConstraintSet.Left, BasePreLoaderConstraintLayout.Id, ConstraintSet.Left, PixelsConverter.DpToPx(23));
			BasePreLoaderConstraintSet.Connect(BaseProgressBar.Id, ConstraintSet.Right, BasePreLoaderConstraintLayout.Id, ConstraintSet.Right, PixelsConverter.DpToPx(23));

			BasePreLoaderConstraintSet.ApplyTo(BasePreLoaderConstraintLayout);
		}

		private void AddBaseViews()
		{
			CreateBaseConstraints();
			CreateProgressBar();

			BaseMainConstraintLayout.AddView(BaseScrollView);
			BaseScrollView.AddView(BaseElementsConstraintLayout);
			BaseMainConstraintLayout.AddView(BasePreLoaderConstraintLayout);
			BasePreLoaderConstraintLayout.AddView(BaseProgressBar);

			CreateBasePreLoaderConstraintSet();
			CreateMainConstraintSet();
		}

		private void CreateProgressBar()
		{
			BaseProgressBar = new ProgressBar(this);
			BaseProgressBar.Indeterminate = true;
			BaseProgressBar.Id = Resource.Id.BaseProgressBar;
			BaseProgressBar.IndeterminateDrawable.SetColorFilter(Color.ParseColor("#ffffff"), PorterDuff.Mode.SrcIn);

			var set = this.CreateBindingSet<BaseActivity<TViewModel>, BaseViewModel>();
			set.Bind(this).For("ShowIndicator").To(vm => vm.IsBusy);
			set.Apply();
		}

		bool _showIndicator;
		public bool ShowIndicator
		{
			get { return _showIndicator; }
			set
			{
				_showIndicator = value;

				if (BasePreLoaderConstraintLayout != null)
				{
					BasePreLoaderConstraintLayout.Clickable = value;
					BasePreLoaderConstraintLayout.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
				}
			}
		}

		public void SetError(EditText editText)
		{
			GradientDrawable gd = new GradientDrawable();
			gd.SetColor(Color.ParseColor("#fae5ea"));
			gd.SetCornerRadius(5);
			gd.SetStroke(2, Color.Red);
			editText.SetBackgroundDrawable(gd);
		}

		public void SetCorrect(EditText editText)
		{
			GradientDrawable gd = new GradientDrawable();
			gd.SetColor(Color.White);
			gd.SetCornerRadius(0);
			gd.SetStroke(0, Color.White);
			editText.SetBackgroundDrawable(gd);
		}

		private void CreateBaseConstraints()
		{
			BaseMainConstraintLayout = new ConstraintLayout(this);
			BaseMainConstraintLayout.Id = Resource.Id.BaseMainConstraintLayout;

			SetContentView(BaseMainConstraintLayout);

			BaseScrollView = new ScrollView(this);
			BaseScrollView.Id = Resource.Id.BaseScrollView;
			BaseScrollView.VerticalScrollBarEnabled = false;
			ConstraintLayout.LayoutParams cplScrollView = new ConstraintLayout.LayoutParams(
				ConstraintLayout.LayoutParams.MatchConstraint, ConstraintLayout.LayoutParams.MatchConstraint);
			BaseScrollView.LayoutParameters = cplScrollView;

			BaseElementsConstraintLayout = new ConstraintLayout(this);
			BaseElementsConstraintLayout.Id = Resource.Id.BaseElementsConstraintLayout;
			ConstraintLayout.LayoutParams cplBECL = new ConstraintLayout.LayoutParams(
				ConstraintLayout.LayoutParams.MatchParent, ConstraintLayout.LayoutParams.MatchParent);
			BaseElementsConstraintLayout.LayoutParameters = cplBECL;
			BaseElementsConstraintLayout.DescendantFocusability = DescendantFocusability.BeforeDescendants;
			BaseElementsConstraintLayout.FocusableInTouchMode = true;

			BasePreLoaderConstraintLayout = new ConstraintLayout(this);
			BasePreLoaderConstraintLayout.Id = Resource.Id.BasePreLoaderConstraintLayout;
			ConstraintLayout.LayoutParams cplBPCL = new ConstraintLayout.LayoutParams(
				ConstraintLayout.LayoutParams.MatchParent, ConstraintLayout.LayoutParams.MatchParent);
			BasePreLoaderConstraintLayout.LayoutParameters = cplBPCL;
			BasePreLoaderConstraintLayout.SetBackgroundColor(Color.ParseColor("#5F555555"));
			BasePreLoaderConstraintLayout.Visibility = ViewStates.Invisible;
		}

		void SetDisplayOptions()
		{
			var display = WindowManager.DefaultDisplay;
			_displayMetrics = new DisplayMetrics();
			display?.GetMetrics(_displayMetrics);
		}

		public int WidthPixels
		{
			get
			{
				return _displayMetrics.WidthPixels;
			}
		}

		public int HeightPixels
		{
			get
			{
				return _displayMetrics.HeightPixels;
			}
		}
	}
}