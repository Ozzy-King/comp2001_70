using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class profileController : ControllerBase
    {
        // GET: proflie
        [HttpGet(Name = "GetUser")]
        public string getUser(String email) {
            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.publicUserData where email = '" + email + "'";
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);


            //using (SqlConnection con = new SqlConnection(connectionString)) {
            //    con.Open();
            //    string commandString = "SELECT * FROM cw2.publicUserData where email = '"+email+"'";
            //    SqlCommand command = new SqlCommand(commandString, con);
            //    using (SqlDataReader reader = command.ExecuteReader()) {
            //        while (reader.Read()) {
            //            return reader.GetValue(1).ToString() + reader.GetValue(2).ToString();
            //        }
            //    }
            //    return "";
            //}
        }

    }
}
