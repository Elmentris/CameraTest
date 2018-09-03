using System;
using Android.Content;

namespace CameraTest.Droid.Services
{
	public class CancelListener<T> : Java.Lang.Object, IDialogInterfaceOnCancelListener
	{
		Action<T> _answer;
		public Action<T> Answer
		{
			get
			{
				return _answer;
			}
			set
			{
				_answer = value;
			}
		}


		public CancelListener()
		{
		}

		public CancelListener(Action<T> answer)
		{
			_answer = answer;
		}

		public void OnCancel(IDialogInterface dialog)
		{
			Answer?.Invoke(default(T));
		}
	}

	public class CancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
	{
		Action _answer;
		public Action Answer
		{
			get
			{
				return _answer;
			}
			set
			{
				_answer = value;
			}
		}


		public CancelListener()
		{
		}

		public CancelListener(Action answer)
		{
			_answer = answer;
		}

		public void OnCancel(IDialogInterface dialog)
		{
			Answer?.Invoke();
		}
	}

}
