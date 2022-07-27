namespace WebApiGPS.Models
{
    public class Tracker
    {
        public int Id { get; set; }
        public string IMEI { get; set; } = null!;
        public decimal Balance { get; set; } = 0;
        public string Phone { get; set; } = null!;
        public virtual Car? Car { get; set; }
        public virtual Person? Person { get; set; }
        public virtual Person? Responsible { get; set; }
        public virtual Charge? Charge { get; set; }
        public List<Geoposition>? Geopositions { get; set; }
    }
}
