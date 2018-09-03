using System;
using CameraTest.Core.Units.Carnet.Services.Abstractions;
using CarNetAi;
using Grpc.Core;
using System.Threading.Tasks;
using CameraTest.Core.Units.DetectModel;
using CameraTest.Core.Common.Models;
using Google.Protobuf;
using CameraTest.Core.Units.DetectModel.Mappers;

namespace CameraTest.Core.Units.Carnet.Services.Implementations
{
    internal class CarnetAiService : ICarnetAiService
    {
        private Channel channel;
        private CarNetAI.CarNetAIClient client;

        public CarnetAiService()
        {
            channel = new Channel("real.by:50051", ChannelCredentials.Insecure);
            client = new CarNetAI.CarNetAIClient(channel);
        }

        public async Task<TryResult<DetectModelParameter>> TryDetectModel(byte[] image)
        {
            var postResult = await client.DetectModelAsync(new DetectModelRequest
            {
                Image = ByteString.CopyFrom(image),

            }, null);

            if (!postResult.IsSuccess)
            {
                return TryResult.Create(false, new DetectModelParameter(){Error = postResult.Error});
            }

            var detectModelParameter = postResult.ToDetectModelParameter();
            detectModelParameter.Bytes = image;

            return TryResult.Create(true, detectModelParameter);
        }
    }
}
