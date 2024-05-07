using ImageUploadRazorPages.Data.Models;
using ImageUploadRazorPages.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploadRazorPages.UI.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly IPersonRepository _personRepository;

        public IndexModel(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [BindProperty]
        public IEnumerable<Person> PersonList { get; set; }

        public async Task OnGetAsync()
        {
            PersonList = await _personRepository.GetPeople();
        }
    }
}
