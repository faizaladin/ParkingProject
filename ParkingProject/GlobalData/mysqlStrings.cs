using System;
using Xamarin.Forms;

namespace ParkingProject
{
    public static class mysqlStrings
    {
        public static string ConnectionString = "Server = 192.168.1.33; User ID = faiz; Password=faiz; Database=ParkingDB";

        public static string GenerateRegisterCommand(string firstName, string lastName, string email, long cellphone, string password, string salt)
        {
            string command = "INSERT INTO Employee(email, firstname, lastname, cellnum, role, password, salt) " +
                "VALUES ('" + email + "'," + "'" + firstName + "'" + "," + "'" + lastName + "'" + "," + cellphone + ", " +
                "'U' ," + "'" + password + "', '" + salt + "')";

            return command;
        }

        public static string GenerateLoginCommand(string email, string password)
        {
            string command = "SELECT count(*) FROM Employee WHERE email='" + email + "' AND password=" + "'" + password + "'";

            return command;
        }

        public static string GenerateRetrieveEmployeeInfoCommand(string email, string password)
        {
            string command = "SELECT email, employeeID, firstname, lastname, cellnum, password FROM Employee" +
                " WHERE email = '" + email + "' AND password = '" + password + "'";

            return command;
        }

        public static string GenerateRetrievePermanentSpotCommand(int employeeID)
        {
            string command = "SELECT spotID FROM spots WHERE employeeID=" + employeeID + " AND spot_type='P'";

            return command;
        }

        public static string GenerateAvailableSpotsCommand(string date)
        {

            string command = "SELECT spotID, availability FROM available_spots " +
                "WHERE date=CONVERT(" + "'" + date + "'" + ", DATE)";

            return command;
        }

        public static string GenerateGetSpotAvailabilityCommand(int spotID, string date)
        {

            string command = "SELECT availability FROM available_spots " +
                "WHERE spotID=" + spotID + " AND date=CONVERT(" + "'" + date + "'" + ", DATE)";

            return command;
        }

        public static string GenerateAddReservationCommand(int employeeID, int spotID, DateTime date, TimeSpan timeStart, TimeSpan timeEnd)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string formattedTimeStart = timeStart.ToString("hh':'mm");
            string formattedTimeEnd = timeEnd.ToString("hh':'mm");

            string command = "INSERT INTO reservation(employeeID, spotID, date, start_time, end_time) " +
                "VALUES(" + employeeID + "," + spotID + ", CONVERT(" + "'" + formattedDate + "'" + ", DATE)" +
                ", CONVERT(" + "'" + formattedTimeStart + "'" + ", TIME)" +
                ", CONVERT(" + "'" + formattedTimeEnd + "'" + ", TIME)" + ")";

            return command;
        }

        public static string GenerateUpdateAvailableSpotsCommand(int spotID, DateTime date, string availability)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");

            string command = "UPDATE available_spots SET availability='" + availability +
                "' WHERE spotID=" + spotID + " AND date=CONVERT(" + "'" + formattedDate + "'" + ", DATE)";

            return command;
        }

        public static string GenerateSwitchCommand(int spotID)
        {
            string command = "Select count(*) from available_spots where spotID = " + spotID;
            return command;
        }
        public static string GenerateGetCurrentReservations(int spotID)
        {
            string command = "SELECT * FROM reservation WHERE spotID=" + spotID;

            return command;
        }
        public static string GenerateMyReservations(int employeeID)
        {
            string command = "select * from reservation where employeeID = " + employeeID;

            return command;
        }

        public static string GenerateUpdateCommand(string colName, string value)
        {
            string command;
            try {
                long colValue = long.Parse(value);
                command = "Update employee SET '" + colName + "' =" + colValue + " where employeeID = " + Employee.id;
            }
            catch {
                string colValue = value;
                command = "Update employee SET '" + colName + "' = '" + colValue + "' where employeeID = " + Employee.id;
            }

            return command;
            
        }
    }
}

