using ImageUploadRazorPages.Data.Models;
using ImageUploadRazorPages.Data.Repositories;
using ImageUploadRazorPages.Data.Shared;
using ImageUploadRazorPages.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploadRazorPages.UI.Pages.People
{
    public class UpdateModel : PageModel
    {
        private readonly IPersonRepository personRepository;
        private readonly IFileService fileService;

        [BindProperty]
        public Person? Person { get; set; }

        public UpdateModel(IPersonRepository personRepository, IFileService fileService)
        {
            this.personRepository = personRepository;
            this.fileService = fileService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var person = await personRepository.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            Person = person;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                string? oldProfilePic = "";
                if (Person?.ImageFile is not null)
                {
                    if (Person.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("File can not exceed 1 MB");
                    }
                    oldProfilePic = Person.ProfilePicture;
                    Person.ProfilePicture = await fileService.SaveFile(Person.ImageFile, "Images", new string[] { ".jpg", ".jpeg", ".png" });
                }
                await personRepository.UpdatePerson(Person);
                if (!string.IsNullOrWhiteSpace(oldProfilePic))
                {
                    fileService.DeleteFile(oldProfilePic, "Images");
                }
                TempData[NotificationTypes.SuccessMessage] = "Saved successfully.";
                return RedirectToPage("/People/Index");
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
}
