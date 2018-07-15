using System;
using Xamarin.Forms;
using SegmentedControl.FormsPlugin.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using SegmentedControl.FormsPlugin.Abstractions;
using System.Collections.Generic;
using System.Linq;

[assembly: ExportRenderer(typeof(SegmentedControl.FormsPlugin.Abstractions.SegmentedControl), typeof(SegmentedControlRenderer))]
namespace SegmentedControl.FormsPlugin.iOS
{
	/// <summary>
	/// SegmentedControl Renderer
	/// </summary>
	public class SegmentedControlRenderer : ViewRenderer<Abstractions.SegmentedControl, UISegmentedControl>
	{
		UISegmentedControl nativeControl;      
       

		protected override void OnElementChanged(ElementChangedEventArgs<Abstractions.SegmentedControl> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				// Instantiate the native control and assign it to the Control property with
				// the SetNativeControl method
				nativeControl = CreateNativeSegmentedControl();
				SetSelectedTextColor();         
				SetNativeControl(nativeControl);
			}

            if ((e.OldElement == null) && (e.NewElement != null)) //new control
            {
                // Configure the control and subscribe to event handlers

                e.NewElement.ChildrenPropertyChanged += SegmentedChildrenPropertyChanged;
                if (nativeControl != null)
                    nativeControl.ValueChanged += NativeControl_ValueChanged;
            }

            if ((e.OldElement != null) && (e.NewElement == null)) //remove control
			{
				// Unsubscribe from event handlers and cleanup any resources
				if (nativeControl != null)
					nativeControl.ValueChanged -= NativeControl_ValueChanged;
                e.OldElement.ChildrenPropertyChanged -= SegmentedChildrenPropertyChanged;            
			}
		}

		private UISegmentedControl CreateNativeSegmentedControl()
		{
			var nativeSegmentControl = new UISegmentedControl();

			for (var i = 0; i < Element.Children.Count; i++)
			{
				var segmentedControlOption = Element.Children[i] as SegmentedControlOption;
				if (segmentedControlOption == null)
					continue;
				var visibleEnable = segmentedControlOption.IsVisible && segmentedControlOption.IsEnabled;
				nativeSegmentControl.InsertSegment(segmentedControlOption.Text, i, false);
				nativeSegmentControl.SetEnabled(visibleEnable, i);                  
			}

			nativeSegmentControl.Enabled = Element.IsEnabled;
			nativeSegmentControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Element.DisabledColor.ToUIColor();
			nativeSegmentControl.SelectedSegment = Element.SelectedSegment;
			return nativeSegmentControl;
		}

		protected void SegmentedChildrenPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
            {
				case "Text":
					for (var i = 0; i < Element.Children.Count; i++)
					{
						var segmentedControlOption = Element.Children[i] as SegmentedControlOption;
						if (segmentedControlOption == null)
							continue;
						nativeControl.SetTitle(segmentedControlOption.Text, i);    
					}
					break;
                case "IsVisible":
				case "IsEnabled":
                    for (var i = 0; i < Element.Children.Count; i++)
                    {
                        var segmentedControlOption = Element.Children[i] as SegmentedControlOption;                  
						var visibleEnable = segmentedControlOption.IsVisible && segmentedControlOption.IsEnabled;
						nativeControl.SetEnabled(visibleEnable, i);                  
                    }
                    break;      
            }
		}


		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

            if (nativeControl == null || Element == null) return;

			switch (e.PropertyName)
			{
				case "Renderer":
                    Element?.SendValueChanged();
					break;
				case "SelectedSegment":
                    nativeControl.SelectedSegment = Element.SelectedSegment;
                    Element.SendValueChanged();
					break;
				case "TintColor":
                    nativeControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Element.DisabledColor.ToUIColor();
					break;
				case "IsEnabled":
                    nativeControl.Enabled = Element.IsEnabled;
                    nativeControl.TintColor = Element.IsEnabled ? Element.TintColor.ToUIColor() : Element.DisabledColor.ToUIColor();
					break;
				case "SelectedTextColor":
                    SetSelectedTextColor();
					break;
				
			}

		}


		void SetSelectedTextColor()
		{
			var attr = new UITextAttributes();
			attr.TextColor = Element.SelectedTextColor.ToUIColor();
			nativeControl.SetTitleTextAttributes(attr, UIControlState.Selected);
		}

        void NativeControl_ValueChanged (object sender, EventArgs e)
		{
			Element.SelectedSegment = (int)nativeControl.SelectedSegment;
		}

		protected override void Dispose(bool disposing)
		{
			if (nativeControl != null)
			{
				nativeControl.ValueChanged -= NativeControl_ValueChanged;                        
				nativeControl.Dispose();
				nativeControl = null;
			}

			try
			{
				base.Dispose(disposing);
			}
			catch (Exception)
			{
				return;
			}
		}

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
		{
			var temp = DateTime.Now;
		}
    }
}
