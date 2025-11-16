using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectDefense.Web.Pages.Lecturer
{
    [Authorize(Roles = "Lecturer")]
    public class IndexModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }
    }
}
