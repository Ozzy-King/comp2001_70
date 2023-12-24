using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("API/[controller]/[action]")]
    public class userTagController : ControllerBase
    {
        [HttpGet]
        public string getUserTagsByUser(string email) {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (email.Length > 60)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            email = databaseConnection.sanatizer(email);

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.userTags where email = '" + email + "'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
        [HttpGet]
        public string getUserTagsBytag(string tag)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (tag.Length > 60)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            tag = databaseConnection.sanatizer(tag);

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.userTags where activity = '" + tag + "'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpPost]
        public string createUserTag([FromBody] JsonObject details) {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (details["email"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs email", DatabaseConnectionClass.errorCodes.missingEmail);
            }
            else if (details["email"].ToString().Length > 60) {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            if (details["password"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs password", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            else if (details["password"].ToString().Length > 30) {
                return DatabaseConnectionClass.returnErrorStringBuilder("password too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            int tagid;
            if (details["tagid"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs tagid", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            if (!int.TryParse(details["tagid"].ToString(), out tagid)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("tag must be an integer string", DatabaseConnectionClass.errorCodes.invalidValue);
            }

            string email = databaseConnection.sanatizer(details["email"].ToString());
            string password = databaseConnection.sanatizer(details["password"].ToString());

            if (!authenticator.authenticate(email, password)) {
                DatabaseConnectionClass.returnErrorStringBuilder("account could not be authenticated", DatabaseConnectionClass.errorCodes.invalidUser);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.UAT_createUserTag @email='"+email+"', @password='"+password+"', @tagID="+tagid.ToString();

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }


        [HttpDelete]
        public string deleteUserTag([FromBody] JsonObject details)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (details["email"] == null){
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs email", DatabaseConnectionClass.errorCodes.missingEmail);
            }
            else if (details["email"].ToString().Length > 60){
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            if (details["password"] == null){
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs password", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            else if (details["password"].ToString().Length > 30){
                return DatabaseConnectionClass.returnErrorStringBuilder("password too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            int tagid;
            if (details["tagid"] == null){
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs tagid", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            if (!int.TryParse(details["tagid"].ToString(), out tagid)){
                return DatabaseConnectionClass.returnErrorStringBuilder("tag must be an integer string", DatabaseConnectionClass.errorCodes.invalidValue);
            }

            string email = databaseConnection.sanatizer(details["email"].ToString());
            string password = databaseConnection.sanatizer(details["password"].ToString());

            //authenticate account o make sure its correct
            if (!authenticator.authenticate(email, password))
            {
                DatabaseConnectionClass.returnErrorStringBuilder("account could not be authenticated", DatabaseConnectionClass.errorCodes.invalidUser);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.UAT_deleteUserTag @email='" + email + "', @password='" + password + "', @tagID=" + tagid.ToString();

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
    }
}
