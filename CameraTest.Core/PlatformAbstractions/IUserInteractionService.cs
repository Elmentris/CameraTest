using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CameraTest.Core.Common.Models;

namespace CameraTest.Core.PlatformAbstractions
{
    public interface IUserInteractionService
    {
		void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true);
		void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true);
		Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", bool cancellable = true);

		void Alert(string message, Action done = null, string title = "", string okButton = "OK");
		Task AlertAsync(string message, string title = "", string okButton = "OK");

		void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
		void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);
		Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null);

//		void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe");
//		Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe");

		void Selector(List<SelectorItem> items, Action<SelectorItem> selector, string title = null, string cancelButton = "Cancel");
        void Selector(List<SelectorItem> items, Action<SelectorItem> selector, SelectorItem cancel, string title = null,
            string cancelButton = "Cancel");
    }
}
