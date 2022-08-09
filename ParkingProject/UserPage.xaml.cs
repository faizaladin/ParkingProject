using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ParkingProject
{

    public partial class UserPage : ContentPage
    {

        public UserPage()
        {
            InitializeComponent();

        }
        

        public void clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(MakeRes)}");
        }

        public void goToManageSpot(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(ManageSpot)}");
        }

        
    }
}
