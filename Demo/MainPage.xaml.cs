using Xamarin.Forms;
using ValueChangedEventArgs = SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs;

namespace Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Title = "Segmented Control";

            ToolbarItems.Add(new ToolbarItem
                {Text = "Navigate", Command = new Command(obj => { Navigation.PushAsync(new SecondPage()); })});
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
    }
}