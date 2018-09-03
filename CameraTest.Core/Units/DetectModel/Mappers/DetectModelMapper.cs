using System;
using System.Linq;
using Newtonsoft.Json;
namespace CameraTest.Core.Units.DetectModel.Mappers
{
    public static class DetectModelMapper
    {
        public static DetectModelParameter ToDetectModelParameter(this DetectModelResponse response)
        {
            if (response == null)
                return null;

            var model = response.DetectedModels[response.SelectedDetectedModelIndex];
            var bbox = new BoundingBox(response.DetectedObjects.FirstOrDefault().Bbox);

            return new DetectModelParameter
            {
                MakeName = model?.MakeName,
                ModelName = model?.ModelName,
                Probability = model?.ModelProb ?? 0,
                Rectangle = bbox
            };
        }
    }
}
