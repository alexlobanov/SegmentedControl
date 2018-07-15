using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SegmentedControl.FormsPlugin.Abstractions
{
    /// <summary>
    ///     SegmentedControl Interface
    /// </summary>
    public class SegmentedControl : Layout<View>
    {
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create("TintColor", typeof(Color), typeof(SegmentedControl), Color.Blue);

        public static readonly BindableProperty DisabledColorProperty =
            BindableProperty.Create("DisabledColor", typeof(Color), typeof(SegmentedControl), Color.Gray);

        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(SegmentedControl), Color.White);

        public static readonly BindableProperty SelectedSegmentProperty =
            BindableProperty.Create("SelectedSegment", typeof(int), typeof(SegmentedControl), 0);

        public Color TintColor
        {
            get => (Color) GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }

        public Color DisabledColor
        {
            get => (Color) GetValue(DisabledColorProperty);
            set => SetValue(DisabledColorProperty, value);
        }

        public Color SelectedTextColor
        {
            get => (Color) GetValue(SelectedTextColorProperty);
            set => SetValue(SelectedTextColorProperty, value);
        }

        public int SelectedSegment
        {
            get => (int) GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
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
            ValueChanged?.Invoke(this, new ValueChangedEventArgs {NewValue = SelectedSegment});
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                if (child.IsVisible)
                    LayoutChildIntoBoundingRegion(child, Rectangle.Zero);
            }
        }
    }

    public class SegmentedControlOption : View
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(SegmentedControlOption), string.Empty);

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "Text":
                case "IsEnabled":
                case "IsVisible":
                    var parentView = Parent as SegmentedControl;
                    if (parentView != null) parentView.SendChinldenPropertyChanged(propertyName);
                    break;
            }
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
}