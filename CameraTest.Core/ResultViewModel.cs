using System;
using CameraTest.Core.ViewModels.Abstract;
using CameraTest.Core.Units.DetectModel;
namespace CameraTest.Core
{
    public class ResultViewModel : BaseViewModel
    {
        public override void Prepare(ViewModelParameter parameter)
        {
            DetectedModel = parameter?.Deserialize<DetectModelParameter>();
        }

        private DetectModelParameter detectedModel;
        public DetectModelParameter DetectedModel
        {
            get => detectedModel;
            set => SetProperty(ref detectedModel, value);
        }
    }
}
