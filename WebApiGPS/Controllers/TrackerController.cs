namespace WebApiGPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TrackerController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("token/{token}")]
        public async Task<ActionResult<List<Tracker>>> GetAllTrackersByToken(string token)
        {
            var trackers = await _context.Trackers!.Select(t => new Tracker
            {
                Id = t.Id,
                Balance = t.Balance,
                IMEI = t.IMEI,
                Phone = t.Phone,
                Car = t.Car,
                Person = t.Person,
                Charge = t.Charge,
                Responsible = t.Responsible,
                Geopositions = new List<Geoposition> {
                    new Geoposition()
                    {
                        Latitude = t.Geopositions!.OrderByDescending(r => r.Id).FirstOrDefault().Latitude,
                        Longitude = t.Geopositions!.OrderByDescending(r => r.Id).FirstOrDefault().Longitude,
                        DateTime = t.Geopositions!.OrderByDescending(r => r.Id).FirstOrDefault().DateTime
                    }
                },
            }).ToListAsync();

            return Ok(trackers);
        }
        
        [HttpGet("imei/{imei}")]
        public async Task<ActionResult<List<Geoposition>>> GetGeopositionByImei(string imei)
        {
            Tracker? tracker = await _context.Trackers!.Where(t => t.IMEI == imei.Trim()).FirstOrDefaultAsync();
            return Ok(await _context.Geopositions!.Where(g => g.Tracker.Id == tracker!.Id).Select(g => new Geoposition { 
                Id = g.Id,
                Latitude = g.Latitude,
                Longitude = g.Longitude,
                DateTime = g.DateTime
            }).OrderByDescending(g => g.Id).ToListAsync());
        }

        [HttpPost("filters")]
        public async Task<ActionResult<List<Tracker>>> GetGeopositionByFilters([FromBody] TrackerFilters filters)
        {
            List<Tracker> trackers = await _context.Trackers
                .Where(
                t =>
                    filters.Responsible.Contains(t.Responsible.Id) ||
                    filters.Cars.Contains(t.Car.Id) ||
                    filters.Persons.Contains(t.Person.Id))
                .Select(
                r => new Tracker {
                    Id = r.Id,
                    Balance = r.Balance,
                    IMEI = r.IMEI,
                    Phone = r.Phone,
                    Car = r.Car,
                    Person = r.Person,
                    Charge = r.Charge,
                    Responsible = r.Responsible,
                    Geopositions = r.Geopositions
                })
                .ToListAsync();
            return Ok(trackers);
        }


        [HttpPost("add-new")]
        public async Task<ActionResult> AddNewTracker(string imei, string phone, int? carId, int? personId, int responsibleId)
        {
            Car? car = _context.Cars!.Find(carId);
            Person? person = _context.Persons!.Find(personId);
            Person? responsible = _context.Persons!.Find(responsibleId);

            responsible!.IsResponsible = true;

            if ((car == null && person == null) || responsible == null)
                return BadRequest("Ошибка");

            Tracker? trackerIsSet = await _context.Trackers!.Where(t => t.IMEI == imei.Trim()).FirstOrDefaultAsync();
            if (trackerIsSet != null)
                return BadRequest("Такой трекер уже существует!");
            if (
                !string.IsNullOrEmpty(imei.Trim()) &&
                !string.IsNullOrEmpty(phone.Trim())
                )
            {
                Charge charge = new()
                {
                    IsCharging = false,
                    Power = 100.0
                };
                Tracker tracker = new()
                {
                    IMEI = imei,
                    Phone = phone,
                    Balance = (decimal)0.0,
                    Car = car,
                    Person = person,
                    Responsible = responsible,
                    Charge = charge,
                };
                Geoposition geoposition = new()
                {
                    Tracker = tracker,
                    DateTime = DateTime.UtcNow,
                    Latitude = 0.0000000,
                    Longitude = 0.0000000
                };
                _context.Trackers!.Add(tracker);
                _context.Geopositions!.Add(geoposition);
                _context.Persons.Update(responsible!);
                await _context.SaveChangesAsync();

                return Ok("Новый трекер добавлен!");
            }
            else
            {
                return BadRequest("Заполните форму");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddGeoAndUpdate(double lat, double lng, string imei, decimal balance, double power)
        {
            Tracker? tracker = await _context.Trackers!.Where(t => t.IMEI == imei.Trim()).FirstOrDefaultAsync();
            if (tracker != null)
            {
                Geoposition geoposinion = new()
                {
                    Latitude = lat,
                    Longitude = lng,
                    Tracker = tracker,
                    DateTime = DateTime.UtcNow
                };
                _context.Geopositions!.Add(geoposinion);

                tracker.Balance = balance;
                _context.Update(tracker);

                Charge? charge = await _context.Charges!.Where(c => c.Id == tracker.Charge!.Id).FirstOrDefaultAsync();
                if (charge != null)
                {
                    charge.Power = power;
                    _context.Update(charge);
                }
                else
                {
                    return BadRequest();
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
