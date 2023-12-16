using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages
{
    public class testModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost()
        {
            var name = Request.Form["Name"];
            var email = Request.Form["Email"];
            ViewData["confirmation"] = $"{name}, information will be sent to {email}";
        }
    }
}
