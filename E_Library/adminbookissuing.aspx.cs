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
    public partial class WebForm6 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        // issue book
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfBookExist() && checkIfMemberExist())
            {

                if (checkIfIssueEntryExist())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'error',  title:'Oops...',  text:'This Member already has this book',  footer: '<a href='adminmembermanagement.aspx'>Why do I have this issue?</a>'});", true);

                    //Response.Write("<script>alert('This Member already has this book');</script>");
                }
                else
                {
                    issueBook();
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'error',  title:'Oops...',  text:'Wrong Book ID or Member ID',  footer: '<a href='adminmembermanagement.aspx'>Why do I have this issue?</a>'});", true);
                //Response.Write("<script>alert('Wrong Book ID or Member ID');</script>");
            }
        }
        // return book
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfBookExist() && checkIfMemberExist())
            {

                if (checkIfIssueEntryExist())
                {
                    returnBook();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'This Entry does not exist',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('This Entry does not exist');</script>");
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'info',  title: 'Wrong Book ID or Member ID',  showConfirmButton: false,  timer: 1500});", true);
                //Response.Write("<script>alert('Wrong Book ID or Member ID');</script>");
            }
        }

        // go button click event
        protected void Button1_Click(object sender, EventArgs e)
        {
            getNames();
        }




        // user defined function

        void returnBook()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand("Delete from book_issue_tbl WHERE book_id='" + TextBox1.Text.Trim() + "' AND member_id='" + TextBox2.Text.Trim() + "'", con);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {

                    cmd = new SqlCommand("update book_master_tbl set current_stock = current_stock+1 WHERE book_id='" + TextBox1.Text.Trim() + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: '',  title: 'Book Returned Successfully',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Book Returned Successfully');</script>");
                    GridView1.DataBind();

                    con.Close();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Error - Invalid details',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Error - Invalid details');</script>");
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'error',  title:'Oops...',  text:'" + ex.Message + "',  footer: '<a href='adminbookissuing.aspx'>Why do I have this issue?</a>'});", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void issueBook()
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO book_issue_tbl(member_id,member_name,book_id,book_name,issue_date,due_date) values(@member_id,@member_name,@book_id,@book_name,@issue_date,@due_date)", con);

            cmd.Parameters.AddWithValue("@member_id", TextBox2.Text.Trim());
            cmd.Parameters.AddWithValue("@member_name", TextBox3.Text.Trim());
            cmd.Parameters.AddWithValue("@book_id", TextBox1.Text.Trim());
            cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
            cmd.Parameters.AddWithValue("@issue_date",TextBox5.Text.Trim());
            cmd.Parameters.AddWithValue("@due_date", TextBox6.Text.Trim());

            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("update  book_master_tbl set current_stock = current_stock-1 WHERE book_id='" + TextBox1.Text.Trim() + "'", con);

            cmd.ExecuteNonQuery();

            con.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'success',  title: 'Book Issued Successfully',  showConfirmButton: false,  timer: 1500});", true);
            //Response.Write("<script>alert('Book Issued Successfully');</script>");

            GridView1.DataBind();
            //try
            //{
               
            //}
            //catch (Exception ex)
            //{
            //    Response.Write("<script>alert('" + ex.Message + "');</script>");
            //}
        }

        bool checkIfBookExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from book_master_tbl WHERE book_id='" + TextBox1.Text.Trim() + "' AND current_stock >0", con);
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
                return false;
            }

        }

        bool checkIfMemberExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select full_name from member_master_tbl WHERE member_id='" + TextBox2.Text.Trim() + "'", con);
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
                return false;
            }

        }

        bool checkIfIssueEntryExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from book_issue_tbl WHERE member_id='" + TextBox2.Text.Trim() + "' AND book_id='" + TextBox1.Text.Trim() + "'", con);
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
                return false;
            }

        }



        void getNames()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select book_name from book_master_tbl WHERE book_id='" + TextBox1.Text.Trim() + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    TextBox4.Text = dt.Rows[0]["book_name"].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Wrong Book ID',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Wrong Book ID');</script>");
                }

                cmd = new SqlCommand("select full_name from member_master_tbl WHERE member_id='" + TextBox2.Text.Trim() + "'", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count >= 1)
                {
                    TextBox3.Text = dt.Rows[0]["full_name"].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({\r\n  position: 'middle-center',  icon: 'error',  title: 'Wrong User ID',  showConfirmButton: false,  timer: 1500});", true);
                    //Response.Write("<script>alert('Wrong User ID');</script>");
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Check your condition here
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
                    DateTime today = DateTime.Today;
                    if (today > dt)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "k", "Swal.fire({  icon: 'info',  title:'Oops...',  text:'" + ex.Message + "',  footer: '<a href='usersignup.aspx'>Why do I have this issue?</a>'});", true);

                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}