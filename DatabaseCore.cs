using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace point_of_sales_build_1._0
{
    public class DatabaseCore{

        SqlConnection con;
        SqlCommand cmd;
        public string errorMessage = null;
        int executeReturn;

        public DatabaseCore(){
            bool test = TcpSocketTest();
            if (test == true)
            {
                try
                {
                    string connectionString = "Server=tcp:pointofsales.database.windows.net,1433;Initial Catalog=posDB;Persist Security Info=False;User ID=madusanka;Password=Adminpass15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                    con = new SqlConnection(connectionString);
                    con.Open();

                }
                catch (Exception ex)
                {
                    errorMessage = "Something Went Wrong !";
                }
            }
            else { MessageBox.Show(errorMessage = "Please check your internet connection");   }

        }

        public int IDU_data(string Query){
            
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(Query, con);
            executeReturn =  cmd.ExecuteNonQuery();
            
            con.Close();
            return executeReturn;
        }


        public SqlDataReader getData(string Query) {
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(Query, con);
                reader = cmd.ExecuteReader();
             
                return reader;
        }

        public string getSingleData(string Query)
        {
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand(Query, con);
            string get = cmd.ExecuteScalar().ToString();
            con.Close();
            return get;
        }
        public int updateTableData(string Query) 
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            executeReturn = cmd.ExecuteNonQuery();
            return executeReturn;
        }
       

        public static bool TcpSocketTest()
        {
            try
            {
                System.Net.Sockets.TcpClient client =
                    new System.Net.Sockets.TcpClient("www.google.com", 80);
                client.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

    }

}
