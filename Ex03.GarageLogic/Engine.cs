using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Engine
    {
        private float m_EnergyRemained = 0;
        private float m_MaxEnergy;
        private float m_EnergyRemainedPrecent = 0;

        public float EnerngyRemained
        {
            get
            {
                return m_EnergyRemained;
            }

            set
            {
                if (value > m_MaxEnergy)
                {
                    throw new ValueOutOfRangeException(m_MaxEnergy, 0, "Remained energy can't be more than the maximum");
                }

                m_EnergyRemained = value;
                setEnergyReaminedPrecent(); 
            }
        }

        public float MaxEnergy
        {
            get
            {
                return m_MaxEnergy;
            }

            set
            {
                m_MaxEnergy = value;
            }
        }

        public float EnergyRemainedPrecent
        {
            get
            {
                return m_EnergyRemainedPrecent;
            }

        }

        public void setEnergyReaminedPrecent()
        {
            m_EnergyRemainedPrecent = (m_EnergyRemained * 100) / m_MaxEnergy;
        }

        public virtual string GetEngineBasicPropsForDisplay()
        {
            string theBasicPropsDisplay = string.Format
(@"Energy Remained  :   {0}
Max Energy  :   {1}
", EnerngyRemained, MaxEnergy);

            return theBasicPropsDisplay;
        }
    }
}
