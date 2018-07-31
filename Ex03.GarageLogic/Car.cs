using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private eColor m_Color;
        private eNumOfDoors m_NumOfDoors;

        internal Car(Engine i_Engine) : base(4, 34, @"c:\Temp\i_QuestionsToGetCarProps.txt")
        {
            //const int k_NumOfWheels = 4;
            //const float k_MaxAirPressureInWheels = 34;
            
            if (i_Engine is FuelEngine)
            {
                const float k_MaxFuelLiters = 43;
                (i_Engine as FuelEngine).FuelTypeName = FuelEngine.eFuelTypeName.Octan95;
                (i_Engine as FuelEngine).MaxEnergy = k_MaxFuelLiters;
            }
            else if (i_Engine is ElectircEngine)
            {
                const float k_MaxElectricityHours = (float)3.6;
                (i_Engine as ElectircEngine).MaxEnergy = k_MaxElectricityHours;
            }
            
            base.Engine = i_Engine;
        }

        public eColor Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        public eNumOfDoors NumOfDoors
        {
            get
            {
                return m_NumOfDoors;
            }

            set
            {
                m_NumOfDoors = value;
            }
        }

        public enum eColor
        {
            Silver = 1,
            Blue = 2,
            Black = 3,
            White = 4,
        }

        public enum eNumOfDoors
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
        }

        public override void InitializeVehicleWithAdditionalProps(string i_PathOfFileWithValuesToClassMembers)
        {
            string[] valsForMembers = File.ReadAllLines(i_PathOfFileWithValuesToClassMembers);
          
            //[0] is color:
            m_Color = getColorFromString(valsForMembers[0]);
            //[1] is num of doors:
            m_NumOfDoors = getDoorsNumFromString(valsForMembers[1]);
        }

        public override string GetVehicleAdditionalPropsForDisplay()
        {
            string theAdditionalPropsDisplay = string.Format
(@"Color    :   {0}
Number of Doors :   {1}
", Color, NumOfDoors);

            return theAdditionalPropsDisplay;
        }

        private eColor getColorFromString(string i_ValStr)
        {
            eColor color = new eColor();
            Enum.TryParse(i_ValStr, out color);

            switch(color)
            {
                case(eColor.Black):
                    break;
                case(eColor.Blue):
                    break;
                case(eColor.Silver):
                    break;
                case(eColor.White):
                    break;
                default:
                    throw new ArgumentException("No such color option!");
            }

            return color;
        }

        private eNumOfDoors getDoorsNumFromString(string i_ValStr)
        {
            eNumOfDoors doorsNum = new eNumOfDoors();
            Enum.TryParse(i_ValStr, out doorsNum);

            switch(doorsNum)
            {
                case (eNumOfDoors.Two):
                    break;
                case (eNumOfDoors.Three):
                    break;
                case (eNumOfDoors.Four):
                    break;
                case (eNumOfDoors.Five):
                    break;
                default:
                    throw new ArgumentException("Number of doors is not optional!");
            }

            return doorsNum;
        }
    }
}
