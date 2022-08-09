using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class ChangeRes : ContentPage
    {
        public Reservation temp;

        public ChangeRes(Reservation reservation)
        {
            InitializeComponent();
            temp = reservation;

        }

        public void goToSpot(object sender, EventArgs e)
        {
            string formatDate = date.Date.ToString("yyyy-MM-dd");
            string reserveDate = date.Date.ToString("yyyy-MM-dd HH:mm:ss");
            bool conflict = false;
            formatDate = formatDate.Trim();
            int totaltime1 = (int)(TimeSpan.Parse(temp.startime).Hours * 2) + (int)(TimeSpan.Parse(temp.startime).Minutes / 30);
            int totaltime2 = (int)(TimeSpan.Parse(temp.endtime).Hours * 2) + (int)(TimeSpan.Parse(temp.endtime).Minutes / 30);
            int totaltime3 = (int)(time1.Time.Hours * 2) + (int)(time1.Time.Minutes / 30);
            int totaltime4 = (int)(time2.Time.Hours * 2) + (int)(time2.Time.Minutes / 30);
            string initialDate = DateTime.Parse(temp.date).ToString("yyyy-MM-dd");
            if (initialDate == formatDate)
            {
                if ((totaltime1 <= totaltime3 && totaltime2 > totaltime3) ||
                    (totaltime1 <= totaltime4 && totaltime2 > totaltime4))
                {
                    conflict = true;
                }
            }
            Shell.Current.Navigation.PushAsync(new ChangeSpot(temp, formatDate, time1.Time, time2.Time, reserveDate, conflict));

        }
    }
}
