using Microsoft.AspNetCore.Mvc;
using books.Infrastructure;
using books.Domain;
using books.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace books.Controllers;

[ApiController]
[Route("api/contacts")]

public class BooksController : ControllerBase
{
    private readonly DataService _dataService;

    public BooksController(DataService dataService)
    {
        _dataService = dataService;
    }
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
}

