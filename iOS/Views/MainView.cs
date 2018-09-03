using System;

using UIKit;
using CameraTest.iOS.Views.Abstractions;
using CameraTest.Core.ViewModels;
using AVFoundation;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace CameraTest.iOS.Views
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class MainView : BaseView<MainViewModel>
    {
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        AVCaptureVideoPreviewLayer videoPreviewLayer;

        protected override void DoBind()
        {
            base.DoBind();

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Bind(this).For(v => v.IsCameraAccessGranted).To(vm => vm.IsCameraAccessGranted);
            set.Bind(cancelButton).To(vm => vm.CloseCommand);
            set.Apply();
        }

        async void CaptureButtonClick(object sender, EventArgs args)
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            var jpegAsByteArray = jpegImageAsNsData.ToArray();

            ViewModel.CaptureCommand?.Execute(jpegAsByteArray);
        }

        void ShowCameraPreview()
        {
            var captureSession = new AVCaptureSession();

            var viewLayer = liveCameraStream.Layer;
            videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = this.View.Frame
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
            captureSession.AddInput(captureDeviceInput);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
            captureSession.StartRunning();

            captureButton.Hidden = false;
            cancelButton.Hidden = false;
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        void HideCameraPreview()
        {
            if(captureSession != null)
            {
                captureSession.StopRunning();
                captureSession.RemoveOutput(stillImageOutput);
                captureSession.Dispose();
                captureSession = null;
            }

            if(stillImageOutput != null)
            {
                stillImageOutput.Dispose();
                stillImageOutput = null;
            }

            captureButton.Hidden = true;
            cancelButton.Hidden = true;
        }

        bool isCameraAccessGranted;
        public bool IsCameraAccessGranted
        {
            get => isCameraAccessGranted;
            set
            {
                isCameraAccessGranted = value;
                if (value)
                {
                    ShowCameraPreview();
                }
                else
                {
                    HideCameraPreview();
                }
            }
        }
    }
}

