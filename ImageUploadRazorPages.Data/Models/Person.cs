using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ImageUploadRazorPages.Data.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string? FirstName { get; set; }

    [Required]
    [StringLength(20)]
    public string? LastName { get; set; }

    public string? ProfilePicture { get; set; }

    public IFormFile? ImageFile { get; set; }

}
