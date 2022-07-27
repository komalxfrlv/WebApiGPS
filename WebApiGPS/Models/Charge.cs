namespace WebApiGPS.Models
{
    public class Charge
    {
        public int Id { get; set; }
        public double Power { get; set; } = 0.0;
        public bool IsCharging { get; set; } = false;
    }
}
