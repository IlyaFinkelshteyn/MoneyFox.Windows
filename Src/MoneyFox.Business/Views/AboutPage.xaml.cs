using MoneyFox.Business.ViewModels;
using MvvmCross.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Business.Views
{
	[XamlCompilation(XamlCompilationOptions.Skip)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
		    SetheaderImage();

            BindingContext = Mvx.Resolve<AboutViewModel>();
        }

	    private void SetheaderImage()
	    {
	        switch (Device.RuntimePlatform)
	        {
	            case Device.iOS:
	                HeaderImage.Source = "Images/applylogo.png";
	                break;
	            case Device.Android:
	                HeaderImage.Source = "Images/applylogo.png";
	                break;
	            case Device.UWP:
	                HeaderImage.Source = "Assets/applylogo.png";
	                break;
	            default:
	                HeaderImage.Source = "applylogo.png";
	                break;
	        }
        }
	}
}