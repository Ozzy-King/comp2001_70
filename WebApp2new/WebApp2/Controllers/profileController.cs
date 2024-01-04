using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text.Json.Nodes;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("API/[controller]/[action]")]
    public class profileController : ControllerBase
    {
        //=================----profile----==================
        // GET: proflie
        [HttpGet(Name = "GetUserByEmail")]
        public string getUserByEmail(String email) {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)){
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (email.Length > 60) {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            email = databaseConnection.sanatizer(email);

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.publicUserData where email = '" + email + "'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
        [HttpGet]
        public string getUserByName(String name)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (name.Length > 100)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            name = databaseConnection.sanatizer(name);

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "SELECT * FROM cw2.publicUserData where userName LIKE '%" + name + "%'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpPost(Name = "CreateUser")] //json should contain email, firstname, lastname, password
        public string createUser([FromBody]JsonObject details) {
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

            if (details["firstname"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs firstname", DatabaseConnectionClass.errorCodes.missingFirstName);
            }
            else if (details["firstname"].ToString().Length > 50) {
                return DatabaseConnectionClass.returnErrorStringBuilder("firstname too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            if (details["lastname"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs lastname", DatabaseConnectionClass.errorCodes.missingLastName);
            }
            else if (details["lastname"].ToString().Length > 50) {
                return DatabaseConnectionClass.returnErrorStringBuilder("lastname too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            if (details["password"] == null) {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs password", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            else if (details["password"].ToString().Length > 30) {
                return DatabaseConnectionClass.returnErrorStringBuilder("password too long", DatabaseConnectionClass.errorCodes.tooLong);
            }

            string email = databaseConnection.sanatizer(details["email"].ToString());
            string firstname = databaseConnection.sanatizer(details["firstname"].ToString());
            string lastname = databaseConnection.sanatizer(details["lastname"].ToString());
            string password = databaseConnection.sanatizer(details["password"].ToString());

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.PRO_createUser @email='"+email+"', @firstname='"+firstname+"' , @lastname='"+lastname+"', @password='"+password+"'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpDelete]
        public string deleteUser([FromBody] JsonObject details) {
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
            string email = databaseConnection.sanatizer(details["email"].ToString());
            string password = databaseConnection.sanatizer(details["password"].ToString());

            if(!authenticator.authenticate(email, password)){
                DatabaseConnectionClass.returnErrorStringBuilder("account could not be authenticated", DatabaseConnectionClass.errorCodes.invalidUser);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.PRO_deleteUser @email='" + email + "', @password='" + password + "'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpDelete]
        public string deleteUserAdmin([FromBody] JsonObject details)
        {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip))
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }
            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();
            if (details["email"] == null)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs email", DatabaseConnectionClass.errorCodes.missingEmail);
            }
            else if (details["email"].ToString().Length > 60)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            if (details["adminemail"] == null)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs admin email", DatabaseConnectionClass.errorCodes.missingEmail);
            }
            else if (details["adminemail"].ToString().Length > 60)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("email too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            if (details["adminpassword"] == null)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("error needs admin password", DatabaseConnectionClass.errorCodes.missingPassword);
            }
            else if (details["adminpassword"].ToString().Length > 30)
            {
                return DatabaseConnectionClass.returnErrorStringBuilder("password too long", DatabaseConnectionClass.errorCodes.tooLong);
            }
            string email = databaseConnection.sanatizer(details["email"].ToString());
            string adminemail = databaseConnection.sanatizer(details["adminemail"].ToString());
            string adminpassword = databaseConnection.sanatizer(details["adminpassword"].ToString());

            if (!authenticator.authenticate(adminemail, adminpassword))
            {
                DatabaseConnectionClass.returnErrorStringBuilder("account could not be authenticated", DatabaseConnectionClass.errorCodes.invalidUser);
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.PRO_deleteUserAdmin @useremail='"+email+"', @email='" + adminemail + "', @password='" + adminpassword + "'";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }

        [HttpPut]//email and password are mandatory
        public string updateUser([FromBody] JsonObject details) {
            string ip = Request.Host.Host.ToString();
            if (ipLogger.check(ip)) {
                return DatabaseConnectionClass.returnErrorStringBuilder("over minute limit", DatabaseConnectionClass.errorCodes.overRequestLimit);
            }

            DatabaseConnectionClass databaseConnection = new DatabaseConnectionClass();

            //get mandatory feilds and do some check where needed
            if (details["email"] == null) {
                return "error needs email";
            }
            else if (details["email"].ToString().Length > 60) {
                return DatabaseConnectionClass.returnErrorStringBuilder("email", DatabaseConnectionClass.errorCodes.tooLong);
            }

            if (details["password"] == null) {
                return "error needs password";
            }
            else if (details["password"].ToString().Length > 30) {
                return DatabaseConnectionClass.returnErrorStringBuilder("password", DatabaseConnectionClass.errorCodes.tooLong);
            }
            string email = databaseConnection.sanatizer(details["email"].ToString());
            string password = databaseConnection.sanatizer(details["password"].ToString());

            //check the user is valid before proceding
            if (!authenticator.authenticate(email, password)) {
                DatabaseConnectionClass.returnErrorStringBuilder("account could not be authenticated", DatabaseConnectionClass.errorCodes.invalidUser);
            }

            //setup and get all inputting values
            string firstname = "NULL";
            if (details["firstname"] != null) {
                if (details["firstname"].ToString().Length > 50) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("firstname", DatabaseConnectionClass.errorCodes.tooLong);
                }
                firstname = "'" + databaseConnection.sanatizer(details["firstname"].ToString()) + "'";
            }
            string lastname = "NULL";
            if (details["lastname"] != null)
            {
                if (details["lastname"].ToString().Length > 50) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("lastname", DatabaseConnectionClass.errorCodes.tooLong);
                }
                lastname = "'" + databaseConnection.sanatizer(details["lastname"].ToString()) + "'";
            }
            string userphoto = "NULL";
            if (details["userphoto"] != null)
            {
                if (details["userphoto"].ToString().Length > 100) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("userphoto", DatabaseConnectionClass.errorCodes.tooLong);
                }
                userphoto = "'" + databaseConnection.sanatizer(details["userphoto"].ToString()) + "'";
            }
            string aboutme = "NULL";
            if (details["aboutme"] != null)
            {
                if (details["aboutme"].ToString().Length > 500)
                {
                    return DatabaseConnectionClass.returnErrorStringBuilder("aboutme", DatabaseConnectionClass.errorCodes.tooLong);
                }
                aboutme = "'" + databaseConnection.sanatizer(details["aboutme"].ToString()) + "'";
            }
            string DOB = "NULL";
            //add error handling
            if (details["DOB"] != null)//date DD-MM-YY
            {
                if (details["DOB"].ToString().Split("-").Length < 3) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("DOB", DatabaseConnectionClass.errorCodes.tooShort);
                }

                int day, month, year;
                string strDay, strMonth, strYear;
                if (!int.TryParse(details["DOB"].ToString().Split("-")[0], out day)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("DOB/day", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                if (!int.TryParse(details["DOB"].ToString().Split("-")[1], out month)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("DOB/month", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                if (!int.TryParse(details["DOB"].ToString().Split("-")[2], out year)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("DOB/year", DatabaseConnectionClass.errorCodes.invalidValue);
                }

                if (helperClass.dateValidator(day, month, year)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("DOB", DatabaseConnectionClass.errorCodes.invalidDate);
                }

                strDay = day.ToString();
                strMonth = month.ToString();
                strYear = year.ToString();
                if (strDay.Length < 2) {
                    strDay = "0" + strDay;
                }
                if (strMonth.Length < 2)
                {
                    strMonth = "0" + strMonth;
                }
                while (strYear.Length < 4) {
                    strYear = "0" + strYear;
                }

                DOB = "'" + strYear + "-"+strMonth+"-"+ strDay + "'";
                Console.WriteLine(DOB);
            }
            string newPassword = "NULL";
            if (details["newPassword"] != null)
            {
                if (details["newPassword"].ToString().Length > 30)
                {
                    return DatabaseConnectionClass.returnErrorStringBuilder("newPassword", DatabaseConnectionClass.errorCodes.tooLong);
                }
                newPassword = "'" + databaseConnection.sanatizer(details["newPassword"].ToString()) + "'";
            }
            string activityPreference = "NULL";
            if (details["activityPreference"] != null)//bit 1/0
            {
                if (details["activityPreference"].ToString() != "1" && details["activityPreference"].ToString() != "0") {
                    return DatabaseConnectionClass.returnErrorStringBuilder("activityPreference", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                activityPreference = databaseConnection.sanatizer(details["activityPreference"].ToString());
            }
            string unit = "NULL";
            if (details["unit"] != null)//bit 1/0
            {
                if (details["unit"].ToString() != "1" && details["unit"].ToString() != "0")
                {
                    return DatabaseConnectionClass.returnErrorStringBuilder("unit", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                unit = databaseConnection.sanatizer(details["unit"].ToString());
            }
            string height = "NULL";
            if (details["height"] != null)//float
            {
                float temp;
                if (!float.TryParse(details["height"].ToString(), out temp)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("height", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                height = databaseConnection.sanatizer(details["height"].ToString());
            }
            string weight = "NULL";
            if (details["weight"] != null)//float
            {
                float temp;
                if (!float.TryParse(details["weight"].ToString(), out temp)) {
                    return DatabaseConnectionClass.returnErrorStringBuilder("weight", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                weight = databaseConnection.sanatizer(details["weight"].ToString());
            }
            string address = "NULL";
            if (details["address"] != null)//integer
            {
                int temp;
                if (!int.TryParse(details["address"].ToString(), out temp))
                {
                    return DatabaseConnectionClass.returnErrorStringBuilder("address", DatabaseConnectionClass.errorCodes.invalidValue);
                }
                address = databaseConnection.sanatizer(details["address"].ToString());
            }

            string connectionString = "Server=dist-6-505.uopnet.plymouth.ac.uk;Database=COMP2001_OClark;User Id=OClark;Password=GgyC627+;";
            string commandString = "exec cw2.PRO_updateUser @email='"+email+"', @currPassword='"+password+"', @userPhoto="+userphoto+", @firstname="+firstname+", @lastname="+lastname+", @aboutme="+aboutme+", @DOB ="+DOB+", @password="+newPassword+", @activityPreference="+activityPreference+", @units="+unit+", @height="+height+", @weight="+weight+", @address="+address+" ";

            databaseConnection.connect(connectionString);
            return databaseConnection.executeCommand(commandString);
        }
    }
}
