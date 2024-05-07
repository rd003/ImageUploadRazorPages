using ImageUploadRazorPages.Data.Models;
using ImageUploadRazorPages.Data.Repositories;
using ImageUploadRazorPages.Data.Shared;
using ImageUploadRazorPages.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploadRazorPages.UI.Pages.People;

public class AddModel : PageModel
{
    private readonly IPersonRepository personRepository;
    private readonly IFileService fileService;

    [BindProperty]
    public Person? Person { get; set; }

    public AddModel(IPersonRepository personRepository,IFileService fileService)
    {
        this.personRepository = personRepository;
        this.fileService = fileService;
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }
        try
        {
            if (Person?.ImageFile!=null)
            {
                if (Person.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("File can not exceed 1 MB");
                }
                Person.ProfilePicture = await fileService.SaveFile(Person.ImageFile, "Images", new string[] { ".jpg", ".jpeg", ".png" });
            }
            await personRepository.AddPerson(Person);
            TempData[NotificationTypes.SuccessMessage] = "Saved successfully.";
            return RedirectToPage("/People/Add");
        }
        catch (InvalidOperationException ex)
        {
            TempData[NotificationTypes.ErrorMessage] = ex.Message;
            return Page();
        }
        catch (FileNotFoundException ex)
        {
            TempData[NotificationTypes.ErrorMessage] = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            TempData[NotificationTypes.ErrorMessage] = "Something went wrong!";
            return Page();
        }


    }

}
