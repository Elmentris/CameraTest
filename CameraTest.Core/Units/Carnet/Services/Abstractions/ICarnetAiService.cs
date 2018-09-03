using System;
using System.Threading.Tasks;
using CameraTest.Core.Common.Models;
using CameraTest.Core.Units.DetectModel;

namespace CameraTest.Core.Units.Carnet.Services.Abstractions
{
    public interface ICarnetAiService
    {
        Task<TryResult<DetectModelParameter>> TryDetectModel(byte[] image);
    }
}
