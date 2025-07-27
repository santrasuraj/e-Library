using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Library
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        // sign up button click event
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkMemberExists())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Member Already Exist with this Member ID, try other ID',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Member Already Exist with this Member ID, try other ID');</script>");
            }
            else
            {
                signUpNewMember();
            }
        }

        // user defined method
        bool checkMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * from member_master_tbl where member_id='" + TextBox8.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'error',  title:'Oops...',  text:'" + ex.Message + "',  footer: '<a href='usersignup.aspx'>Why do I have this issue?</a>'});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }
        void signUpNewMember()
        {
            //Response.Write("<script>alert('Testing');</script>");
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl(full_name, dob, contact_name, email, state,city, pincode, full_address, member_id, password, account_status) VALUES(@full_name, @dob, @contact_name, @email, @state,@city, @pincode, @full_address, @member_id, @password, @account_status)", con);
                cmd.Parameters.AddWithValue("@full_name", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_name", TextBox3.Text.Trim()); // Matches contact_name in table
                cmd.Parameters.AddWithValue("@email", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@city", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@member_id",TextBox8.Text.Trim()); // Example implementation for member_id
                cmd.Parameters.AddWithValue("@password", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@account_status", "pending");

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'top-end',  icon: 'success',  title: 'Sign Up Successful. Go to User Login to Login',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Sign Up Successful. Go to User Login to Login');</script>");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'error',  title:'Oops...',  text:'" + ex.Message + "',  footer: '<a href='usersignup.aspx'>Why do I have this issue?</a>'});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}