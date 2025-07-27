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
    public partial class WebForm7 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
        // Go button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            getMemberByID();
        }
        // Active button
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("active");
           TextBox7 .Text = "active";
        }
        // pending button
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("pending");
            TextBox7.Text = "pending";
        }
        // deactive button
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            updateMemberStatusByID("deactive");
            TextBox7.Text = "deactive";
        }
        // delete button
        protected void Button2_Click(object sender, EventArgs e)
        {
            deleteMemberByID();
        }





        // user defined function

        bool checkIfMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from member_master_tbl where member_id='" + TextBox1.Text.Trim() + "';", con);
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
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }

        void deleteMemberByID()
        {
            if (checkIfMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE from member_master_tbl WHERE member_id='" + TextBox1.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "l", "Swal.fire({\r\n  title: \'Are you sure?\',\r\n  text: \'You won't be able to revert this!\',\r\n  icon: \'warning\',\r\n  showCancelButton: true,\r\n  confirmButtonColor: \'#3085d6\',\r\n  cancelButtonColor: \'#d33\',\r\n  confirmButtonText: \'Yes, delete it!\'\r\n}).then((result) => {\r\n  if (result.isConfirmed) {\r\n    Swal.fire({\r\n      title: \'Deleted!\',\r\n      text: \'Member Deleted Successfully.\',\r\n      icon: \'success\'\r\n    });\r\n  }\r\n});", true);
                    //Response.Write("<script>alert('Member Deleted Successfully');</script>");
                    clearForm();
                    GridView1.DataBind();

                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('" + ex.Message + "');</script>");
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Invalid Member ID',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Invalid Member ID');</script>");
            }
        }

        void getMemberByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                SqlCommand cmd = new SqlCommand("select * from member_master_tbl where member_id='" + TextBox1.Text.Trim() + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TextBox2.Text = dr.GetValue(0).ToString();
                        TextBox7.Text = dr.GetValue(10).ToString();
                        TextBox8.Text = dr.GetValue(1).ToString();
                        TextBox3.Text = dr.GetValue(2).ToString();
                        TextBox4.Text = dr.GetValue(3).ToString();
                        TextBox9.Text = dr.GetValue(4).ToString();
                        TextBox10.Text = dr.GetValue(5).ToString();
                        TextBox11.Text = dr.GetValue(6).ToString();
                        TextBox6.Text = dr.GetValue(7).ToString();

                    }

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Invalid credentials',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Invalid credentials');</script>");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void updateMemberStatusByID(string status)
        {
            if (checkIfMemberExists())
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl SET account_status='" + status + "' WHERE member_id='" + TextBox1.Text.Trim() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Member Status Updated',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Member Status Updated');</script>");


                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Invalid Member ID',  showConfirmButton: false,  timer: 1500});", true);
                Response.Write("<script>alert('Invalid Member ID');</script>");
            }

        }

        void clearForm()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
            TextBox6.Text = "";
        }

    }
}