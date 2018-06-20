using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Demo
{
	public class ContentPageViewModel : INotifyPropertyChanged
	{
		private bool isVisibleSegment = false;      
		private bool isEnableSegment = false;

		public Command ChangeIsVisibleSegmentCommand { get; protected set; }
		public Command ChangeIsEnableSegmentCommand { get; protected set; }

		public bool IsVisibleSegment
		{
			get => isVisibleSegment;
			set
			{
				isVisibleSegment = value;
				OnPropertyChanged(nameof(IsVisibleSegment));
			}
		}

		public bool IsEnableSegment
        {
			get => isEnableSegment;
            set
            {
				isEnableSegment = value;
				OnPropertyChanged(nameof(IsEnableSegment));
            }
        }



		public ContentPageViewModel()
		{
			ChangeIsVisibleSegmentCommand = new Command(ChangeIsVisibleSegment);
			ChangeIsEnableSegmentCommand = new Command(ChangeIsEnableSegment);
		}

		protected void ChangeIsEnableSegment()
		{
			IsEnableSegment = !IsEnableSegment;
		}


		protected void ChangeIsVisibleSegment()
		{
			IsVisibleSegment = !IsVisibleSegment;
		}
        

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

	}
}
