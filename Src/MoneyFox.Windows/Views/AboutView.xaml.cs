using MoneyFox.Business.ViewModels;
using MoneyFox.Business.Views;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     Displays the about of this app.
    /// </summary>
    public sealed partial class AboutView
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public AboutView()
        {
            InitializeComponent();
            DataContext = Mvx.Resolve<AboutViewModel>();

            var aboutPage = new AboutPage();
            ContentGrid.Children.Add(aboutPage.CreateFrameworkElement());
        }
    }
}