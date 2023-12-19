using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace WebApp2
{
    

    public class DatabaseConnectionClass
    {
        SqlConnection? _databaseConnection;
        public bool connect(string server, string database, string username, string password) {
            string connectionString = "Server="+server+";Database="+database+";User Id="+username+";Password="+password+";";
            _databaseConnection = new SqlConnection(connectionString);
            _databaseConnection.Open();
            return false;
        }
        public bool connect(string connectionString) {
            _databaseConnection = new SqlConnection(connectionString);
            _databaseConnection.Open();
            return false;
        }

        //takes a command and returns a json string
        public string executeCommand(string Command) {
            string output = "[";
            string[] getnames;
            bool checkflipper = false;
            if (_databaseConnection == null) { return output + "]"; }
            SqlCommand command = new SqlCommand(Command, _databaseConnection);
            SqlDataReader reader = command.ExecuteReader();
            
            //gets the names of the colums
            getnames = new string[reader.FieldCount];
            for (int i = 0; i < getnames.Length; i++) {
                getnames[i] = reader.GetName(i);
            }

            //fills in json string from return type
            while (reader.Read()) {

                //if false, change flipper to true so it will add a comma only after the first loop.
                if (!checkflipper) { checkflipper = true; }
                else {
                    output += ",";
                }
                output += "{";
                for (int i = 0; i < getnames.Length; i++) {
                    output += "\"" + getnames[i] + "\":\"" + reader.GetValue(i).ToString()+"\"";
                    if (i < getnames.Length-1) {
                        output += ",";
                    }
                }
                output += "}";
            }
            return output + "]" ;
        }

        ~DatabaseConnectionClass() {
            if (_databaseConnection == null) { return; }
            _databaseConnection.Close();
        }

    }
}
