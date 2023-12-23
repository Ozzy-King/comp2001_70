using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace WebApp2.Controllers
{
    [Route("API/[controller]/[action]")]
    public class tagController : ControllerBase
    {
        //=================----tags----====================
        [HttpGet]
        public string getAllTag() {
            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.activityTag";
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpGet]
        public string getTagByID(int id) {
            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.activityTag where ID = " + id.ToString();
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpGet]
        public string getTagByName(String name) {
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (name.Length > 20)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("TagName", DatabaseConnectionClass.errorCodes.tooLong);
            }
            name = databaseConnection.sanatizer(name);
            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.activityTag where name = '" + name + "'"; //lookat names of the colums
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
    }
}
