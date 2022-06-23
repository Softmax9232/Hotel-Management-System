﻿using Hotel_Management_System.Screens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Management_System.Controllers
{
    public partial class HotelsScreen : Form
    {
        String query;
        DatabaseConnection dc = new DatabaseConnection();

        private String name;
        private String contact;
        private String zip;
        private String address;
        private String city;
        private String country;
        private String web;
        private int capacity;
        private int floors;
        private String street;
        private String description;
        private String email;
        public HotelsScreen()
        {
            InitializeComponent();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void checkIfEmployee()
        {
            if (!Statics.employeeIdTKN.Equals(0))
            {
                p1.Hide();
            }
        }

        private void HotelsScreen_Load(object sender, EventArgs e)
        {
            checkIfEmployee();
            getHotelDetails();
            getEmpCounts();
            getBookingCount();
            getRoomsCount();
            getTotalEarning();
        }

        private void getFieldsData()
        {
            name = hotelNameField.Text;
            contact = contactField.Text;
            zip = zipField.Text;
            address = addressField.Text;
            city = cityField.Text;
            country = countryField.Text;
            web = webField.Text;
            capacity = int.Parse(capacityField.Text);
            floors = int.Parse(floorCountField.Text);
            street = streetField.Text;
            description = descriptionFIeld.Text;
            email = emailField.Text;
        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool regexChecker()
        {
            if (!Regex.Match(hotelNameField.Text, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success)
            {
                MessageBox.Show("Hotel name can have only alpabets.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contactField.Focus();
                return false;
            }
            if (!Regex.Match(contactField.Text, @"^[0-9]+$").Success)
            {
                MessageBox.Show("Contact number must only contain numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contactField.Focus();
                return false;
            }
            if (!Regex.Match(zipField.Text, @"^\d{5}$").Success)
            {
                MessageBox.Show("Zipcode must only contain numbers with length of 5.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                zipField.Focus();
                return false;
            }
            if (!Regex.Match(cityField.Text, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success)
            {
                MessageBox.Show("City field must contain alpabets or space only.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cityField.Focus();
                return false;
            }
            if (!Regex.Match(countryField.Text, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success)
            {
                MessageBox.Show("Country field must contain alpabets or space only.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                countryField.Focus();
                return false;
            }
            if (!Regex.Match(capacityField.Text, @"^[0-9]+$").Success)
            {
                MessageBox.Show("Capacity field can only have numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                capacityField.Focus();
                return false;
            }
            if (!Regex.Match(floorCountField.Text, @"^[0-9]+$").Success)
            {
                MessageBox.Show("Floors field can only have numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                floorCountField.Focus();
                return false;
            }
            return true;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (hotelNameField.Text == "" || contactField.Text == "" || zipField.Text == "" || addressField.Text == "" ||
                 cityField.Text == "" || countryField.Text == "" || webField.Text == "" || emailField.Text == "" || capacityField.Text == "" ||
                 floorCountField.Text == "" || streetField.Text == "" || descriptionFIeld.Text == "" || emailField.Text == "")
            {
                MessageBox.Show("Please fill all fields to update.", "Missing Info", MessageBoxButtons.OK);
            }
            else
            {
                bool regCheck = regexChecker();
                if (regCheck == false)
                {
                    return;
                }
                bool emailCheck = IsValid(emailField.Text);
                if(emailCheck == false)
                {
                    MessageBox.Show("Please enter valid email.", "Missing Info", MessageBoxButtons.OK);
                    return;
                }
                int a = getDescriptionLength();
                if (a > 300)
                {
                    MessageBox.Show("Description length cannot be greater than 50.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    descriptionFIeld.Focus();
                    return;
                }
                getFieldsData();
                query = "UPDATE Hotels.Hotel SET Name = '" + name + "', ContactNumber = '" + contact + "', Email= '" + email + "', Website = '" + web + "', Description = '" + description + "', FloorCount = " + floors + ", TotalRooms = " +  capacity + ", AddressLine = '" + address + "', Street = '" + street + "', City = '" + city + "' , Country = '" + country + "', Zip = '" + zip + "' WHERE HotelId = " + Statics.hotelIdTKN;
                dc.setData(query, "Record updated successfully.");
                getHotelDetails();
            }
        }

        private int getDescriptionLength()
        {
            return descriptionFIeld.TextLength;
        }

        private void getHotelDetails()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT * FROM Hotels.Hotel WHERE HotelId = " + Statics.hotelIdTKN;
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                hotelNameField.Text = dr.GetString(1);
                contactField.Text = dr.GetString(2);
                emailField.Text = dr.GetString(3);
                webField.Text = dr.GetString(4);
                descriptionFIeld.Text = dr.GetString(5);
                floorCountField.Text = dr.GetSqlInt32(6).ToString();
                capacityField.Text = dr.GetSqlInt32(7).ToString();
                addressField.Text = dr.GetString(8);
                streetField.Text = dr.GetString(9);
                cityField.Text = dr.GetString(10);
                zipField.Text = dr.GetString(11);
                countryField.Text = dr.GetString(12);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DiscountScreen ds = new DiscountScreen();
            ds.ShowDialog(this);
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void getEmpCounts()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT COUNT(EmployeeId) from Hotels.Employees WHERE HotelId = " + Statics.hotelIdTKN;

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            empLabel.Text = id.ToString();
        }

        private void getRoomsCount()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT COUNT(RoomId) from Rooms.Room WHERE HotelId = " + Statics.hotelIdTKN;

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            roomsLabel.Text = id.ToString();
        }

        private void getBookingCount()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT COUNT(BookingId) from Bookings.Booking WHERE HotelId = " + Statics.hotelIdTKN;

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            bookingLabel.Text = id.ToString();
        }

        private void getTotalEarning()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT SUM(PaymentAmount) FROM Bookings.Payments WHERE BookingId IN (SELECT BookingId FROM Bookings.Booking WHERE HotelId =" + Statics.hotelIdTKN +  ")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
            }
            earningLabel.Text = id.ToString();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            makeFile();
        }

        // Create empty excel file first then provide the directory.
        private void makeFile()
        {
            checkIfExist();
            createEmptyFile();
            var file = new FileInfo(@"C:\Users\Ali Asar\Desktop\Hotel Report\Report.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(file))
            {
                query = "SELECT Bookings.Booking.BookingId, Bookings.Booking.StayDuration, Bookings.Booking.Status, Hotels.Guests.GuestId, Hotels.Guests.GuestFirstName, Hotels.Guests.GuestLastName, Hotels.Guests.GuestContactNumber, Hotels.Guests.GuestPassportNumber, Hotels.Guests.HotelId, Hotels.Employees.EmployeeId, Hotels.Employees.EmployeeFirstName, Hotels.Employees.EmployeeLastName, Hotels.Employees.EmployeeContactNumber, Bookings.Payments.PaymentId, Bookings.Payments.PaymentStatus,Bookings.Payments.PaymentAmount FROM Bookings.Booking INNER JOIN Hotels.Employees ON Bookings.Booking.EmployeeId = Hotels.Employees.EmployeeId FULL JOIN Hotels.Guests ON Bookings.Booking.GuestId = Hotels.Guests.GuestId FULL JOIN Bookings.Payments ON Bookings.Booking.BookingId = Bookings.Payments.BookingId WHERE Bookings.Booking.HotelId = " + Statics.hotelIdTKN;
                ExcelWorksheet sheet = excel.Workbook.Worksheets["sheet1"];
                SqlConnection con = dc.getConnection();
                con.Open();
                var command = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int count = dt.Rows.Count;
                sheet.Cells.LoadFromDataTable(dt,true);
                FileInfo excelFile = new FileInfo(@"C:\Users\Ali Asar\Desktop\Hotel Report\Report.xlsx");
                excel.SaveAs(excelFile);
                MessageBox.Show("Excel file is downloaded on desktop.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void createEmptyFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var app = new Microsoft.Office.Interop.Excel.Application();
            var wb = app.Workbooks.Add();
            wb.SaveAs(@"C:\Users\Ali Asar\Desktop\Hotel Report\Report.xlsx");
            wb.Close();
        }

        private void checkIfExist()
        {
            FileInfo file = new FileInfo(@"C:\Users\Ali Asar\Desktop\Hotel Report\Report.xlsx");
            if (file.Exists)
            {
                Console.WriteLine("Yes");
                file.Delete();
            }
        }
    }
}
