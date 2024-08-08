using Bogus;
using CoreAPI.Mongo.Entity;
using CoreAPI.Mongo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Books book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _bookService.CreateAsync(book);
            return Ok(book.id);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Books booksData)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            await _bookService.UpdateAsync(id, booksData);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            await _bookService.DeleteAsync(book.id);
            return NoContent();
        }

        [HttpGet]
        [Route("CreateRandomBooks")]
        public async Task<ActionResult> CreateRandomBooks()
        {
            // Set up the Bogus Faker for the Books class
            var bookFaker = new Faker<Books>()
                .RuleFor(b => b.id, f => f.Random.Guid().ToString("N").Substring(0, 24))        // Generate a fake GUID as a string
                .RuleFor(b => b.name, f => f.Lorem.Sentence(3))                                 // Generate a book name
                .RuleFor(b => b.price, f => f.Random.Int(10, 100))                              // Generate a random price between 10 and 100
                .RuleFor(b => b.category, f => f.Commerce.Categories(1)[0])                     // Generate a random category
                .RuleFor(b => b.author, f => f.Name.FullName());                                // Generate a full name for the author

            // Generate 1000 instances of Books
            List<Books> booksList = bookFaker.Generate(1000);

            foreach (var book in booksList)
            {
                await _bookService.CreateAsync(book);
            }

            return Ok("Books created!");
        }
    }
}