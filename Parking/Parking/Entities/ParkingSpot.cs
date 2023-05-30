using Parking.Entities.Enums;
using System;
using System.Text;

namespace Parking.Entities
{
    internal class ParkingSpot
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public Vehicle Vehicle { get; set; } = new Vehicle();
        public DateTime ExitTime { get; set; }
        public TimeSpan TimeOfPermanency { get; set; }
        public Availability Status { get; set; } = Availability.Available;

        public ParkingSpot() { }

        public ParkingSpot(int numberParkingSpot)
        {
            Id = numberParkingSpot;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            EntryTime = DateTime.Now;
            Vehicle = vehicle;
            Status = Availability.Unavailable;
        }

        public void RemoveVehicle()
        {
            ExitTime = DateTime.Now;
            Status = Availability.Available;
            Vehicle = null;
        }

        public TimeSpan TimePermanency()
        {
            TimeSpan t = ExitTime.Subtract(EntryTime);
            TimeOfPermanency = t;
            return TimeOfPermanency;
        }


        public double PricePerPeriod()
        {
            double pricePerPeriod = 0;
            double n = TimeOfPermanency.TotalMinutes;

            if (n <= 780 && n > 60)
            {
                pricePerPeriod = Math.Ceiling(n / 60) * 5.00;
            }
            else if(n <= 60)
            {
                pricePerPeriod = 5.00;
            }
            else
            {
                pricePerPeriod = 13 * 5.00;
            }

            return pricePerPeriod;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Id + "; ");
            sb.Append(Vehicle.TypeOfVehicle + "; ");
            sb.Append(Vehicle.NumberPlate + "; ");
            sb.Append(EntryTime + "; ");
            if (ExitTime >=  EntryTime)
            {
                sb.Append(ExitTime + "; ");
            }
            return sb.ToString();
        }
    }
}
