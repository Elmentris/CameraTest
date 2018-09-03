using System;
using static DetectModelResponse.Types.DetectedObject.Types;

namespace CameraTest.Core.Units.DetectModel
{
    public class BoundingBox
    {
        public double TlX { get; set; }
        public double TlY { get; set; }
        public double BrX { get; set; }
        public double BrY { get; set; }

        public BoundingBox(){}
        public BoundingBox(BBox bBox)
        {
            TlX = bBox.TlX;
            TlY = bBox.TlY;
            BrX = bBox.BrX;
            BrY = bBox.BrY;
        }
    }
}
