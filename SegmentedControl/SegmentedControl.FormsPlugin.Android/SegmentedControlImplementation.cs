using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using SegmentedControl.FormsPlugin.Abstractions;
using SegmentedControl.FormsPlugin.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidView = Android.Views;

[assembly: ExportRenderer(typeof(SegmentedControl.FormsPlugin.Abstractions.SegmentedControl), typeof(SegmentedControlRenderer))]
namespace SegmentedControl.FormsPlugin.Android
{
    /// <summary>
    ///     SegmentedControl Renderer
    /// </summary>
    public class SegmentedControlRenderer : ViewRenderer<Abstractions.SegmentedControl, RadioGroup>
    {
        private readonly Context context;
        private RadioButton _rb;
        private RadioGroup nativeControl;

        public SegmentedControlRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Abstractions.SegmentedControl> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                // Instantiate the native control and assign it to the Control property with
                // the SetNativeControl method

                CreateRadioButtons();
                var option = (RadioButton) nativeControl.GetChildAt(Element.SelectedSegment);
                if (option != null)
                    option.Checked = true;
                SetNativeControl(nativeControl);
            }

            if (e.NewElement != null && e.OldElement == null)
            {
                // Configure the control and subscribe to event handlers
                e.NewElement.ChildrenPropertyChanged += SegmentedChildrenPropertyChanged;
                if (nativeControl != null)
                    nativeControl.CheckedChange += NativeControl_ValueChanged;
            }

