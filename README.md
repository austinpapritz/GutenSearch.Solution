Library Prompt

## Librarian

- DONE: As a librarian, I want to create, read, update, delete, and list books in the catalog, so that we can keep track of our inventory.

- DONE: As a librarian, I should only be able to create, update and delete if I am logged in. All users should be able to have read functionality. (Hint: authorize CUD routes for books.)
(wait for User roles)

- As a librarian, I want to search for a book by author or title, so that I can find a book when there are a lot of books in the library.

- As a librarian, I want to enter multiple authors for a book, so that I can include accurate information in my catalog. (Hint: make an authors table and a books table with a many-to-many relationship.)

- As a librarian, I want to see a list of overdue books, so that I can call up the patron who checked them out and tell them to bring them back — OR ELSE!

## Patron

- As a patron, I want to check a book out, so that I can take it home with me. I should only be able to do this if I am logged in.

- As a patron, I want to know how many copies of a book are on the shelf, so that I can see if any are available. (Hint: make a copies table; a book should have many copies.)

- As a patron, I want to see a history of all the books I checked out, so that I can look up the name of that awesome sci-fi novel I read three years ago. (Hint: make a checkouts table that is a join table between patrons and copies.)

- As a patron, I want to know when a book I checked out is due, so that I know when to return it.
