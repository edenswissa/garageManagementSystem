using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private string m_Model;
        private string m_LicenceNumber;
        private List<Wheel> m_Wheels = new List<Wheel>();
        private Engine m_Engine;
        private string m_PathOfFileWithBasicQuestionsToAskUser = @"c:\Temp\i_QuestionsToGetVehicleProps.txt";
        private string m_PathOfFileWithAdditionalQuestionsToAskUser;

        public Vehicle(int i_NumOfWheels, int i_MaxAirPressureInWheels, string i_PathOfFileWithAdditionalPropsQuestions)
        {
            for (int i = 0; i < i_NumOfWheels; i++)
            {
                m_Wheels.Add(new Wheel(i_MaxAirPressureInWheels));
            }

            m_PathOfFileWithAdditionalQuestionsToAskUser = i_PathOfFileWithAdditionalPropsQuestions;
        }

        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }

        public string LicenceNumber
        {
            get
            {
                return m_LicenceNumber;
            }

            set
            {
                m_LicenceNumber = value;
            }
        }

        public List<Wheel> Wheels
        {
            get
            {
                return m_Wheels;
            }

            set
            {
                m_Wheels = value;
            }
        }

        public Engine Engine
        {
            get
            {
                return m_Engine;
            }

            set
            {
                m_Engine = value;
            }
        }

        public string PathOfFileWithBasicQuestionsToAskUser
        {
            get { return m_PathOfFileWithBasicQuestionsToAskUser; }
            set { m_PathOfFileWithBasicQuestionsToAskUser = value; }
        }

        public string PathOfFileWithAdditinalQuestionsToAskUser
        {
            get { return m_PathOfFileWithAdditionalQuestionsToAskUser; }
            set { m_PathOfFileWithAdditionalQuestionsToAskUser = value; }
        }

        public class Wheel
        {
            private string m_Manufacturer;
            private float m_CurrentAirPressure;
            private float m_MaxAirPressure;

            public Wheel(float i_MaxAirPressure)
            {
                m_MaxAirPressure = i_MaxAirPressure;
                m_Manufacturer = "";
                m_CurrentAirPressure = 0;
            }

            public string Manufacturer
            {
                get { return m_Manufacturer; }
                set { m_Manufacturer = value; }
            }

            public float CurrentAirPressure
            {
                get { return m_CurrentAirPressure; }
            }

            public float MaxAirPressure
            {
                get { return m_MaxAirPressure; }
                set { m_MaxAirPressure = value; }
            }

            public void BlowWheel(float i_AirPressureToAdd)
            {
                if (m_CurrentAirPressure + i_AirPressureToAdd > m_MaxAirPressure)
                {
                    m_CurrentAirPressure = m_MaxAirPressure;
                }
                else
                {
                    m_CurrentAirPressure += i_AirPressureToAdd;
                }
            }

            public string GetPropsForDisplay()
            {
                string theAdditionalPropsDisplay = string.Format
(@"Current Air Pressure :   {0}
Wheels Manufacturer :   {1}
", CurrentAirPressure, Manufacturer);

                return theAdditionalPropsDisplay;
            }
        }

        public abstract void InitializeVehicleWithAdditionalProps(string i_PathOfFileWithValuesToClassMembers);

        public abstract string GetVehicleAdditionalPropsForDisplay();

        public string GetVehicleBasicPropsForDisplay()
        {
            string theBasicPropsDisplay = string.Format
(@"Model    :   {0}
", Model);
            theBasicPropsDisplay = theBasicPropsDisplay + Wheels[0].GetPropsForDisplay() + Engine.GetEngineBasicPropsForDisplay();

            return theBasicPropsDisplay;
        }
        
        public void BlowWheelsToMax()
        {
            foreach(Wheel wheel in m_Wheels)
            {
                wheel.BlowWheel(wheel.MaxAirPressure);
            }
        }

        private void setManufacturerToAllWheels(string i_Manufacturer)
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.Manufacturer = i_Manufacturer;
            }
        }

        public void InitializeVehicleWithBasicProps(string i_PathOfFileToGetAnswers)
        {
            const int k_NumOfBasicProps = 4;
            string[] lines = new string[k_NumOfBasicProps];

            using (StreamReader answersFile = new StreamReader(i_PathOfFileToGetAnswers, true))
            {
                for (int i = 0; i < k_NumOfBasicProps; i++)
                {
                    lines[i] = answersFile.ReadLine();
                }
            }

            this.Model = lines[0];
            this.Engine.EnerngyRemained = float.Parse(lines[1]);
            setManufacturerToAllWheels(lines[2]);
        }
    }
}
