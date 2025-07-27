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
    public partial class userlogin : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // user login
       

        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id='" + TextBox1.Text.Trim() + "' AND password='" + TextBox2.Text.Trim() + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  position: 'top-end',  icon: 'success',  title: '" + dr.GetValue(8).ToString() + "',  showConfirmButton: false,  timer: 1500});", true);
                        //Response.Write("<script>alert('" + dr.GetValue(8).ToString() + "');</script>");
                        Session["username"] = dr.GetValue(8).ToString();
                        Session["fullname"] = dr.GetValue(2).ToString();
                        Session["role"] = "user";
                        //Session["status"] = dr.GetValue(10).ToString();
                      
                    }
                    Response.Redirect("HomePage.aspx");
                }

                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Invalid credentials, try other ID',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Invalid credentials');</script>");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'info',  title:'Oops...',  text:'" + ex.Message + "',  footer: '<a href='HomePage.aspx'>Why do I have this issue?</a>'});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}