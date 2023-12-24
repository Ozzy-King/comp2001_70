using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp2.Controllers
{
    [Route("API/[controller]/[action]")]
    [ApiController]
    public class locationController : ControllerBase
    {
        //===================----location----=====================
        //get for location
        [HttpGet(Name = "getAllLocation")]
        public string getAllLocation()
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.location";
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        //dont need to sanatize due to it being an integer and not a string
        [HttpGet(Name = "getLocationByID")]
        public string getLocationByID(int id)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.location where ID = " + id.ToString();
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
        
        [HttpGet(Name = "getLocationByName")]
        public string getLocationByName(string name)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (name.Length > 200)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("LocationName", DatabaseConnectionClass.errorCodes.tooLong);
            }
            name = databaseConnection.sanatizer(name);
            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.location where name = '" + name + "'"; //lookat names of the colums
            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }


    }
}
