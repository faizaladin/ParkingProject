using System;

using Xamarin.Forms;

namespace ParkingProject

{
    public class Reservation
    {
        public int employeeID { get; set; }
        public int spotID { get; set; }
        public string date { get; set; }
        public string startime { get; set; }
        public string endtime { get; set; }
        public string displayname { get; set;}

    }
}
