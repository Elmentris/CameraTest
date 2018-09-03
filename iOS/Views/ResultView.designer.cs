using UIKit;
using Cirrious.FluentLayouts.Touch;
namespace CameraTest.iOS.Views
{
    partial class ResultView
    {
        UIView whiteBottomView;
        UIImageView carImage;
        UILabel makeLabel;
        UILabel modelLabel;
        UILabel probabilityLabel;
        UIButton doneButton;

        protected override void DoViewDidLoad()
        {
            base.DoViewDidLoad();
        }

        protected override void InitView()
        {
            base.InitView();

            carImage = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                ClipsToBounds = true,
                ContentMode = UIViewContentMode.ScaleAspectFill
            };

            whiteBottomView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White
            };

            makeLabel = new UILabel
            {
                Font = UIFont.SystemFontOfSize(15f, UIFontWeight.Regular),
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Left,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            modelLabel = new UILabel
            {
                Font = UIFont.SystemFontOfSize(15f, UIFontWeight.Regular),
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Left,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            probabilityLabel = new UILabel
            {
                Font = UIFont.SystemFontOfSize(15f, UIFontWeight.Regular),
                TextColor = UIColor.Black,
                TextAlignment = UITextAlignment.Left,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            doneButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Layer =
                {
                    BorderWidth = 1f,
                    BorderColor = UIColor.Black.CGColor,
                    CornerRadius = 6f
                }
            };
            doneButton.SetTitle("Done", UIControlState.Normal);
            doneButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
        }

        protected override void ConstraintView()
        {
            base.ConstraintView();

            whiteBottomView.AddSubviews(makeLabel, modelLabel,
                                        probabilityLabel, doneButton);
            whiteBottomView.AddConstraints(
                doneButton.Bottom().EqualTo(-20f).BottomOf(whiteBottomView),
                doneButton.WithSameCenterX(whiteBottomView),
                doneButton.Height().EqualTo(44f),
                doneButton.Width().GreaterThanOrEqualTo(100f),

                probabilityLabel.Bottom().EqualTo(-10f).TopOf(doneButton),
                probabilityLabel.Leading().EqualTo(15f).LeadingOf(whiteBottomView),
                probabilityLabel.Trailing().EqualTo(-15f).TrailingOf(whiteBottomView),

                modelLabel.Bottom().EqualTo(-10f).TopOf(probabilityLabel),
                modelLabel.Leading().EqualTo(15f).LeadingOf(whiteBottomView),
                modelLabel.Trailing().EqualTo(-15f).TrailingOf(whiteBottomView),

                makeLabel.Bottom().EqualTo(-10f).TopOf(modelLabel),
                makeLabel.Leading().EqualTo(15f).LeadingOf(whiteBottomView),
                makeLabel.Trailing().EqualTo(-15f).TrailingOf(whiteBottomView),
                makeLabel.Top().EqualTo(20f).TopOf(whiteBottomView)
            );

            carImage.SetContentCompressionResistancePriority(20f, UILayoutConstraintAxis.Vertical);
            whiteBottomView.SetContentCompressionResistancePriority(750f, UILayoutConstraintAxis.Vertical);

            View.AddSubviews(carImage, whiteBottomView);
            View.AddConstraints(
                carImage.Top().EqualTo().TopOf(View),
                carImage.Leading().EqualTo().LeadingOf(View),
                carImage.Trailing().EqualTo().TrailingOf(View),
                carImage.Bottom().EqualTo().BottomOf(View),

                whiteBottomView.Bottom().EqualTo().BottomOf(View),
                whiteBottomView.Leading().EqualTo().LeadingOf(View),
                whiteBottomView.Trailing().EqualTo().TrailingOf(View)
            );
        }
    }
}
