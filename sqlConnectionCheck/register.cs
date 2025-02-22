using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace sqlConnectionCheck
{
    public partial class register : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=");


        public register()
        {
            InitializeComponent();
        }

        private void register_Load(object sender, EventArgs e)
        {
            cboGender.Items.Add("Female");
            cboGender.Items.Add("Male");

        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (!this.emailTxt.Text.Contains('@') || !this.emailTxt.Text.Contains('.'))
            {
                MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (passwordTxt.Text != confirmTxt.Text)
            {
                    MessageBox.Show("Password doesn't match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(firstnameTxt.Text) || string.IsNullOrEmpty(lastnameTxt.Text) || string.IsNullOrEmpty(cboGender.Text) || string.IsNullOrEmpty(emailTxt.Text) || string.IsNullOrEmpty(usernameTxt.Text) || string.IsNullOrEmpty(passwordTxt.Text) || string.IsNullOrEmpty(confirmTxt.Text))
            {
                MessageBox.Show("Please fill out all information ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                connection.Open();

                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM loginform.userinfo WHERE Username = @UserName", connection),
                cmd2 = new MySqlCommand("SELECT * FROM loginform.userinfo WHERE Email = @UserEmail", connection);


                cmd1.Parameters.AddWithValue("@UserName", usernameTxt.Text);
                cmd2.Parameters.AddWithValue("@UserEmail", emailTxt.Text);

                bool userExists = false, mailExists = false;

                using (var dr1 = cmd1.ExecuteReader())
                    if (userExists = dr1.HasRows) MessageBox.Show("Username not available!");

                using (var dr2 = cmd2.ExecuteReader())
                    if (mailExists = dr2.HasRows) MessageBox.Show("Email not available!");


                if (!(userExists || mailExists))
                {

                    string iquery = "INSERT INTO loginform.userinfo(`ID`,`FirstName`,`LastName`,`Gender`,`Birthday`,`Email`,`Username`, `Password`) VALUES (NULL, '" + firstnameTxt.Text + "', '" + lastnameTxt.Text + "', '" + cboGender.Text + "', '" + dobTxt.Value.Date + "', '" + emailTxt.Text + "', '" + usernameTxt.Text + "', '" + passwordTxt.Text + "')";
                    MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                    commandDatabase.CommandTimeout = 60;

                    try
                    {
                        MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        // Show any error message.
                        MessageBox.Show(ex.Message);
                    }

                    MessageBox.Show("Account successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                connection.Close();
            }


        }

        private void backtoLoginBtn_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form1 frm4 = new Form1();
            frm4.ShowDialog();


        }
    }
}
