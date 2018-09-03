using System;
using CameraTest.Droid.Views.Abstract;
using CameraTest.Core;
using Android.OS;
using CameraTest.Core.Units.DetectModel;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using Android.Graphics;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Support.Constraints;
using Android.Content;

namespace CameraTest.Droid.Views
{
    [Activity(Theme = "@style/CameraTest.Main", Label = "", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class ResultActivity :BaseScrollToolbarActivity<ResultViewModel>
    {
        protected override int LayoutId => Resource.Layout.Result;

        ImageView carPhoto;
        TextView makeName;
        TextView modelName;
        TextView probability;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _toolbar.Visibility = ViewStates.Gone;

            FindViews();
        }

        protected override void OnResume()
        {
            base.OnResume();

            DoBind();
        }

        void FindViews()
        {
            carPhoto = FindViewById<ImageView>(Resource.Id.ivCarPhoto);
            makeName = FindViewById<TextView>(Resource.Id.tvMakeName);
            modelName = FindViewById<TextView>(Resource.Id.tvModelName);
            probability = FindViewById<TextView>(Resource.Id.tvProbability);
        }

        void DoBind()
        {
            var set = this.CreateBindingSet<ResultActivity, ResultViewModel>();
            set.Bind(this).For(v => v.DetectedModel).To(vm => vm.DetectedModel);
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

            makeName.Text = $"Make: {DetectedModel.MakeName}";
            modelName.Text = $"Model: {DetectedModel.ModelName}";
            probability.Text = $"Probability: {DetectedModel.Probability}";

            var bmp = BitmapFactory.DecodeByteArray(DetectedModel.Bytes, 0, DetectedModel.Bytes.Length);

            carPhoto.SetImageBitmap(Bitmap.CreateScaledBitmap(bmp, WidthPixels, HeightPixels, false));
           
            DrawRectangle();
        }

        void DrawRectangle()
        {
            var imageViewHeight = HeightPixels;
            var imageViewWidth = WidthPixels;

            var tlX = imageViewWidth * DetectedModel.Rectangle.TlX;
            var tlY = imageViewHeight * DetectedModel.Rectangle.TlY;
            var brX = imageViewWidth * DetectedModel.Rectangle.BrX;
            var brY = imageViewHeight * DetectedModel.Rectangle.BrY;

            var widht = brX - tlX;
            var height = brY - tlY;

            var rectangle = new RectangleView(this, tlX, tlY, brX, brY);
            _constraintLayoutRoot.AddView(rectangle);
            _constraintLayoutRoot.BringChildToFront(rectangle);
        }
    }

    public class RectangleView : View
    {
        float tlX, tlY, brX, brY;

        public RectangleView(Context context, double tlX, double tlY, double brX, double brY) 
            : base(context)
        {
            this.tlX = (float)tlX;
            this.tlY = (float)tlY;
            this.brX = (float)brX;
            this.brY = (float)brY;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var paint = new Paint { Color = Color.Yellow, StrokeWidth = 3 };
            paint.SetStyle(Paint.Style.Stroke);
            canvas.DrawRect(new RectF(tlX, tlY, brX, brY), paint);
        }
    }

}
