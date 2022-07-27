using System.Globalization;

namespace WebApiGPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PersonController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("token/{token}")]
        public async Task<ActionResult<List<Person>>> GetAllPersonsByToken(string token)
        {
            if (token != null)
                return await _context.Persons!.ToListAsync();
            return new List<Person>();
        }

        [HttpGet("irres/token/{token}")]
        public async Task<ActionResult<List<Person>>> GetIrresponsibleByToken(string token)
        {
            if (token != null)
                return await _context.Persons!.Where(r => r.IsResponsible == false).ToListAsync();
            return new List<Person>();
        }

        [HttpGet("res/token/{token}")]
        public async Task<ActionResult<List<Person>>> GetResponsiblesByToken(string token)
        {
            if (token != null)
                return await _context.Persons!.Where(r => r.IsResponsible == true).ToListAsync();
            return new List<Person>();
        }

        [HttpPost]
        public async Task<ActionResult> Post(string name, string surname, string phone)
        {
            name = name.Trim().ToUpper();
            surname = surname.Trim().ToUpper();
            phone = phone.Trim().ToUpper();

            if (
                !string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(surname) &&
                !string.IsNullOrEmpty(phone)
                )
            {
                Person person = new()
                {
                    Name = name,
                    Surname = surname,
                    Phone = phone,
                };
                _context.Persons!.Add(person);
                await _context.SaveChangesAsync();

                return Ok("Новый человек добавлен!");
            }
            else
            {
                return BadRequest("Заполните форму");
            }
        }
    }
}
