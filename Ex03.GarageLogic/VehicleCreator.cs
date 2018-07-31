using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class VehicleCreator
    {
        public enum eVehicleType
        {
            ElectricCar = 1,
            ElectricMotorcycle = 2,
            FuelCar = 3,
            FuelMotorcycle = 4,
            FuelTruck = 5,
        }

        public static Vehicle CreateEmptySpecificVehicleType(eVehicleType i_RequiredVehicleType)
        {
            Vehicle retVal;

            switch (i_RequiredVehicleType) 
            {
                case (eVehicleType.ElectricCar):
                    retVal = new Car(new ElectircEngine());
                    break;
                case (eVehicleType.ElectricMotorcycle):
                    retVal = new Motorcycle(new ElectircEngine());
                    break;
                case (eVehicleType.FuelCar):
                    retVal = new Car(new FuelEngine());
                    break;
                case (eVehicleType.FuelMotorcycle):
                    retVal = new Motorcycle(new FuelEngine());
                    break;
                case (eVehicleType.FuelTruck):
                    retVal = new Truck(new FuelEngine());
                    break;
                default:
                    throw new ValueOutOfRangeException("No such vehicle type!");
            }

            return retVal;
        }

    } 
}
