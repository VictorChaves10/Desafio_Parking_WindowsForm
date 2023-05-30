using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Entities
{
    internal class Vehicle
    {
        public string TypeOfVehicle { get; set; }

        private string _numberPlate;

        public string NumberPlate
        {
            get { return _numberPlate; }
            set
            {
                if (value.Length == 7)
                {
                    _numberPlate = value;
                }
            }
        }
     
        public Vehicle() { }
        public Vehicle(string typeOfVehicle, string numberPlate)
        {
            TypeOfVehicle = typeOfVehicle;
            _numberPlate = numberPlate;
        }

    }
}
