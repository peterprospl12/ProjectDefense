using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Web.Pages
{
    public class IndexModel(SignInManager<User> signInManager) : PageModel
    {
        public IActionResult OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Lecturer"))
                {
                    return RedirectToPage("/Lecturer/Index");
                }

                if (User.IsInRole("Student"))
                {
                    return RedirectToPage("/Student/Index");
                }
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}