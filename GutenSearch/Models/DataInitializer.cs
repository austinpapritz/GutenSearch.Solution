using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;


namespace GutenSearch.Models;

public class DataInitializer
{
    public static void InitializeData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GutenSearchContext>();
            context.Database.Migrate();

            // If there's already stuff in db, don't run.
            if (context.Books.Any())
            {
                return;
            }

            var book = new Book[]
            {
                new Book { Title = "Book Title"}
            };

            context.Books.AddRange(book);
            context.SaveChanges();
        }
    }
}