using System;
using Plugin.Connectivity.Abstractions;

namespace CameraTest.Core.Common.Interfaces
{
	public interface IConnectivityService
	{
		bool IsConnected { get; }
		event EventHandler<ConnectivityChangedEventArgs> Connected;
	}
}
