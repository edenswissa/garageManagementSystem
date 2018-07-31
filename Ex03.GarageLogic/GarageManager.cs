using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private Dictionary<string, Order> m_OrdersDictionary = new Dictionary<string, Order>();
        private Dictionary<string, Order> m_FixingUpVehicles = new Dictionary<string, Order>();
        private Dictionary<string, Order> m_FixedVehicles = new Dictionary<string, Order>();
        private Dictionary<string, Order> m_PaidVehicles = new Dictionary<string, Order>();

        public Dictionary<string, Order> OrdersDictionary
        {
            get
            {
                return m_OrdersDictionary;
            }

            set
            {
                m_OrdersDictionary = value;
            }
        }

        public Dictionary<string, Order> FixingUpVehicles
        {
            get
            {
                return m_FixingUpVehicles;
            }

            set
            {
                m_FixingUpVehicles = value;
            }
        }

        public Dictionary<string, Order> PaidVehicles
        {
            get
            {
                return m_PaidVehicles;
            }

            set
            {
                m_PaidVehicles = value;
            }
        }

        public Dictionary<string, Order> FixedgVehicles
        {
            get
            {
                return m_FixedVehicles;
            }

            set
            {
                m_FixedVehicles = value;
            }
        }

        public void AddOrderToGarage(string i_LicenceNumber, string i_OwnerName, string i_PhoneNumber, string i_EnteredVehicleType, ref Order io_InitializedOrder)
        {
            io_InitializedOrder = new Order();

            io_InitializedOrder.OwnerName = i_OwnerName;
            io_InitializedOrder.OwnerPhoneNumber = i_PhoneNumber;
            io_InitializedOrder.VehicleStatus = Order.eVehicleStatus.FixingUp;
            VehicleCreator.eVehicleType enumEnteredVehicleType;
            if (!Enum.TryParse(i_EnteredVehicleType, true, out enumEnteredVehicleType))
            {
                throw new FormatException("Not valid input!");
            }

            io_InitializedOrder.TheVehicle = VehicleCreator.CreateEmptySpecificVehicleType(enumEnteredVehicleType);
            io_InitializedOrder.TheVehicle.LicenceNumber = i_LicenceNumber;
            m_OrdersDictionary.Add(i_LicenceNumber, io_InitializedOrder);
            m_FixingUpVehicles.Add(i_LicenceNumber, io_InitializedOrder);
        }

        public void ChangeVehicleStatus(string i_LicenceNumber, Order.eVehicleStatus i_NewStatus)
        {
            //check input logically
             if (!i_NewStatus.Equals(Order.eVehicleStatus.FixingUp) && !i_NewStatus.Equals(Order.eVehicleStatus.Fixed) &&
                !i_NewStatus.Equals(Order.eVehicleStatus.Paid))
            {
                throw new ArgumentException("No such choice!");
            }
            
            Order.eVehicleStatus oldStatus = m_OrdersDictionary[i_LicenceNumber].VehicleStatus;
            if(!oldStatus.Equals(i_NewStatus))
            {
                //first add to the correct dictionary:
                switch (i_NewStatus)
                {
                    case (Order.eVehicleStatus.FixingUp):
                        m_FixingUpVehicles.Add(i_LicenceNumber, m_OrdersDictionary[i_LicenceNumber]);
                        m_OrdersDictionary[i_LicenceNumber].VehicleStatus = Order.eVehicleStatus.FixingUp;
                        break;
                    case (Order.eVehicleStatus.Fixed):
                        m_FixedVehicles.Add(i_LicenceNumber, m_OrdersDictionary[i_LicenceNumber]);
                        m_OrdersDictionary[i_LicenceNumber].VehicleStatus = Order.eVehicleStatus.Fixed;
                        break;
                    case (Order.eVehicleStatus.Paid):
                        m_PaidVehicles.Add(i_LicenceNumber, m_OrdersDictionary[i_LicenceNumber]);
                        m_OrdersDictionary[i_LicenceNumber].VehicleStatus = Order.eVehicleStatus.Paid;
                        break;
                }

                //then delete from previous dictionary:
                switch (oldStatus)
                {
                    case (Order.eVehicleStatus.FixingUp):
                        m_FixingUpVehicles.Remove(i_LicenceNumber);
                        break;
                    case (Order.eVehicleStatus.Fixed):
                        m_FixedVehicles.Remove(i_LicenceNumber);
                        break;
                    case (Order.eVehicleStatus.Paid):
                        m_PaidVehicles.Remove(i_LicenceNumber);
                        break;
                }
            }
        }

        public void FillFuelInVehicle(string i_LicenceOfVehicleInGarage, FuelEngine.eFuelTypeName i_FuelType, float i_AmountToFill)
        {
            if(m_OrdersDictionary[i_LicenceOfVehicleInGarage].TheVehicle.Engine is FuelEngine)
            {
                (m_OrdersDictionary[i_LicenceOfVehicleInGarage].TheVehicle.Engine as FuelEngine).FillFuel(i_AmountToFill, i_FuelType);
            }
            else
            {
                throw new ArgumentException("The vehicle is not using fuel for energy!");
            }
        }

        public void ChargeBattery(float i_MinutesToCharge, string i_LicenseNumber)
        {
            if (m_OrdersDictionary[i_LicenseNumber].TheVehicle.Engine is ElectircEngine)
            {
                (m_OrdersDictionary[i_LicenseNumber].TheVehicle.Engine as ElectircEngine).ChargeBattery(i_MinutesToCharge / 60);
            }
            else
            {
                throw new ArgumentException("The vehicle is not using electric to energy");
            }
        }   
    }
}
