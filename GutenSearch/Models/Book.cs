using System.ComponentModel.DataAnnotations;

namespace GutenSearch.Models;

public class Book
{
    public int BookId { get; set; }
    [Required]
    public string Title { get; set; }
    public List<AuthorBook> AuthorBooks { get; set; }
    public List<Copy> Copies { get; set; }
}