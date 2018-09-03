using Android.Graphics;
using Android.OS;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using CameraTest.Core.ViewModels.Abstract;
using MvvmCross.ViewModels;

namespace CameraTest.Droid.Views.Abstract
{
    public abstract class BaseToolbarActivity<TViewModel> : BaseActivity<TViewModel> where TViewModel : BaseViewModel, IMvxViewModel
	{
		protected Toolbar _toolbar;

		protected virtual int MenuId
		{
			get { return -1; }
		}

		protected bool HasOptionMenu
		{
			get
			{
				return MenuId != -1;
			}
		}

		protected void SetToolbarTitle(string title)
		{
			_toolbar.Title = title;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			if (HasOptionMenu)
				MenuInflater.Inflate(MenuId, menu);

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			CreateToolbar();
			SetSupportActionBar(_toolbar);

			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

		}

		void CreateToolbar()
		{
			_toolbar = new Toolbar(this);
			_toolbar.Id = Resource.Id.toolbar;
			ConstraintLayout.LayoutParams clptoolbar = new ConstraintLayout.LayoutParams(
				ConstraintLayout.LayoutParams.MatchParent, ConstraintLayout.LayoutParams.WrapContent);
			_toolbar.LayoutParameters = clptoolbar;
			//_toolbar.Title = "OCBB";
			_toolbar.SetTitleTextColor(Color.White);
			_toolbar.SetBackgroundColor(Color.ParseColor("#00bbcc"));
			_toolbar.SetSubtitleTextColor(Color.ParseColor("#ffffff"));
			_toolbar.SetSubtitleTextAppearance(this, Resource.Style.TextAppearance_Widget_AppCompat_Toolbar_Subtitle);

			BaseScrollView.RemoveView(BaseElementsConstraintLayout);
			BaseMainConstraintLayout.RemoveView(BaseScrollView);
			BaseMainConstraintLayout.RemoveView(BasePreLoaderConstraintLayout);
			BasePreLoaderConstraintLayout.RemoveView(BaseProgressBar);

			BaseElementsConstraintLayout.AddView(_toolbar);
			BaseMainConstraintLayout.AddView(BaseElementsConstraintLayout);

			BaseMainConstraintLayout.AddView(BasePreLoaderConstraintLayout);
			BasePreLoaderConstraintLayout.AddView(BaseProgressBar);
			CreateMainConstraintSet();
		}

		private void CreateMainConstraintSet()
		{
			var constraintSet = new ConstraintSet();
			constraintSet.Clone(BaseMainConstraintLayout);

			constraintSet.Connect(_toolbar.Id, ConstraintSet.Top, BaseElementsConstraintLayout.Id, ConstraintSet.Top);
			constraintSet.Connect(_toolbar.Id, ConstraintSet.Left, BaseElementsConstraintLayout.Id, ConstraintSet.Left);
			constraintSet.Connect(_toolbar.Id, ConstraintSet.Right, BaseElementsConstraintLayout.Id, ConstraintSet.Right);

			constraintSet.ApplyTo(BaseMainConstraintLayout);
		}
	}
}