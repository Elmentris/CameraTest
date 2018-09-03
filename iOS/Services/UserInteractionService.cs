using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraTest.Core.Common.Models;
using CameraTest.Core.PlatformAbstractions;
using UIKit;

namespace CameraTest.iOS.Services
{
    public class UserInteractionService : IUserInteractionService
    {
		public void Confirm(string message, Action okClicked, string title = "", string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			Confirm(message, confirmed =>
			{
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton, cancellable);
		}

		public void Confirm(string message, Action<bool> answer, string title = "", string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var confirm = new UIAlertView(title ?? string.Empty, message,
											  null, cancelButton, okButton);
				if (answer != null)
				{
					confirm.Clicked +=
						(sender, args) =>
							answer(confirm.CancelButtonIndex != args.ButtonIndex);
				}
				confirm.Show();
			});
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true)
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, (r) => tcs.TrySetResult(r), title, okButton, cancelButton, cancellable);
			return tcs.Task;
		}

//		public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
//		{
//			var confirm = new UIAlertView(title ?? string.Empty, message, null, negative, positive, neutral);
//			if (answer != null)
//			{
//				confirm.Clicked +=
//					(sender, args) =>
//					{
//						var buttonIndex = args.ButtonIndex;
//						if (buttonIndex == confirm.CancelButtonIndex)
//							answer(ConfirmThreeButtonsResponse.Negative);
//						else if (buttonIndex == confirm.FirstOtherButtonIndex)
//							answer(ConfirmThreeButtonsResponse.Positive);
//						else
//							answer(ConfirmThreeButtonsResponse.Neutral);
//					};
//				confirm.Show();
//			}
//		}
//
//		public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
//		{
//			var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
//			ConfirmThreeButtons(message, (r) => tcs.TrySetResult(r), title, positive, negative, neutral);
//			return tcs.Task;
//		}

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var alert = new UIAlertView(title ?? string.Empty, message, null, okButton);
				if (done != null)
				{
					alert.Clicked += (sender, args) => done();
				}
				alert.Show();
			});

		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.TrySetResult(null), title, okButton);
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

		public void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var input = new UIAlertView(title ?? string.Empty, message, null, cancelButton, okButton);
				input.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
				var textField = input.GetTextField(0);
				textField.Placeholder = placeholder;
				textField.Text = initialText;
				if (answer != null)
				{
					input.Clicked +=
						(sender, args) =>
							answer(input.CancelButtonIndex != args.ButtonIndex, textField.Text);
				}
				input.Show();
			});
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.TrySetResult(new InputResponse { Ok = ok, Text = text }), placeholder, title, okButton, cancelButton, initialText);
			return tcs.Task;
		}

		public void Selector(List<SelectorItem> items, Action<SelectorItem> selector, string title = null, string cancelButton = "Cancel")
		{
			UIAlertController alertController = UIAlertController.Create(title, null, UIAlertControllerStyle.ActionSheet);

			foreach (var item in items)
			{
                alertController.AddAction(UIAlertAction.Create(item.Text, UIAlertActionStyle.Default, (obj) => selector(items.ElementAt(item.Id))));
			}
			alertController.AddAction(UIAlertAction.Create(cancelButton, UIAlertActionStyle.Cancel, (obj) => { }));

			var window = UIApplication.SharedApplication.KeyWindow;
			var controller = window.RootViewController;
			while (controller.PresentedViewController != null)
			{
				controller = controller.PresentedViewController;
			}

			UIPopoverPresentationController presentationPopover = alertController.PopoverPresentationController;
			controller = controller.PresentedViewController ?? controller;
			if (presentationPopover != null)
			{
				presentationPopover.SourceView = controller.View;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
			}

			controller.PresentViewController(alertController, true, null);
		}

        public void Selector(List<SelectorItem> items, Action<SelectorItem> selector, SelectorItem cancel, string title = null, string cancelButton = "Cancel")
        {
            UIAlertController alertController = UIAlertController.Create(title, null, UIAlertControllerStyle.ActionSheet);

            foreach (var item in items)
            {
                alertController.AddAction(UIAlertAction.Create(item.Text, UIAlertActionStyle.Default, (obj) => selector(items.ElementAt(item.Id))));
            }
            alertController.AddAction(UIAlertAction.Create(cancelButton, UIAlertActionStyle.Cancel, (obj) => selector(cancel)));

            var window = UIApplication.SharedApplication.KeyWindow;
            var controller = window.RootViewController;
            while (controller.PresentedViewController != null)
            {
                controller = controller.PresentedViewController;
            }

            UIPopoverPresentationController presentationPopover = alertController.PopoverPresentationController;
            controller = controller.PresentedViewController ?? controller;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = controller.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            controller.PresentViewController(alertController, true, null);
        }
    }
}
