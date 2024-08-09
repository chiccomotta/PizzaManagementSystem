using System;
using Bogus;
using CoreAPI.Mongo.Entity;
using CoreAPI.Mongo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreAPI.Mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await userService.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var book = await userService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await userService.CreateAsync(user);
            return Ok(user.Id);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var book = await userService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            await userService.UpdateAsync(id, user);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await userService.DeleteAsync(user.Id);
            return NoContent();
        }

        [HttpGet]
        [Route("CreateRandomUsers")]
        public async Task<ActionResult> CreateRandomUsers()
        {
            // Faker for the Address class
            var addressFaker = new Faker<Address>()
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Region, f => f.Address.State())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.Phone, f => f.Phone.PhoneNumber("(###) ###-####"));

            // Faker for the User class
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Guid().ToString("N")[..24])           // Generate a fake GUID as a string
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Languages, f => f.Make(3, () => f.Random.Word()))       // Generate a list of 3 random words as languages
                .RuleFor(u => u.Address, f => addressFaker.Generate())                  // Use the address faker to generate an address
                .RuleFor(u => u.YearsProgramming, f => f.Random.Int(1, 30))             // Generate a random number between 1 and 30
                .RuleFor(u => u.BirthDate, f => f.Date.Past(40, DateTime.Now.AddYears(-18)));   // Generate a birth date between 18 and 58 years ago

            // Generate a list of 10 fake users
            List<User> list = userFaker.Generate(1);

            await userService.CreateAllAsync(list);
            return Ok("OK, users created!");
        }
    }
}