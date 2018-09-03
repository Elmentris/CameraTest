using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using CameraTest.Core.Common.Models;
using CameraTest.Core.PlatformAbstractions;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace CameraTest.Droid.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        readonly CancelListener<bool> _cancelListenerForConfirm;
        readonly CancelListener _cancelListenerForAlert;

		protected Activity CurrentActivity
		{
			get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
		}

		public UserInteractionService()
		{
			_cancelListenerForConfirm = new CancelListener<bool>();
			_cancelListenerForAlert = new CancelListener();
		}

		public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			Confirm(message, confirmed =>
			{
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton, cancellable);
		}

		public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null || (CurrentActivity != null && CurrentActivity.IsFinishing))
					return;

				_cancelListenerForConfirm.Answer = answer;

				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate
						{
                            answer?.Invoke(true);
                        })
						.SetNegativeButton(cancelButton, delegate
						{
                            answer?.Invoke(false);
                        })
						.SetCancelable(cancellable)
						.SetOnCancelListener(_cancelListenerForConfirm)
						.Show();
			}, null);
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, tcs.SetResult, title, okButton, cancelButton, cancellable);
			return tcs.Task;
		}

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
		{
			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null || (CurrentActivity != null && CurrentActivity.IsFinishing))
					return;

				_cancelListenerForAlert.Answer = done;

				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate
						{
                            done?.Invoke();
                        })
						.SetOnCancelListener(_cancelListenerForAlert)
						.Show();
			}, null);
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.SetResult(null), title, okButton);
			return tcs.Task;
		}

		public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Input(message, (ok, text) =>
			{
				if (ok)
					okClicked(text);
			},
				placeholder, title, okButton, cancelButton, initialText);
		}

		public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Application.SynchronizationContext.Post(ignored =>
			{
				//if (CurrentActivity == null || (CurrentActivity != null && CurrentActivity.IsFinishing))
				//	return;

				//LayoutInflater inflater = (LayoutInflater)CurrentActivity.GetSystemService(Context.LayoutInflaterService);
				//var view = inflater.Inflate(Resource.Layout.input_dialog, null);

				//var input = view.FindViewById<EditText>(Resource.Id.input);
				//input.Hint = hint;
				//input.Text = initialText;

				//new AlertDialog.Builder(CurrentActivity)
					//.SetMessage(message)
						//.SetTitle(title)
						//.SetView(view)
						//.SetPositiveButton(okButton, delegate
						//{
      //                      answer?.Invoke(true, input.Text);
      //                  })
						//.SetNegativeButton(cancelButton, delegate
						//{
                        //    answer?.Invoke(false, input.Text);
                        //})
                               //.Show().SetCanceledOnTouchOutside(false);
			}, null);
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.SetResult(new InputResponse { Ok = ok, Text = text }), placeholder, title, okButton, cancelButton, initialText);
			return tcs.Task;
		}

		public int DpToPixel(Context context, float dp)
		{
			return (int)(dp * ((float)context.Resources.DisplayMetrics.DensityDpi / 160f));
		}

		public void Selector(List<SelectorItem> items, Action<SelectorItem> selector, string title = null, string cancelButton = "Cancel")
		{
			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null || (CurrentActivity != null && CurrentActivity.IsFinishing))
					return;

				new AlertDialog.Builder(CurrentActivity)
							   .SetTitle(title)
							   .SetItems(items.Select(x => x.Text).ToArray(), (s, e) => { selector?.Invoke(items.ElementAt(e.Which)); })
							   .Show();
			}, null);
		}

        public void Selector(List<SelectorItem> items, Action<SelectorItem> selector, SelectorItem cancel, string title = null, string cancelButton = "Cancel")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null || (CurrentActivity != null && CurrentActivity.IsFinishing))
                    return;

                new AlertDialog.Builder(CurrentActivity)
                    .SetTitle(title)
                    .SetItems(items.Select(x => x.Text).ToArray(), (s, e) => { selector?.Invoke(items.ElementAt(e.Which)); })
                    .Show();
            }, null);
        }
    }
}
