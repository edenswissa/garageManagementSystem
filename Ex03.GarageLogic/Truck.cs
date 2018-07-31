using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ex03.GarageLogic
{
    class Truck: Vehicle
    {
        private bool m_HasDangerousCargo;
        private float m_MaxCargoWeight;

        internal Truck(Engine i_Engine): base(18, 30, @"c:\Temp\i_QuestionsToGetTruckProps.txt")
        {
            const float k_MaxFuelLiters = (float)130;
            (i_Engine as FuelEngine).FuelTypeName = FuelEngine.eFuelTypeName.Soler;
            (i_Engine as FuelEngine).MaxEnergy = k_MaxFuelLiters;

            base.Engine = i_Engine;
        }

        public bool HasDangerousCargo
        {
            get
            {
                return m_HasDangerousCargo;
            }

            set
            {
                m_HasDangerousCargo = value;
            }
        }

        public float MaxCargoWeight
        {
            get
            {
                return m_MaxCargoWeight;
            }

            set
            {
                m_MaxCargoWeight = value;
            }
        }

        public override void InitializeVehicleWithAdditionalProps(string i_PathOfFileWithValuesToClassMembers)
        {
            string[] valsForMembers = File.ReadAllLines(i_PathOfFileWithValuesToClassMembers);

            //[0] is dangerous cargo:
            m_HasDangerousCargo = getIfDangerousCargoFromStr(valsForMembers[0]);

            //[1] is max cargo weight
            float.TryParse(valsForMembers[1], out m_MaxCargoWeight);
        }

        public override string GetVehicleAdditionalPropsForDisplay()
        {
            string theAdditionalPropsDisplay = string.Format
(@"Has Dangerous Cargo  :   {0}
Max Cargo Weight    :   {1}
", HasDangerousCargo.ToString(), MaxCargoWeight);

            return theAdditionalPropsDisplay;
        }

        private bool getIfDangerousCargoFromStr(string i_ValStr)
        {
            bool retVal;
            const string k_yes = "y";
            const string k_no = "n";

            if (k_yes.Equals(i_ValStr.ToLower()))
            {
                retVal = true;
            }
            else if (k_no.Equals(i_ValStr.ToLower()))
            {
                retVal = false;
            }
            else
            {
                throw new ArgumentException("Not valid inpot for dangerous cargo. Please type Y for yes or N for no...");
            }

            return retVal;
        }
    }
}