            if (e.OldElement != null && e.NewElement == null)
            {
                // Unsubscribe from event handlers and cleanup any resources

                if (nativeControl != null)
                    nativeControl.CheckedChange -= NativeControl_ValueChanged;
                e.OldElement.ChildrenPropertyChanged -= SegmentedChildrenPropertyChanged;
            }
        }

        private void CreateRadioButtons()
        {
            var layoutInflater = AndroidView.LayoutInflater.From(context);

            var view = layoutInflater.Inflate(Resource.Layout.RadioGroup, null);

            nativeControl = (RadioGroup) layoutInflater.Inflate(Resource.Layout.RadioGroup, null);
            for (var i = 0; i < Element.Children.Count; i++)
            {
                var o = Element.Children[i] as SegmentedControlOption;
                if (o == null)
                    continue;
                var rb = (RadioButton) layoutInflater.Inflate(Resource.Layout.RadioButton, null);

                rb.LayoutParameters = new RadioGroup.LayoutParams(0, LayoutParams.WrapContent, 1f);
                rb.Text = o.Text;
                if (i == 0)
                    rb.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
                else if (i == Element.Children.Count - 1)
                    rb.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);

                ConfigureRadioButton(i, rb, o.IsEnabled, o.IsVisible);

                nativeControl.AddView(rb);
            }
        }

        protected void SegmentedChildrenPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Text":
                    for (var i = 0; i < Element.Children.Count; i++)
                    {
                        var segmentedControlOption = Element.Children[i] as SegmentedControlOption;
                        var childView = nativeControl.GetChildAt(i) as RadioButton;
                        if (segmentedControlOption == null || childView == null)
                            continue;
                        childView.Text = segmentedControlOption.Text;
                    }

                    break;
                case "IsVisible":
                    for (var i = 0; i < Element.Children.Count; i++)
                    {
                        var segmentedControlOption = Element.Children[i];
                        var childView = nativeControl.GetChildAt(i);
                        if (childView == null)
                            continue;
                        childView.Visibility = segmentedControlOption.IsVisible
                            ? AndroidView.ViewStates.Visible
                            : AndroidView.ViewStates.Gone;
                    }

                    break;
                case "IsEnabled":
                    OnPropertyChanged();
                    break;
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (nativeControl == null || Element == null) return;

            switch (e.PropertyName)
            {
                case "Renderer":
                    Element?.SendValueChanged();
                    break;
                case "SelectedSegment":
                    var option = (RadioButton) nativeControl.GetChildAt(Element.SelectedSegment);

                    if (option != null)
                        option.Checked = true;
                    if (Element.SelectedSegment < 0)
                    {
                        CreateRadioButtons();
                        nativeControl.CheckedChange += NativeControl_ValueChanged;
                        SetNativeControl(nativeControl);
                    }

                    Element.SendValueChanged();
                    break;
                case "TintColor":
                    OnPropertyChanged();
                    break;
                case "IsEnabled":
                    OnPropertyChanged();
                    break;
                case "SelectedTextColor":
                    var v = (RadioButton) nativeControl.GetChildAt(Element.SelectedSegment);
                    v.SetTextColor(Element.SelectedTextColor.ToAndroid());
                    break;
            }
        }

        private void OnPropertyChanged()
        {
            if (nativeControl != null && Element != null)
                for (var i = 0; i < Element.Children.Count; i++)
                {
                    var o = Element.Children[i];
                    var rb = (RadioButton) nativeControl.GetChildAt(i);
                    ConfigureRadioButton(i, rb, o.IsEnabled, o.IsVisible);
                }
        }

        private void ConfigureRadioButton(int i, RadioButton rb, bool isEnabled, bool isVisible)
        {
            if (i == Element.SelectedSegment)
            {
                rb.SetTextColor(Element.SelectedTextColor.ToAndroid());
                _rb = rb;
            }
            else
            {
                var textColor = Element.IsEnabled && isEnabled
                    ? Element.TintColor.ToAndroid()
                    : Element.DisabledColor.ToAndroid();
                rb.SetTextColor(textColor);
            }

            GradientDrawable selectedShape;
            GradientDrawable unselectedShape;

            var gradientDrawable = (StateListDrawable) rb.Background;
            var drawableContainerState = (DrawableContainer.DrawableContainerState) gradientDrawable.GetConstantState();
            var children = drawableContainerState.GetChildren();

            // Doesnt works on API < 18
            selectedShape = children[0] is GradientDrawable
                ? (GradientDrawable) children[0]
                : (GradientDrawable) ((InsetDrawable) children[0]).Drawable;
            unselectedShape = children[1] is GradientDrawable
                ? (GradientDrawable) children[1]
                : (GradientDrawable) ((InsetDrawable) children[1]).Drawable;

            var color = Element.IsEnabled && isEnabled
                ? Element.TintColor.ToAndroid()
                : Element.DisabledColor.ToAndroid();

            selectedShape.SetStroke(3, color);
            selectedShape.SetColor(color);
            unselectedShape.SetStroke(3, color);
            rb.Visibility = isVisible ? AndroidView.ViewStates.Visible : AndroidView.ViewStates.Gone;
            rb.Enabled = Element.IsEnabled && isEnabled;
        }

        private void NativeControl_ValueChanged(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            var rg = (RadioGroup) sender;
            if (rg.CheckedRadioButtonId != -1)
            {
                var id = rg.CheckedRadioButtonId;
                var radioButton = rg.FindViewById(id);
                var radioId = rg.IndexOfChild(radioButton);
                var sharedControl = Element.Children[radioId];

                var rb = (RadioButton) rg.GetChildAt(radioId);

                var color = Element.IsEnabled && sharedControl.IsEnabled
                    ? Element.TintColor.ToAndroid()
                    : Element.DisabledColor.ToAndroid();

                _rb?.SetTextColor(color);
                rb.SetTextColor(Element.SelectedTextColor.ToAndroid());
                _rb = rb;

                Element.SelectedSegment = radioId;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (nativeControl != null && nativeControl.Handle != IntPtr.Zero)
            {
                nativeControl.CheckedChange -= NativeControl_ValueChanged;
                nativeControl.Dispose();
                nativeControl = null;
                _rb = null;
            }

            if (Element != null) Element.ChildrenPropertyChanged -= SegmentedChildrenPropertyChanged;

            try
            {
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }
    }
}