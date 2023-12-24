using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;

namespace WebApp2
{
    public class authenticator
    {
        public static bool authenticate(string email, string password) {
            //set up the clients and message with post and the body content
            HttpClient client = new HttpClient(); //creates the client
            client.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk"); //sets the base address
            HttpRequestMessage message = new HttpRequestMessage(new HttpMethod("POST"), "COMP2001/auth/api/USERS");
            HttpContent content = new StringContent("{ \"email\":\""+email+"\", \"password\":\""+password+"\" }",
                                    Encoding.UTF8,
                                    "application/json"); //setups the body withthe email and password
            message.Content = content;

            //send and recive response
            HttpResponseMessage response = client.Send(message);//send for the response
            Stream responseStream = response.Content.ReadAsStream();

            //read from stream into a string
            int temp;
            string jsonResponse= "";
            while ( (temp = responseStream.ReadByte()) != -1 ) {
                jsonResponse += (char)temp;
            }
            //format the resonse and return result
            jsonResponse=jsonResponse.Remove(0,1); jsonResponse=jsonResponse.Remove(jsonResponse.Length-1, 1);//removes the irst and last square brakcet(])
            return bool.Parse(jsonResponse.Split(",")[1].ToLower().Trim().Trim('\"')); //splits the response, gets the second element, removes the trailing white space, removes the quotes and lowers it so it can be pasrsed to a boolean
        }
    }
}
