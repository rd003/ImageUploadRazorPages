using ImageUploadRazorPages.Data.Repositories;
using ImageUploadRazorPages.Data.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploadRazorPages.UI.Pages.People;

public class AddModel : PageModel
{
    private readonly IPersonRepository personRepository;
    private readonly IFileService fileService;

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
        
    }
}
