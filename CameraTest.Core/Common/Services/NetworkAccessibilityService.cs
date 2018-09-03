using CameraTest.Core.Common.Interfaces;

namespace CameraTest.Core.Common.Services
{
    public class NetworkAccessibilityService : INetworkAccessibilityService
    {
        public bool HasAccess => Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
    }
}
