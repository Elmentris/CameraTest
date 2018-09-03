using System;

using UIKit;
using CameraTest.iOS.Views.Abstractions;
using CameraTest.Core;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Binding.BindingContext;
using CameraTest.Core.Units.DetectModel;
using Foundation;
using CoreGraphics;

namespace CameraTest.iOS.Views
{
    [MvxChildPresentation]
    public partial class ResultView : BaseView<ResultViewModel>
    {
        protected override void DoBind()
        {
            base.DoBind();

            var set = this.CreateBindingSet<ResultView, ResultViewModel>();
            set.Bind(this).For(v => v.DetectedModel).To(vm => vm.DetectedModel);
            set.Bind(doneButton).To(vm => vm.CloseCommand);
            set.Apply();
        }

        DetectModelParameter detectedModel;
        public DetectModelParameter DetectedModel
        {
            get => detectedModel;
            set
            {
                detectedModel = value;
                SetInfo();
            }
        }

        void SetInfo()
        {
            if (DetectedModel == null)
                return;

            carImage.LayoutIfNeeded();
            carImage.Image = UIImage.LoadFromData(NSData.FromArray(DetectedModel.Bytes));
            probabilityLabel.Text = $"Probability: {DetectedModel.Probability}";
            modelLabel.Text = $"Model: {DetectedModel.ModelName}";
            makeLabel.Text = $"Make: {DetectedModel.MakeName}";

            DrawRectangle();
        }

        void DrawRectangle()
        {
            var imageViewHeight = carImage.Frame.Height;
            var imageViewWidth = carImage.Frame.Width;

            var tlX = imageViewWidth * DetectedModel.Rectangle.TlX;
            var tlY = imageViewHeight * DetectedModel.Rectangle.TlY;
            var brX = imageViewWidth * DetectedModel.Rectangle.BrX;
            var brY = imageViewHeight * DetectedModel.Rectangle.BrY;

            var widht = brX - tlX;
            var height = brY - tlY;

            var view = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear,
                Layer =
                {
                    BorderWidth = 2f,
                    BorderColor = UIColor.Yellow.CGColor
                }
            };

            view.Frame = new CGRect(tlX, tlY, widht, height);
            carImage.AddSubview(view);
        }
    }
}

