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
    public partial class WebForm5 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        // add publisher
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Publisher Already Exist with this ID.',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Publisher Already Exist with this ID.');</script>");
            }
            else
            {
                addNewPublisher();
            }
        }
        // update publisher
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                updatePublisherByID();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Publisher with this ID does not exist',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Publisher with this ID does not exist');</script>");
            }
        }
        // delete publisher
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkPublisherExists())
            {
                deletePublisherByID();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Publisher with this ID does not exist',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Publisher with this ID does not exist');</script>");
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            getPublisherByID();
        }




        // user defined functions

        void getPublisherByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from publisher_master_tbl where publisher_id='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TextBox2.Text = dt.Rows[0][1].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Publisher with this ID does not exist',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Publisher with this ID does not exist.');</script>");
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");

            }
        }

        bool checkPublisherExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from publisher_master_tbl where publisher_id='" + TextBox1.Text.Trim() + "';", con);
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
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }
        }

        void addNewPublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO publisher_master_tbl(publisher_id,publisher_name) values(@publisher_id,@publisher_name)", con);

                cmd.Parameters.AddWithValue("@publisher_id", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher_name", TextBox2.Text.Trim());


                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Publisher added successfully',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Publisher added successfully.');</script>");
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        public void updatePublisherByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand("update publisher_master_tbl set publisher_name=@publisher_name WHERE publisher_id='" + TextBox1.Text.Trim() + "'", con);
                cmd.Parameters.AddWithValue("@publisher_name", TextBox2.Text.Trim());
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Publisher Updated Successfully',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Publisher Updated Successfully');</script>");
                    GridView1.DataBind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Publisher ID does not Exist',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Publisher ID does not Exist');</script>");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        public void deletePublisherByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand("Delete from publisher_master_tbl WHERE publisher_id='" + TextBox1.Text.Trim() + "'", con);
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Publisher Deleted Successfully',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Publisher Deleted Successfully');</script>");
                    GridView1.DataBind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Publisher ID does not Exist',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Publisher ID does not Exist');</script>");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


    }
}