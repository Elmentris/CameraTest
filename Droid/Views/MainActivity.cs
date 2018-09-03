using Android.OS;
using Android.Views;
using Android.App;
using Android.Content.PM;
using CameraTest.Droid.Views.Abstract;
using CameraTest.Core.ViewModels;
using Android.Widget;
using System;
using Android.Support.Constraints;
using CameraTest.Droid.Views.Camera;
using Android.Hardware;
using System.Collections.Generic;
using System.Linq;

namespace CameraTest.Droid.Views
{
    [Activity(Theme = "@style/CameraTest.Main", Label = "", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : BaseScrollToolbarActivity<MainViewModel>, Android.Hardware.Camera.IPictureCallback, Android.Hardware.Camera.IAutoFocusCallback
    {
        protected override int LayoutId => Resource.Layout.Main;

        Android.Hardware.Camera _camera;
        bool _cameraReleased = false;
        Button _caprtureButton;
        ConstraintLayout _cameraRoot;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _toolbar.Visibility = ViewStates.Gone;

            _cameraRoot = this.FindViewById<ConstraintLayout>(Resource.Id.clCameraRoot);
            _cameraRoot.Click += TapOnCamera;
            _caprtureButton = this.FindViewById<Button>(Resource.Id.bCapture);
            _caprtureButton.Click += CaptureClicked;

            InitCamera();
        }

        private void TapOnCamera(object sender, EventArgs e)
        {
            _camera.AutoFocus(this);
        }

        void InitCamera()
        {
            _camera = SetUpCamera();
            SetCamFocusMode();
            SetCameraPreview();
        }

        private void CaptureClicked(object sender, EventArgs e)
        {
            try
            {
                _camera.StartPreview();
                _camera.TakePicture(null, null, this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetCamFocusMode()
        {
            if (_camera == null)
            {
                return;
            }
            var parameters = _camera.GetParameters();
            List<String> focusModes = parameters.SupportedFocusModes.ToList();
            if (focusModes.Contains(Android.Hardware.Camera.Parameters.FocusModeContinuousPicture))
            {
                parameters.FocusMode = Android.Hardware.Camera.Parameters.FocusModeContinuousPicture;
            }
            else if (focusModes.Contains(Android.Hardware.Camera.Parameters.FocusModeAuto))
            {
                parameters.FocusMode = Android.Hardware.Camera.Parameters.FocusModeAuto;
            }
            _camera.SetParameters(parameters);
        }


        private void SetCameraPreview()
        {
            _cameraRoot.AddView(new CameraPreview(this, _camera));
        }

        protected override void OnDestroy()
        {
            _camera.StopPreview();
            _camera.Release();
            _cameraReleased = true;
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            if (_cameraReleased)
            {
                _camera.Reconnect();
                _camera.StartPreview();
                _cameraReleased = false;
            }
            base.OnResume();
        }

        /// <summary>
        /// Get an instace of the current hardware camera of the device
        /// </summary>
        /// <returns></returns>
        Android.Hardware.Camera SetUpCamera()
        {
            Android.Hardware.Camera c = null;
            try
            {
                c = Android.Hardware.Camera.Open();
            }
            catch (Exception e)
            {
                //Log.Debug("", "Device camera not available now.");
            }

            return c;
        }

        public void OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
        {
            try
            {
                ViewModel.CaptureCommand?.Execute(data);
                //We start the camera preview back since after taking a picture it freezes
                camera.StartPreview();
            }
            catch (System.Exception e)
            {
                //Log.Debug(APP_NAME, "File not found: " + e.Message);
            }

        }

        public void OnAutoFocus(bool success, Android.Hardware.Camera camera)
        {

        }
    }
}
