using Android.OS;
using Android.Support.Constraints;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using CameraTest.Core.ViewModels.Abstract;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;

namespace CameraTest.Droid.Views.Abstract
{
    public abstract class BaseScrollToolbarActivity<TViewModel> : BaseActivity<TViewModel> where TViewModel : BaseViewModel, IMvxViewModel
	{
		protected Toolbar _toolbar;

        protected ConstraintLayout _constraintLayoutRoot;

		protected abstract int LayoutId { get; }
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

	    public override bool OnCreateOptionsMenu(IMenu menu)
	    {
	        if (HasOptionMenu)
	            MenuInflater.Inflate(MenuId, menu);

	        return base.OnCreateOptionsMenu(menu);
	    }

        protected void SetToolbarTitle(string title)
		{
			_toolbar.Title = title;
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			CreateToolbar();
			SetSupportActionBar(_toolbar);

			SupportActionBar.SetDisplayShowHomeEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
		    SupportActionBar.Title = ViewModel.Title;

            InitLayout();
		}

		private void InitLayout()
		{
            _constraintLayoutRoot = this.BindingInflate(LayoutId, null) as ConstraintLayout;

			ConstraintLayout.LayoutParams layoutParams = new ConstraintLayout.LayoutParams(ConstraintLayout.LayoutParams.MatchConstraint, ConstraintLayout.LayoutParams.MatchConstraint);
			_constraintLayoutRoot.LayoutParameters = layoutParams;

			BaseMainConstraintLayout.RemoveView(BaseScrollView);
			BaseScrollView.RemoveView(BaseElementsConstraintLayout);
			BaseMainConstraintLayout.AddView(BaseElementsConstraintLayout);

            BaseMainConstraintLayout.BringChildToFront(BasePreLoaderConstraintLayout);

			var rootElementSet = new ConstraintSet();
			rootElementSet.Clone(BaseMainConstraintLayout);
			rootElementSet.Connect(BaseElementsConstraintLayout.Id, ConstraintSet.Top, BaseMainConstraintLayout.Id, ConstraintSet.Top);
			rootElementSet.Connect(BaseElementsConstraintLayout.Id, ConstraintSet.Bottom, BaseMainConstraintLayout.Id, ConstraintSet.Bottom);
			rootElementSet.Connect(BaseElementsConstraintLayout.Id, ConstraintSet.Left, BaseMainConstraintLayout.Id, ConstraintSet.Left);
			rootElementSet.Connect(BaseElementsConstraintLayout.Id, ConstraintSet.Right, BaseMainConstraintLayout.Id, ConstraintSet.Right);
			rootElementSet.ApplyTo(BaseMainConstraintLayout);

			BaseElementsConstraintLayout.AddView(_constraintLayoutRoot);

			ConstraintSet elementsSet = new ConstraintSet();
			elementsSet.Clone(BaseElementsConstraintLayout);

			elementsSet.Connect(_constraintLayoutRoot.Id, ConstraintSet.Top, _toolbar.Id, ConstraintSet.Bottom);
			elementsSet.Connect(_constraintLayoutRoot.Id, ConstraintSet.Bottom, BaseElementsConstraintLayout.Id, ConstraintSet.Bottom);
			elementsSet.Connect(_constraintLayoutRoot.Id, ConstraintSet.Left, BaseElementsConstraintLayout.Id, ConstraintSet.Left);
			elementsSet.Connect(_constraintLayoutRoot.Id, ConstraintSet.Right, BaseElementsConstraintLayout.Id, ConstraintSet.Right);

			elementsSet.ApplyTo(BaseElementsConstraintLayout);
		}

		private void CreateToolbar()
		{
			_toolbar = new Toolbar(this);
			_toolbar.Id = Resource.Id.toolbar;
			ConstraintLayout.LayoutParams clptoolbar = new ConstraintLayout.LayoutParams(
				ConstraintLayout.LayoutParams.MatchConstraint, ConstraintLayout.LayoutParams.WrapContent);
			_toolbar.LayoutParameters = clptoolbar;
			_toolbar.TextAlignment = TextAlignment.Center;
            //TODO: COLORS
			_toolbar.SetTitleTextColor(ContextCompat.GetColor(this, Android.Resource.Color.White));
            //_toolbar.SetBackgroundColor(Color.ParseColor(Resources.GetString(Resource.Color.gradient_start)));
			_toolbar.SetSubtitleTextAppearance(this, Resource.Style.TextAppearance_Widget_AppCompat_Toolbar_Subtitle);

			BaseElementsConstraintLayout.AddView(_toolbar);

			CreateMainConstraintSet();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					Finish();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		private void CreateMainConstraintSet()
		{
			var constraintSet = new ConstraintSet();
			constraintSet.Clone(BaseElementsConstraintLayout);

			constraintSet.Connect(_toolbar.Id, ConstraintSet.Top, BaseElementsConstraintLayout.Id, ConstraintSet.Top);
			constraintSet.Connect(_toolbar.Id, ConstraintSet.Left, BaseElementsConstraintLayout.Id, ConstraintSet.Left);
			constraintSet.Connect(_toolbar.Id, ConstraintSet.Right, BaseElementsConstraintLayout.Id, ConstraintSet.Right);

			constraintSet.ApplyTo(BaseElementsConstraintLayout);
		}
	}
}