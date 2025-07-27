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
    public partial class WebForm4 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
        // add button click
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Author with this ID already Exist. You cannot add another Author with the same Author ID',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Author with this ID already Exist. You cannot add another Author with the same Author ID');</script>");
            }
            else
            {
                addNewAuthor();
            }
        }
        // update button click
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
            {
                updateAuthor();

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: ',  title: 'Author does not exist',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Author does not exist');</script>");
            }
        }
        // delete button click
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
            {
                deleteAuthor();

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Author does not exist',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Author does not exist');</script>");
            }
        }
        // GO button click
        protected void Button1_Click(object sender, EventArgs e)
        {
            getAuthorByID();
        }



        // user defined function
        void getAuthorByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from author_master_tbl where author_id='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TextBox2.Text = dt.Rows[0][1].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Invalid Author ID',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Invalid Author ID');</script>");
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");

            }
        }


        void deleteAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("DELETE from author_master_tbl WHERE author_id='" + TextBox1.Text.Trim() + "'", con);

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "l", "Swal.fire({\r\n  title: \'Are you sure?\',\r\n  text: \'You won't be able to revert this!\',\r\n  icon: \'warning\',\r\n  showCancelButton: true,\r\n  confirmButtonColor: \'#3085d6\',\r\n  cancelButtonColor: \'#d33\',\r\n  confirmButtonText: \'Yes, delete it!\'\r\n}).then((result) => {\r\n  if (result.isConfirmed) {\r\n    Swal.fire({\r\n      title: \'Deleted!\',\r\n      text: \'Member Deleted Successfully.\',\r\n      icon: \'success\'\r\n    });\r\n  }\r\n});", true);
                //Response.Write("<script>alert('Author Deleted Successfully');</script>");
                clearForm();
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void updateAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("UPDATE author_master_tbl SET author_name=@author_name WHERE author_id='" + TextBox1.Text.Trim() + "'", con);

                cmd.Parameters.AddWithValue("@author_name", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Author Updated Successfully',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Author Updated Successfully');</script>");
                clearForm();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


        void addNewAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO author_master_tbl(author_id,author_name) values(@author_id,@author_name)", con);

                cmd.Parameters.AddWithValue("@author_id", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@author_name", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Author added Successfully',  showConfirmButton: false,  timer: 1500});", true);

                //Response.Write("<script>alert('Author added Successfully');</script>");
                clearForm();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: '" + ex.Message + "',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }



        bool checkIfAuthorExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from author_master_tbl where author_id='" + TextBox1.Text.Trim() + "';", con);
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

        void clearForm()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
        }
    }
}