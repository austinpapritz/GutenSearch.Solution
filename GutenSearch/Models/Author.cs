using System.ComponentModel.DataAnnotations;

namespace GutenSearch.Models;

public class Author
{
    public int AuthorId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public List<AuthorBook> AuthorBooks { get; set; }
}