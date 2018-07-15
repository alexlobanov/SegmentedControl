using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ValueChangedEventArgs = SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs;

namespace Demo
{
    public partial class SecondPage : ContentPage
    {
        public SecondPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ContentPageViewModel();
            ;
            Title = "Second Page";
        }

        public ContentPageViewModel viewModel { get; set; }

        protected override void OnAppearing()
        {
            viewModel.IsVisibleSegment = false;
            base.OnAppearing();
        }

        private void Handle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(new Label {Text = "Items tab selected"});
                    break;
                case 1:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(new Label {Text = "Notes tab selected"});
                    break;
                case 2:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(new Label {Text = "Approvers tab selected"});
                    break;
                case 3:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(new Label {Text = "Attachments tab selected"});
                    break;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}