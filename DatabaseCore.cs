using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace point_of_sales_build_1._0
{
    public class DatabaseCore{

        SqlConnection con;
        SqlCommand cmd;
        public string errorMessage = null;
        int executeReturn;

        public DatabaseCore(){
            try {
                string connectionString = "Server=tcp:pointofsales.database.windows.net,1433;Initial Catalog=posDB;Persist Security Info=False;User ID=madusanka;Password=Adminpass15;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                con = new SqlConnection(connectionString);
                con.Open();

            }
            catch (Exception ex){
                errorMessage = "Something Went Wrong !";
            }

        }

        public int IDU_data(string Query){
            
            SqlCommand cmd = new SqlCommand(Query, con);
            executeReturn =  cmd.ExecuteNonQuery();
            con.Close();
            return executeReturn;
        }


        public SqlDataReader getData(string Query) {

                SqlDataReader reader = null;

                SqlCommand cmd = new SqlCommand(Query, con);
                reader = cmd.ExecuteReader();
                return reader;
        }


    }
}
