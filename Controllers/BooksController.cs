using Microsoft.AspNetCore.Mvc;
using books.Infrastructure;
using books.Domain;
using books.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace books.Controllers;

[ApiController]
[Route("api/books")]

public class BooksController : ControllerBase
{
    private readonly DataService _dataService;

    public BooksController(DataService dataService)
    {
        _dataService = dataService;
    }

    // GET api/books
    // GET api/books?search=
    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetBooks([FromQuery] string? search)
    {
        var query = _dataService.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Author.Contains(search));
        }

        var booksDto = query.Select(c => new BookDto
        {
            Id = c.Id,
            Title = c.Title,
            Author = c.Author,
            
        });

        return Ok(booksDto);
    }

    // GET api/books/1
    [HttpGet("{id:int}")]
    public ActionResult<BookDto> GetBook(int id)
    {
        var book = _dataService.Books.SingleOrDefault(c => c.Id == id);

        if (book is null)
        {
            return NotFound();
        }

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author
            
        };

        return Ok(bookDto);
    }

    // POST api/books
    [HttpPost]
    public IActionResult CreateBook([FromBody] BookForCreationDto bookForCreationDto)
    {
        if (bookForCreationDto.Author == bookForCreationDto.Title)
        {
            ModelState.AddModelError("wrongTitle", "Author and title cannot be the same.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var maxId = _dataService.Books.Max(c => c.Id);

        var book = new Book
        {
            Id = maxId + 1,
            Author = bookForCreationDto.Author,
            Title = bookForCreationDto.Title
            
        };

        _dataService.Books.Add(book);

        var bookDto = new BookDto
        {
            Id = book.Id,
            Author = book.Author,
            Title = book.Title
            
        };

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
    }

    // PUT api/books/{id}
    [HttpPut("{id:int}")]
    public IActionResult UpdateBook(int id, [FromBody] BookForUpdateDto bookForUpdateDto)
    {
        var book = _dataService.Books.SingleOrDefault(c => c.Id == id);

        if (book is null)
        {
            return NotFound();
        }

        book.Author = bookForUpdateDto.Author;
        book.Title = bookForUpdateDto.Title;
        

        return NoContent();
    }

    // DELETE api/books/{id}
    [HttpDelete("{id:int}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _dataService.Books.SingleOrDefault(c => c.Id == id);

        if (book is null)
        {
            return NotFound();
        }

        _dataService.Books.Remove(book);

        return NoContent();
    }

    // PATCH api/books/{id}
    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateBook(int id, [FromBody] JsonPatchDocument<BookForUpdateDto> patchDocument)
    {
        var book = _dataService.Books.SingleOrDefault(c => c.Id == id);

        if (book is null)
        {
            return NotFound();
        }

        var bookToBePatched = new BookForUpdateDto()
        {
            Title = book.Title,
            Author = book.Author
        };

        patchDocument.ApplyTo(bookToBePatched, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!TryValidateModel(bookToBePatched))
        {
            return BadRequest(ModelState);
        }

        book.Title = bookToBePatched.Title;
        book.Author = bookToBePatched.Author;

        return NoContent();
    }
}

