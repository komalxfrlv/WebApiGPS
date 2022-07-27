namespace WebApiGPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CarController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("token/{token}")]
        public async Task<ActionResult<List<Car>>> GetAllCarsByToken(string token)
        {
            if (token != null)
                return await _context.Cars!.ToListAsync();
            return new List<Car>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            return Ok(await _context.Cars!.FindAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post(string mark, string model, string regNumber, string vin)
        {
            regNumber = regNumber.Trim().ToUpper();

            Car? carIsSet = await _context.Cars!.Where(c => c.RegNumber == regNumber).FirstOrDefaultAsync();
            if (carIsSet != null)
                return BadRequest("Такая машина уже существует!");
            if (
                !string.IsNullOrEmpty(model.Trim()) &&
                !string.IsNullOrEmpty(mark.Trim()) &&
                !string.IsNullOrEmpty(regNumber.Trim()) &&
                !string.IsNullOrEmpty(vin.Trim())
                )
            {
                Car car = new()
                {
                    Mark = mark.Trim().ToUpper(),
                    Model = model.Trim().ToUpper(),
                    RegNumber = regNumber.Trim().ToUpper(),
                    VIN = vin.Trim().ToUpper(),
                };
                _context.Cars!.Add(car);
                await _context.SaveChangesAsync();

                return Ok("Новый автомобиль добавлен!");
            }
            else
            {
                return BadRequest("Заполните форму");
            }
        }
    }
}
