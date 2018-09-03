using System.Threading.Tasks;
using CameraTest.Core.ViewModels.Abstract;
using CameraTest.Core.Common.Services;
using CameraTest.Core.Common.Interfaces;
using Plugin.Permissions.Abstractions;
using MvvmCross.Commands;
using Grpc.Core;
using CarNetAi;
using Google.Protobuf;
using System;
using CameraTest.Core.Units.DetectModel;
using CameraTest.Core.Units.Carnet.Services.Abstractions;

namespace CameraTest.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IPermissionsService _permissionService;
        private readonly ICarnetAiService carnetAiService;

        public MainViewModel(IPermissionsService permissionsService, ICarnetAiService carnetAiService)
        {
            _permissionService = permissionsService;
            this.carnetAiService = carnetAiService;
        }

        public override async void OnResume()
        {
            base.OnResume();

            await CheckPermissions();
        }

        private async Task CheckPermissions()
        {
            var cameraPermission = await _permissionService.PreCheckPermissionsAccessAsync(Permission.Camera);
            if (cameraPermission == PermissionStatus.Granted)
            {
                IsCameraAccessGranted = true;
                return;
            }

            var result = await _permissionService.CheckPermissionsAccesGrantedAsync(Permission.Camera);
            if (result)
            {
                IsCameraAccessGranted = true;
                return;
            }
        }

        private bool isCameraAccessGranted;
        public bool IsCameraAccessGranted
        { 
            get => isCameraAccessGranted;
            set => SetProperty(ref isCameraAccessGranted, value);
        }

        private MvxAsyncCommand<byte[]> captureCommand;
        public IMvxAsyncCommand<byte[]> CaptureCommand
        {
            get 
            {
                return captureCommand ?? (captureCommand = new MvxAsyncCommand<byte[]>(async (item) => 
                {
                    if (item == null || item.Length == 0)
                        return;

                    await ServerCommandWrapperService.InternetServerCommandWrapperWithBusy(async () => 
                    {
                        var result = await carnetAiService.TryDetectModel(item);
                        if(!result.OperationSucceeded)
                        {
                            await AsyncAlertFromResources(result.Result.Error);
                            return;
                        }

                        await NavigationService.Navigate<ResultViewModel, ViewModelParameter>(new ViewModelParameter(result.Result));
                    });
                }));
            }
        }
    }
}
