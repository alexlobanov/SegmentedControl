using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SegmentedControl.FormsPlugin.Abstractions
{
    /// <summary>
    /// SegmentedControl Interface
    /// </summary>
	public class SegmentedControl : Layout<View>
	{

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			Debug.WriteLine("Root: " + propertyName);
			base.OnPropertyChanged(propertyName);
		}

		public static readonly BindableProperty TintColorProperty = BindableProperty.Create("TintColor", typeof(Color), typeof(SegmentedControl), Color.Blue);

		public Color TintColor
		{
			get { return (Color)GetValue(TintColorProperty); }
			set { SetValue(TintColorProperty, value); }
		}

        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create("DisabledColor", typeof(Color), typeof(SegmentedControl), Color.Gray);

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

		public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(SegmentedControl), Color.White);

		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}

		public static readonly BindableProperty SelectedSegmentProperty = BindableProperty.Create("SelectedSegment", typeof(int), typeof(SegmentedControl), 0);

		public int SelectedSegment
		{
			get { 
				return (int)GetValue(SelectedSegmentProperty); 
			}
			set { 
				SetValue(SelectedSegmentProperty, value);
			}
		}
	
		public event EventHandler<ValueChangedEventArgs> ValueChanged;      

		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler<PropertyChangedEventArgs> ChildrenPropertyChanged;
       
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendChinldenPropertyChanged(string property)
        {
			ChildrenPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SendValueChanged()
		{
            ValueChanged?.Invoke(this, new ValueChangedEventArgs { NewValue = this.SelectedSegment });
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			for (var i = 0; i < Children.Count; i++)
            {
				var child = (View)Children[i];
                if (child.IsVisible)
					LayoutChildIntoBoundingRegion(child, Rectangle.Zero);
            }
		}
	}

	public class SegmentedControlOption : View
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(SegmentedControlOption), string.Empty);

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			Debug.WriteLine("Children: " + propertyName);
			base.OnPropertyChanged(propertyName);         
			switch (propertyName)
			{
				case "Text":
				case "IsEnabled":
				case "IsVisible":
					var parentView = this.Parent as SegmentedControl;
					if (parentView != null)
					{
						parentView.SendChinldenPropertyChanged(propertyName);                  
					}
					break;
			}
		}

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}

	public class ValueChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
}
