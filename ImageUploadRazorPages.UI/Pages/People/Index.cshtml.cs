using ImageUploadRazorPages.Data.Models;
using ImageUploadRazorPages.Data.Repositories;
using ImageUploadRazorPages.Data.Shared;
using ImageUploadRazorPages.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploadRazorPages.UI.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IFileService _fileService;

        public IndexModel(IPersonRepository personRepository,IFileService fileService)
        {
            _personRepository = personRepository;
            _fileService = fileService;
        }

        [BindProperty]
        public IEnumerable<Person> PersonList { get; set; }

        public async Task OnGetAsync()
        {
            PersonList = await _personRepository.GetPeople();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                var person = await _personRepository.GetPersonById(id);
                if (person == null)
                {
                    return NotFound();
                }
                await _personRepository.DeletePerson(id);
                if (!string.IsNullOrWhiteSpace(person.ProfilePicture))
                {
                    _fileService.DeleteFile(person.ProfilePicture, "Images");
                }
                TempData[NotificationTypes.SuccessMessage] = "Deleted successfully";
            }
            catch (FileNotFoundException ex)
            {
                TempData[NotificationTypes.ErrorMessage] = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                TempData[NotificationTypes.ErrorMessage] = ex.Message;
            }

            catch (Exception ex)
            {
                TempData[NotificationTypes.ErrorMessage] = "Something went wrong!!";
            }
            return RedirectToPage("/People/Index");
        }

    }
}
