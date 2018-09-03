using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CameraTest.Core.Common.Interfaces;
using CameraTest.Core.Common.Models;
using CameraTest.Core.Common.Services;
using CameraTest.Core.PlatformAbstractions;
using CameraTest.Core.Resources;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace CameraTest.Core.ViewModels.Abstract
{
    public abstract class BaseViewModel : MvxViewModel<ViewModelParameter, ViewModelResult>
	{
        IMvxViewModelLoader _viewModelLoader;
		protected readonly IMvxMessenger Messenger = Mvx.Resolve<IMvxMessenger>();
        protected readonly IServerCommandWrapperService ServerCommandWrapperService = new ServerCommandWrapperService();
	    protected readonly IUserInteractionService UserInteractionService = Mvx.Resolve<IUserInteractionService>();
	    protected readonly IMvxNavigationService NavigationService = Mvx.Resolve<IMvxNavigationService>();
        protected readonly List<MvxSubscriptionToken> Subscriptions = new List<MvxSubscriptionToken>();
//	    protected readonly INotificationHubService NotificationHubService = Mvx.Resolve<INotificationHubService>();
	    protected ViewModelParameter Parameter;
	    Task _syncTask;

        #region Properties
        public BaseViewModel()
	    {
	        ServerCommandWrapperService.IsBusyChanged += IsBusyChangedHandler;
//	        NotificationHubService.OnSocketChanged += OnSocketChanged;
	    }
        
	    IMvxViewModelLoader ViewModelLoader => _viewModelLoader ?? (_viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>());
        public bool NotIsBusy => !_isBusy;

        string _title = string.Empty;
        public virtual string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        bool _isProccessing;
        public bool IsProccessing
        {
            get => _isProccessing;
            set => SetProperty(ref _isProccessing, value);
        }

        bool _isBusy;
        public virtual bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                    RaisePropertyChanged(() => NotIsBusy);
            }
        }

        public string this[string resource] => GetText(resource);
        protected string GetText(string resource) => Translator.GetText(resource);

        protected Task AsyncAlertFromResources(string messageKey, string titleKey = "") =>
            UserInteractionService.AlertAsync(GetText(messageKey), GetText(titleKey));
        #endregion

      

        #region Methods

        public override void Prepare(ViewModelParameter parameter)
	    {
	        Parameter = parameter;
	    }

        public TViewModel LoadViewModel<TViewModel>(MvxBundle parameters = null) where TViewModel : MvxViewModel
        {
			return ViewModelLoader.LoadViewModel(new MvxViewModelRequest(typeof(TViewModel), parameters, null), null) as TViewModel;
        }

		public override void Start()
		{
			base.Start();
			DebugMethod();
		}

        public virtual void OnResume()
        {
            DebugMethod();
        }

        public virtual void OnPause()
        {
            DebugMethod();
        }

        protected virtual void MakeChoose(SelectorItem selector, object item = null)
        {
            selector?.Command?.Execute(item);
        }

        void IsBusyChangedHandler(object sender, bool e) => IsBusy = e;

        void DebugMethod([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
		{
			Debug.WriteLine("{1} of {0}", memberName, GetType().FullName);
		}

//	    private async void OnSocketChanged(object sender, SocketStatus e)
//	    {
////	        if (e == SocketStatus.Connected)
////	            await Synchronize();
////	        else
////	            _syncTask = null;
//	    }

	    private Task Synchronize()
	    {
	        if (_syncTask?.IsCompleted ?? true)
	            _syncTask = SyncData();
	        return _syncTask;
	    }

	    public virtual async Task SyncData()
	    {
	        
	    }

        #endregion

        public virtual void OnDestroy()
		{
			DebugMethod();
			foreach (var item in Subscriptions)
				item.Dispose();

            ServerCommandWrapperService.IsBusyChanged -= IsBusyChangedHandler;
//		    NotificationHubService.OnSocketChanged -= OnSocketChanged;
        }

        MvxCommand _closeCommand;
		public virtual ICommand CloseCommand
		{
			get
			{
				return _closeCommand ?? (_closeCommand = new MvxCommand(async () => await NavigationService.Close(this)));
			}
		}
	}
}
