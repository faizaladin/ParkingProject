using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class MakeRes : ContentPage
    {
        public MakeRes()
        {
            InitializeComponent();
        }

        public void goToSpot(object sender, EventArgs e)
        {
            string formatDate = date.Date.ToString("yyyy-MM-dd");
            string reserveDate = date.Date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            formatDate = formatDate.Trim();
            Shell.Current.Navigation.PushAsync(new Spots(formatDate, time1.Time, time2.Time, reserveDate));

        }

    }
}
