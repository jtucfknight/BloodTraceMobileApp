using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BloodTrace.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

        private void Tblogout_Clicked(object sender, EventArgs e)
        {
            Settings.UserName = "";
            Settings.Password = "";
            Settings.AccessToken = "";
            Navigation.InsertPageBefore(new SignInPage(), this);
            Navigation.PopAsync();
        }

        private void TapFindBlood_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindBloodPage());
        }

        private void TapRegisterBlood_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterBlood());
        }

        private void TapLatestDonars_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LatestDonars());
        }

        private void Help_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HelpPage());
        }
    }
}