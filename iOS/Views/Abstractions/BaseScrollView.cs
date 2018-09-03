using CameraTest.Core.ViewModels.Abstract;
using Cirrious.FluentLayouts.Touch;
using UIKit;

namespace CameraTest.iOS.Views.Abstractions
{
    public abstract class BaseScrollView<TViewModel> : BaseView<TViewModel> where TViewModel : BaseViewModel
    {
        protected UIView ContentView { get; private set; }
        protected UIScrollView ScrollView { get; private set; }

        protected override void DoViewDidLoad()
        {
            base.DoViewDidLoad();
            InitScrollView();
            InitContentViewConstraints();
        }

        void InitScrollView()
        {
            ScrollView = new UIScrollView() { TranslatesAutoresizingMaskIntoConstraints = false, Bounces = false };
            ContentView = new UIView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear,
            };

            View.AddSubview(ScrollView);
            View.AddConstraints(
                ScrollView.AtLeftOf(View),
                ScrollView.AtRightOf(View),
                ScrollView.AtTopOf(View),
                ScrollView.AtBottomOf(View)
                );
            ScrollView.AddSubview(ContentView);
        }

        public virtual void InitContentViewConstraints()
        {
            ScrollView.AddConstraints(
                ContentView.AtTopOf(ScrollView),
                ContentView.AtBottomOf(ScrollView),
                ContentView.WithSameWidth(ScrollView),
                ContentView.WithSameCenterX(ScrollView)
                );
        }
    }

}
