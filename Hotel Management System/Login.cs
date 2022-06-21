﻿using Hotel_Management_System.Controllers;
using Hotel_Management_System.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Management_System
{
    public partial class Login : Form
    {

        DatabaseConnection dc = new DatabaseConnection();
        String query;
        public int hotelIdToken;
        public int employeeIdToken;

        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private String checkNewUser()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT NewUser FROM Authentication.Login WHERE username = '" + usernameTextField.Text + "' AND password = '" + passwordTextField.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            String str = "";
            while (dr.Read())
            {
                str = dr.GetString(0);
            }
            con.Close();
            return str;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            String isNewUser = checkNewUser().Trim();
            query = "SELECT LoginId FROM Authentication.Login WHERE Username = @username AND Password = @password";
            SqlConnection connection = dc.getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", usernameTextField.Text);
            cmd.Parameters.AddWithValue("@password", passwordTextField.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            TokenHotelIdHOTEL();
            Statics.setHotelId(hotelIdToken);
            TokenEployeeId();
            Statics.setEmployeeId(employeeIdToken);
            if (String.IsNullOrEmpty(usernameTextField.Text) || String.IsNullOrEmpty(passwordTextField.Text))
            {
                errorLabel.Text = "        All fields are required.";
                errorLabel.Visible = true;
            }
            else if (!reader.HasRows)
            {
                errorLabel.Text = "Incorrect username or password.";
                errorLabel.Visible = true;
            }
            else if(reader.HasRows && isNewUser.Equals("Yes"))
            {
                Statics.setUname(usernameTextField.Text);
                Statics.setPass(passwordTextField.Text);
                CreatePassword reset = new CreatePassword();
                reset.Show();
                this.Hide();
            }
            else if (reader.HasRows && isNewUser.Equals("NO"))
            {
                errorLabel.Visible = false;
                this.Hide();
                Dashboard db = new Dashboard();
                db.Show();
            }
            connection.Close();
        }

        private void TokenEployeeId()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT EmployeeId FROM Authentication.Login WHERE username = '" + usernameTextField.Text + "' AND password = '" + passwordTextField.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if(dr.GetValue(0) != DBNull.Value)
                {
                    employeeIdToken = dr.GetInt32(0);
                }
            }
        }

        private void TokenHotelIdHOTEL()
        {
            SqlConnection con = dc.getConnection();
            con.Open();
            query = "SELECT HotelId FROM Authentication.Login WHERE Username = '" + usernameTextField.Text + "' AND Password = '" + passwordTextField.Text + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                hotelIdToken = dr.GetInt32(0);
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            SuperAdminLogin superAdmin = new SuperAdminLogin();
            superAdmin.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if(usernameTextField.Text == "")
            {
                MessageBox.Show("Please enter username.", "Missing Info", MessageBoxButtons.OK);
            }
            else
            {
                query = "SELECT LoginId FROM Authentication.Login WHERE Username = @username";
                SqlConnection connection = dc.getConnection();
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", usernameTextField.Text);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Statics.setTempUname(usernameTextField.Text);
                    this.Hide();
                    ResetPassword rp = new ResetPassword();
                    rp.Show();
                }
                else
                {
                    MessageBox.Show("Username not found.", "Incorrect Info", MessageBoxButtons.OK);
                }
            }
        }

        private void passwordTextField_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void changeVisibile(object sender, EventArgs e)
        {
            Image myimage1 = new Bitmap(@"C:\Users\Ali Asar\source\repos\Hotel Management System\Hotel Management System\Icons\eyevisoff.png");
            Image myimage2 = new Bitmap(@"C:\Users\Ali Asar\source\repos\Hotel Management System\Hotel Management System\Icons\eyevisible.png");

            if (passwordTextField.UseSystemPasswordChar == true)
            {
                passwordTextField.UseSystemPasswordChar = false;
                passwordTextField.IconRight = myimage2;
            }
            else if(passwordTextField.UseSystemPasswordChar == false)
            {
                passwordTextField.UseSystemPasswordChar = true;
                passwordTextField.IconRight = myimage1;
            }
        }
    }
}
