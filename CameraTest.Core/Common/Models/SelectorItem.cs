using System.Windows.Input;

namespace CameraTest.Core.Common.Models
{
    public class SelectorItem
    {
		public int Id { get; set; }
		public string Text { get; set; }
		public ICommand Command { get; set; }
    }
}
