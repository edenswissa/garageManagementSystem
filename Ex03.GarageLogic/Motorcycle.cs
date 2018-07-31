using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ex03.GarageLogic
{
    class Motorcycle: Vehicle
    {
        private int m_EngineCapacity;
        private eLicenceType m_LicenceType;

        internal Motorcycle(Engine i_Engine): base(2, 29, @"C:\Temp\i_QuestionsToGetMotorcycleProps.txt")
        {
                if (i_Engine is FuelEngine)
            {
                const float k_MaxFuelLiters = (float)7.5;
                (i_Engine as FuelEngine).FuelTypeName = FuelEngine.eFuelTypeName.Octan98;
                (i_Engine as FuelEngine).MaxEnergy = k_MaxFuelLiters;
            }
            else if (i_Engine is ElectircEngine)
            {
                const float k_MaxElectricityHours = (float)2.6;
                (i_Engine as ElectircEngine).MaxEnergy = k_MaxElectricityHours;
            }
            
            base.Engine = i_Engine;
        }

        public int EngineCapacity
        {
            get
            {
                return m_EngineCapacity;
            }

            set
            {
                m_EngineCapacity = value;
            }
        }

        internal eLicenceType LicenceType
        {
            get
            {
                return m_LicenceType;
            }

            set
            {
                m_LicenceType = value;
            }
        }

        public enum eLicenceType
        {
            A,
            A1,
            B1,
            B2,
        }

        public override void InitializeVehicleWithAdditionalProps(string i_PathOfFileWithValuesToClassMembers)
        {
            string[] valsForMembers = File.ReadAllLines(i_PathOfFileWithValuesToClassMembers);

            //[0] is licence type:
            m_LicenceType = getLicenceTypeFromStr(valsForMembers[0]);
            //[1] is engine capacity
            int.TryParse(valsForMembers[1], out m_EngineCapacity);
        }

        public override string GetVehicleAdditionalPropsForDisplay()
        {
            string theAdditionalPropsDisplay = string.Format
(@"License Type :   {0}
Engine Capacity :   {1}
", LicenceType, EngineCapacity);

            return theAdditionalPropsDisplay;
        }

        private eLicenceType getLicenceTypeFromStr(string i_ValStr)
        {
            eLicenceType licence = new eLicenceType();

            if (!Enum.TryParse(i_ValStr.ToUpper(), out licence))
            {
                throw new ArgumentException("No such licence option!");
            }

            return licence;
        }
    }
}
