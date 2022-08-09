using System;

using Xamarin.Forms;

namespace ParkingProject
{
    public static class Employee
    {
        public static string email { get; set; }
        public static string password { get; set; }
        public static int id { get; set; }
        public static long cellNum { get; set; }
        public static string firstName { get; set; }
        public static string lastName { get; set; }
        public static int spotID { get; set; }
        public static bool switchValue {get; set;}

        public static void clearInfo()
        {
            email = "";
            password = "";
            id = 0;
            cellNum = 0;
            firstName = "";
            lastName = "";
            spotID = 0;
            switchValue = false;
        }
    }

   

}

