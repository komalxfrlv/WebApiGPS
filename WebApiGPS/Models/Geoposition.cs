namespace WebApiGPS.Models
{
    public class Geoposition
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
        public virtual Tracker Tracker { get; set; } = null!;
    }
}
