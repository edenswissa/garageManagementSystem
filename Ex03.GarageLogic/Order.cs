using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Order
    {
        private string m_OwnerName;
        private string m_OwnerPhoneNumber;
        private eVehicleStatus m_VehicleStatus = eVehicleStatus.FixingUp;
        private Vehicle m_TheVehicle;

        public string OwnerName
        {
            get
            {
                return m_OwnerName;
            }

            set
            {
                m_OwnerName = value;
            }
        }

        public string OwnerPhoneNumber
        {
            get
            {
                return m_OwnerPhoneNumber;
            }

            set
            {
                m_OwnerPhoneNumber = value;
            }
        }

        public eVehicleStatus VehicleStatus
        {
            get
            {
                return m_VehicleStatus;
            }

            set
            {
                m_VehicleStatus = value;
            }
        }

        public Vehicle TheVehicle
        {
            get
            {
                return m_TheVehicle;
            }
            set
            {
                m_TheVehicle = value;
            }
        }

        public enum eVehicleStatus
        {
            FixingUp = 1,
            Fixed = 2,
            Paid = 3,
        }

        public string GetOrderPropsForDisplay()
        {
            string thePropsDisplay = string.Format
(@"License Number   :   {0}
Owner Name  :   {1}
Status in Garage    :   {2}
", TheVehicle.LicenceNumber, OwnerName, VehicleStatus);

            return thePropsDisplay;
        }
    }
}
