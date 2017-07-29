using Windows.UI.Xaml;
using MoneyFox.Business.Views;
using Xamarin.Forms;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     Displays the about of this app.
    /// </summary>
    public sealed partial class AboutView
    {
        private readonly AboutPage aboutPage; 

        /// <summary>
        ///     Constructor
        /// </summary>
        public AboutView()
        {
            InitializeComponent();
            
            aboutPage = new AboutPage();
            ContentGrid.Children.Add(aboutPage.CreateFrameworkElement());
        }

        private void AboutView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new AboutPage().CreateFrameworkElement());
        }
    }
}