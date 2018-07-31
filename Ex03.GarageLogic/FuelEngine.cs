using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private eFuelTypeName m_FuelTypeName;

        internal eFuelTypeName FuelTypeName
        {
            get
            {
                return m_FuelTypeName;
            }

            set
            {
                m_FuelTypeName = value;
            }
        }

        public void FillFuel(float i_LittersToFill, eFuelTypeName i_FuelType)
        {
            if (this.EnerngyRemained + i_LittersToFill > this.MaxEnergy)
            {
                throw new ValueOutOfRangeException(this.MaxEnergy, 0, "Input is out of range");
            }
            if (this.FuelTypeName != i_FuelType)
            {
                throw new ArgumentException("Not the same type of fuel");
            }
            else
            {
                this.EnerngyRemained += i_LittersToFill;
            }
        }

        public override string GetEngineBasicPropsForDisplay()
        {
            string theAdditionalPropsDisplay = string.Format
(@"Type of Fuel :   {0}
",FuelTypeName.ToString());
            theAdditionalPropsDisplay = base.GetEngineBasicPropsForDisplay() + theAdditionalPropsDisplay;

            return theAdditionalPropsDisplay;
        }

        public enum eFuelTypeName
        {
            Octan95 = 1,
            Octan96,
            Octan98,
            Soler,
        }
    }
}
