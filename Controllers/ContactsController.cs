using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        //Get all contact methods
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());

        }

        //get single contact
        [HttpGet]
        [Route("{id.guid}")]

        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        // Add(Post) all contact methods
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest) 
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone,
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        //Update (Put) all contacts method
        [HttpPut]
        [Route("{id.guid}")]
        public async Task<IActionResult>UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)

        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null) 
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;

                await dbContext.SaveChangesAsync();
                return Ok(contact);
            
            }
            return NotFound();
        }
        // Delete Method
        [HttpDelete]
        [Route("{id.guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact != null)
            {
                dbContext.Contacts.Remove(contact);
              await  dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound(contact);
        }
    }

}
