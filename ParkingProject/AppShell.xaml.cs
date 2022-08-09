using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class AppShell : Shell, INotifyPropertyChanged
    {



        public AppShell()
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(this, false);
            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            Routing.RegisterRoute(nameof(MakeRes), typeof(MakeRes));
            Routing.RegisterRoute(nameof(Spots), typeof(Spots));
            Routing.RegisterRoute(nameof(ManageSpot), typeof(ManageSpot));
            Routing.RegisterRoute(nameof(Waitlist), typeof(Waitlist));
        }

        private async void logout(Object sender, EventArgs e)
        {
            Employee.clearInfo();
            await Shell.Current.Navigation.PopToRootAsync(false);
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        private async void gotoaccount(Object sender, EventArgs e)
        {

        }

        private void gotoManageSpot(Object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(ManageSpot)}");
        }

        private void gotoWaitlist(Object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(Waitlist)}");
        }
    }
}
