// Utilizing AJAX to asynchronously delete a stylist after user-confirmation.
const deleteLinks = document.querySelectorAll('.delete-link');

// Create click handler for every deleteLink element.
deleteLinks.forEach((deleteLink) => {
    deleteLink.addEventListener('click', (e) => {
        e.preventDefault(); 

        // Grab the bookId from the data-id attribute.
        let bookId = deleteLink.getAttribute('data-id');
        let url = "/Books/Delete/" + bookId;
        
        // Ask user for confirmation.
        if (confirm('Are you sure you want to delete this book?')) {
            // Initiates an AJAX request on confirmation.
            $.ajax({
                // Route and type of request.
                url: url,
                type: 'POST',
                // Delete route requires an Id.
                data: { id: bookId },
                // The controller sends back Ok() if successful.
                success: function(result) {
                    location.reload();
                },
                error: function(result) {
                    alert("Error deleting book. Please try again later.");
                }
            });
        }
    });
});
