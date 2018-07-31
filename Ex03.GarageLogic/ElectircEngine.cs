using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectircEngine : Engine
    {
        public void ChargeBattery(float i_HoursToFill)
        {
            if (this.EnerngyRemained + i_HoursToFill > this.MaxEnergy)
            {
                throw new ValueOutOfRangeException(this.MaxEnergy, 0, "Input is out of range");
            }
            else
            {
                this.EnerngyRemained += i_HoursToFill;
            }
        }

        public override string GetEngineBasicPropsForDisplay()
        {
            return base.GetEngineBasicPropsForDisplay();
        }
    }
}
