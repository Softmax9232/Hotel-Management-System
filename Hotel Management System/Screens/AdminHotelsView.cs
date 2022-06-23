﻿using Hotel_Management_System.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Management_System.Controllers
{
    public partial class AdminHotelsView : Form
    {

        String query;
        DatabaseConnection dc = new DatabaseConnection();
        private int id;
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
        private int maxId;


        public AdminHotelsView()
        {
            InitializeComponent();
            hotelIdField.ReadOnly = false;
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SuperAdminLogin superAdmin = new SuperAdminLogin();
            superAdmin.Show();
        }

        private void populateTable()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            String query = "SELECT HotelId AS ID, Name, ContactNumber AS Contact, Email, Website, FloorCount AS Floors, TotalRooms AS Rooms, AddressLine AS Address, Street, City, Zip, Country FROM Hotels.Hotel ";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            hotelsTable.DataSource = ds.Tables[0];
            con.Close();
        }

        private String getDescription()
        {
            SqlConnection con = dc.getConnection();
            String str = "";
            con.Open();
            query = "SELECT Description from Hotels.Hotel WHERE HotelId = " + int.Parse(hotelIdField.Text);
            SqlCommand cmd = new SqlCommand(query, con);
            str = cmd.ExecuteScalar().ToString();
            con.Close();
            return str;
        }


        private void clearFields()
        {
            hotelIdField.Text = "";
            hotelNameField.Text = "";
            contactField.Text = "";
            zipField.Text = "";
            addressField.Text = "";
            cityField.Text = "";
            countryField.Text = "";
            capacityField.Text = "";
            floorCountField.Text = "";
            webField.Text = "";
            streetField.Text = "";
            descriptionFIeld.Text = "";
            emailField.Text = "";
        }

        private void HotelChainPage_Load(object sender, EventArgs e)
        {
            populateTable();
        }


        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            addButton.Enabled = false;
            if (hotelIdField.Text == "")
            {
                MessageBox.Show("Please enter id to search record.", "Missing Info", MessageBoxButtons.OK);
            }
            else
            {
                bool temp = false;
                SqlConnection con = dc.getConnection();
                con.Open();
                query = "SELECT * FROM Hotels.Hotel WHERE HotelId = " + int.Parse(hotelIdField.Text);
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
                    temp = true;
                }
                if (temp == false)
                    MessageBox.Show("No record found.");
                con.Close();
            }
        }

        private void hotelsTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            hotelIdField.Text = hotelsTable.SelectedRows[0].Cells[0].Value.ToString();
            hotelNameField.Text = hotelsTable.SelectedRows[0].Cells[1].Value.ToString();
            contactField.Text = hotelsTable.SelectedRows[0].Cells[2].Value.ToString();
            emailField.Text = hotelsTable.SelectedRows[0].Cells[3].Value.ToString();
            webField.Text = hotelsTable.SelectedRows[0].Cells[4].Value.ToString();
            floorCountField.Text = hotelsTable.SelectedRows[0].Cells[5].Value.ToString();
            capacityField.Text = hotelsTable.SelectedRows[0].Cells[6].Value.ToString();
            addressField.Text = hotelsTable.SelectedRows[0].Cells[7].Value.ToString();
            streetField.Text = hotelsTable.SelectedRows[0].Cells[8].Value.ToString();
            cityField.Text = hotelsTable.SelectedRows[0].Cells[9].Value.ToString();
            zipField.Text = hotelsTable.SelectedRows[0].Cells[10].Value.ToString();
            countryField.Text = hotelsTable.SelectedRows[0].Cells[11].Value.ToString();
            String str = getDescription();
            descriptionFIeld.Text = str;
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

        private bool checkUniqueName()
        {
            query = "SELECT HotelId FROM Hotels.Hotel WHERE Name = '" + hotelNameField.Text + "'";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
               id = dr.GetInt32(0);
            }
            if(id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkUniqueContact()
        {
            query = "SELECT HotelId FROM Hotels.Hotel WHERE ContactNumber = '" + contactField.Text + "'";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            if (id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        private bool checkUniqueEmail()
        {
            query = "SELECT HotelId FROM Hotels.Hotel WHERE Email = '" + emailField.Text + "'";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            if (id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkUniqueWeb()
        {
            query = "SELECT HotelId FROM Hotels.Hotel WHERE Website = '" + webField.Text + "'";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            if (id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkUniqueAddress()
        {
            query = "SELECT HotelId FROM Hotels.Hotel WHERE AddressLine = '" + addressField.Text + "'";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = dr.GetInt32(0);
            }
            if (id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkUnique()
        {
            bool c1 = checkUniqueName();
            if (c1 == false)
            {
                MessageBox.Show("Hotel with same name already exists.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hotelNameField.Focus();
                return false;
            }
            bool c2 = checkUniqueContact();
            if (c2 == false)
            {
                MessageBox.Show("A Hotel with same contact number already exists.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contactField.Focus();
                return false;
            }
            bool c3 = checkUniqueEmail();
            if (c3 == false)
            {
                MessageBox.Show("A Hotel with same email already exists.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                emailField.Focus();
                return false;
            }
            bool c4 = checkUniqueWeb();
            if (c4 == false)
            {
                MessageBox.Show("A Hotel with same website already exists.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                webField.Focus();
                return false;
            }
            bool c5 = checkUniqueAddress();
            if (c5 == false)
            {
                MessageBox.Show("A Hotel with same address already exists.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                addressField.Focus();
                return false;
            }
            return true;
        }

        private int getDescriptionLength()
        {
            return descriptionFIeld.TextLength;
        }

        private bool regexChecker()
        {
            if (!Regex.Match(hotelNameField.Text, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success)
            {
                MessageBox.Show("Hotel name can have only alpabets.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hotelNameField.Focus();
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
            if (!Regex.Match(contactField.Text, @"^[0-9]+$").Success)
            {
                MessageBox.Show("Contact number must only contain numbers.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contactField.Focus();
                return false;
            }
            return true;
        }

        private void addButton_Click_2(object sender, EventArgs e)
        {
            if (hotelNameField.Text != "" && contactField.Text != "" && zipField.Text != "" && addressField.Text != "" &&
                 cityField.Text != "" && countryField.Text != "" && webField.Text != "" && emailField.Text != "" && capacityField.Text != "" &&
                 floorCountField.Text != "" && streetField.Text != "" && descriptionFIeld.Text != "" && emailField.Text != "")
            {
                bool check = checkUnique();
                if(check == false)
                {
                    return;
                }
                bool regCheck = regexChecker();
                if (regCheck == false)
                {
                    return;
                }
                bool emailCheck = IsValid(emailField.Text);
                if (emailCheck == false)
                {
                    MessageBox.Show("Invalid email format entered.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                query = "INSERT INTO Hotels.Hotel (Name, ContactNumber, Email, Website, Description, FloorCount, TotalRooms, AddressLine, Street, City, Zip, Country) VALUES ('" + name + "' , '" + contact + "', '" + email + "' , '" + web + "' , '" + description + "' , " + floors + ", " + capacity + ", '" + address + "' , '" + street + "' , '" + city + "' , '" + zip + "', '" + country + "')";
                dc.setData(query, "Hotel inserted successfully!");
                getRecentHotelId();
                Statics.hotelNameId(maxId, name);
                Statics.setHotelId(maxId);
                //sendMail();
                clearFields();
                populateTable();
                ShowDefaultScreen defaultScreen = new ShowDefaultScreen();
                defaultScreen.Show();
            }
            else
            {
                MessageBox.Show("All fields must be filled.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void sendMail()
        {
            String fromAddress = "";
            String toAddress = emailField.Text;
            String password = "";

            MailMessage mail = new MailMessage();

            mail.Subject = "Hotel default username and password.";
            mail.From = new MailAddress(fromAddress);
            ShowDefaultScreen sds = new ShowDefaultScreen();
            String uName = sds.getUsername();
            String uPass = sds.getPassword();
            mail.Body = "Hello, Here is your hotel username: " + uName + " and password: " + uPass;
            mail.To.Add(new MailAddress(toAddress));

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;

            NetworkCredential nec = new NetworkCredential(fromAddress, password);
            smtp.Credentials = nec;
            smtp.Send(mail);

            MessageBox.Show("Account details send successfully.");


        }

        private void getRecentHotelId()
        {
            query = "SELECT MAX(HotelId) FROM Hotels.Hotel";
            SqlConnection con = dc.getConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                maxId = dr.GetInt32(0);
            }

        }

        private void deleteRoomType()
        {
            query = "DELETE FROM Rooms.RoomType WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void deleteRooms()
        {
            query = "DELETE FROM Rooms.Room WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void deleteHotelLogin()
        {
            query = "DELETE FROM Authentication.Login WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void clearButton_Click_1(object sender, EventArgs e)
        {
            addButton.Enabled = true;
            clearFields();
        }

        private void deleteRoomsBooked()
        {
            query = "DELETE FROM Rooms.RoomBooked WHERE RoomId IN (SELECT RoomId FROM Rooms.Room WHERE HotelId = " + hotelIdField.Text + ")";
            dc.setData(query, "");
        }

        private void deleteServicesUsed()
        {
            query = "DELETE FROM HotelService.ServicesUsed WHERE BookingId IN (SELECT BookingId FROM Bookings.Booking WHERE HotelId = " + hotelIdField.Text + ")";
            dc.setData(query, "");
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            addButton.Enabled = true;
            if (hotelIdField.Text == "")
            {
                MessageBox.Show("Please enter id to delete.", "Missing Info", MessageBoxButtons.OK);
            }
            else
            {
                deleteHotelLogin();
                deleteDepartments();
                deleteServicesUsed();
                deleteRoomsBooked();
                deleteRooms();
                deleteRoomType();
                deleteHotelBookings();
                deleteEmployees();
                query = "DELETE FROM Hotels.Hotel WHERE HotelId = " + int.Parse(hotelIdField.Text);
                dc.setData(query, "Record deleted successfully.");
                clearFields();
                populateTable();
            }
        }

        private void deleteHotelBookings()
        {
            query = "DELETE FROM Bookings.Booking WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void deleteEmployees()
        {
            query = "DELETE FROM Hotels.Employees WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void deleteDepartments()
        {
            query = "DELETE FROM Hotels.Departments WHERE HotelId = " + int.Parse(hotelIdField.Text);
            dc.setData(query, "");
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            addButton.Enabled = true;
            if (hotelIdField.Text == "")
            {
                MessageBox.Show("Please enter id to update record.", "Missing Info", MessageBoxButtons.OK);
            }
            else
            {
                bool regCheck = regexChecker();
                if (regCheck == false)
                {
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
                query = "UPDATE Hotels.Hotel SET Name = '" + name + "', ContactNumber = '" + contact + "', Email= '" + email + "', Website = '" + web + "', Description = '" + description + "', FloorCount = " + floors + ", TotalRooms = " + capacity + ", AddressLine = '" + address + "', Street = '" + street + "', City = '" + city + "' , Country = '" + country + "', Zip = '" + zip + "' WHERE HotelId = " + int.Parse(hotelIdField.Text);
                dc.setData(query, "Record updated successfully.");
                clearFields();
                populateTable();
            }
        }
    }
}
