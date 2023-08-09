using System.ComponentModel.DataAnnotations;

namespace GutenSearch.Models;

// This class represents the copies of a book available for checkout.
public class Copy
{
    public int CopyId { get; set; }
    public int CopyCount { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}