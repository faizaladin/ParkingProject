using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class MyAccount : ContentPage
    {
        public MyAccount()
        {
            InitializeComponent();
            LoadInfo();

        }

        public void LoadInfo()
        {
            ID.Text = Employee.email;
            First.Text = Employee.firstName;
            Last.Text = Employee.lastName;
            Cell.Text = Employee.cellNum.ToString();
        }

        public async void updateInfo(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new AccountUpdate());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ID.Text = Employee.email;
            First.Text = Employee.firstName;
            Last.Text = Employee.lastName;
            Cell.Text = Employee.cellNum.ToString();
        }
    }
}
