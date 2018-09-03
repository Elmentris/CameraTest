using UIKit;
using Cirrious.FluentLayouts.Touch;

namespace CameraTest.iOS.Views
{
    partial class MainView
    {
        UIView liveCameraStream;
        UIButton captureButton;
        UIButton cancelButton;

        public override bool ShowActivityIndicator => true;

        protected override void DoViewDidLoad()
        {
            base.DoViewDidLoad();

            NavigationController.SetNavigationBarHidden(true, false);
        }

        protected override void InitView()
        {
            base.InitView();

            liveCameraStream = new UIView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            captureButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                Layer =
                {
                    CornerRadius = 6f,
                }
            };
            captureButton.SetTitle("Capture", UIControlState.Normal);
            captureButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            captureButton.TouchUpInside += CaptureButtonClick;

            cancelButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                Layer =
                {
                    CornerRadius = 6f,
                }
            };
            cancelButton.SetTitle("Cancel", UIControlState.Normal);
            cancelButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
        }

        protected override void ConstraintView()
        {
            base.ConstraintView();

            captureButton.SetContentCompressionResistancePriority(750f, UILayoutConstraintAxis.Horizontal);
            cancelButton.SetContentCompressionResistancePriority(750f, UILayoutConstraintAxis.Horizontal);

            this.View.AddSubviews(liveCameraStream, captureButton, cancelButton);
            this.View.AddConstraints(
                liveCameraStream.Top().EqualTo().TopOf(this.View),
                liveCameraStream.Leading().EqualTo().LeadingOf(this.View),
                liveCameraStream.Trailing().EqualTo().TrailingOf(this.View),
                liveCameraStream.Bottom().EqualTo().BottomOf(this.View),

                captureButton.Bottom().EqualTo(-20f).BottomOf(this.View),
                captureButton.Leading().EqualTo(20f).LeadingOf(this.View),
                captureButton.Trailing().EqualTo(-10f).LeadingOf(cancelButton),
                captureButton.Height().EqualTo(44f),

                cancelButton.Leading().EqualTo(10f).TrailingOf(captureButton),
                cancelButton.Trailing().EqualTo(-20f).TrailingOf(this.View),
                cancelButton.Height().EqualTo().HeightOf(captureButton),
                cancelButton.WithSameWidth(captureButton),
                cancelButton.Bottom().EqualTo().BottomOf(captureButton)
            );
        }
    }
}
