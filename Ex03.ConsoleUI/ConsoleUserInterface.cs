using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;
using System.IO;

namespace Ex03.ConsoleUI
{
    public class GarageConsoleUserInterface
    {
        private GarageManager m_Garage = new GarageManager();
        private const string k_PathOfFileWithAnswersToQuestions = @"c:\Temp\io_AnswersToGetVehicleProps.txt";

        public enum eManuOption
        {
            EnterVehicle = 1,
            ShowLicenceNumbers,
            ChangeVehicleStatus,
            BlowAirInVehicle,
            FillFuelInVehicle,
            ChargeElectricVehicleBattery,
            ShowVehicleDetails,
            Exit
        }

        public void RunGarageProgram()
        {
            eManuOption userChoice;
            bool userWantsToExit = false;

            do
            {
                try
                {
                    userChoice = displayMainManu();
                    userWantsToExit = false;

                    switch (userChoice)
                    {
                        case (eManuOption.EnterVehicle):
                            enterAnOrderToGarageFromMainManu();
                            break;
                        case (eManuOption.ShowLicenceNumbers):
                            showAllVehiclesLicenseNumberWithFiltering();
                            break;
                        case (eManuOption.ChangeVehicleStatus):
                            changeVehicleStatusFromMainManu();
                            break;
                        case (eManuOption.BlowAirInVehicle):
                            blowWheelsToMaxFromMainMenu();
                            break;
                        case (eManuOption.FillFuelInVehicle):
                            fillFuelInVehicleFromMainManu();
                            break;
                        case (eManuOption.ChargeElectricVehicleBattery):
                            chargeBatteryFromMainMenu();
                            break;
                        case (eManuOption.ShowVehicleDetails):
                            showDetailsOfVehicle();
                            break;
                        case (eManuOption.Exit):
                            userWantsToExit = true;
                            break;
                        default:
                            throw new FormatException("There is no such option");
                    }
                }

                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                }

                catch (Exception nonFormatException)
                {
                    Console.WriteLine(nonFormatException.Message);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                }
            }
            while (!userWantsToExit);
        }

        private eManuOption displayMainManu()
        {
            eManuOption choice;
            const string k_mainManu =
            @"
Hi! What would you like to do?

1. Enter a vehicle to the garage
2. Show vehicles' licence numbers
3. Change a vehicle's status
4. Blow air in a vehicle's tiers
5. Fill Fuel in a vehicle
6. Charge an electric vehicle
7. Show vehicle's details
8. Exit

            ";

            Console.WriteLine(k_mainManu);
            string choiceStr = Console.ReadLine();
            if (!Enum.TryParse(choiceStr, out choice))
            {
                throw new FormatException("Input not valid");
            }

            return choice;
        }

        private void enterAnOrderToGarageFromMainManu()
        {
            bool inputIsOk = true;

            do
            {
                try
                {
                    inputIsOk = true;
                    Console.WriteLine("Please enter licence number:");
                    string licenceOfVehicleToAdd = Console.ReadLine();
                    if (licenceOfVehicleToAdd.Length <= 0)
                    {
                        throw new FormatException("Input for licence number has not been received");
                    }

                    if (m_Garage.OrdersDictionary.ContainsKey(licenceOfVehicleToAdd))
                    {//vehicle already in garage
                        Console.WriteLine("Vehicle is already in the garage");
                        m_Garage.ChangeVehicleStatus(licenceOfVehicleToAdd, Order.eVehicleStatus.FixingUp);
                    }
                    else
                    {//vehicle is not in the garage
                        Order lastAddedOrder = enterOrderDetailsToGarage(licenceOfVehicleToAdd);
                        enterVehicleDetails(lastAddedOrder.TheVehicle);
                        Console.WriteLine("Your vehicle was entered to the garage successfully");
                    }
                }

                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    inputIsOk = false;
                }
            }
            while (!inputIsOk);
        }

        private Order enterOrderDetailsToGarage(string i_LicenseNumOfRelevantVehicle)
        {
            int dummyResult;
            bool inputIsOk = true;
            Order currentOrder = new Order();

            do
            {
                try
                {
                    inputIsOk = true;
                    Console.WriteLine("Owner name: ");
                    string ownerName = Console.ReadLine();
                    if (ownerName.Length <= 0)
                    {
                        throw new FormatException("Input for owner name has not been received");
                    }

                    Console.WriteLine("Phone number: ");
                    string phoneNumber = Console.ReadLine();
                    if (int.TryParse(phoneNumber, out dummyResult) == false)
                    {
                        throw new FormatException("Phone number must contain digits only");
                    }

                    Console.WriteLine("Please choose type of vehicle:");
                    foreach (VehicleCreator.eVehicleType vehicleType in Enum.GetValues(typeof(VehicleCreator.eVehicleType)))
                    {
                        Console.WriteLine(String.Format("press {0} for {1}", (int)vehicleType, vehicleType.ToString()));
                    }

                    string enteredVehicleType = Console.ReadLine();
                    m_Garage.AddOrderToGarage(i_LicenseNumOfRelevantVehicle, ownerName, phoneNumber, enteredVehicleType, ref currentOrder);
                }

                catch (Exception exception)
                {
                    inputIsOk = false;
                    Console.WriteLine(exception.Message);
                }
            }
            while (!inputIsOk);

            return currentOrder;
        }

        private void enterVehicleDetails(Vehicle i_LastAddedToGarageVehicle)
        {
            bool inputIsOk = true;

            do
            {
                try
                {
                    inputIsOk = true;
                    //basic vehicle details
                    ReadQuestionsFromFileAndGetAnswers(i_LastAddedToGarageVehicle.PathOfFileWithBasicQuestionsToAskUser);
                    i_LastAddedToGarageVehicle.InitializeVehicleWithBasicProps(k_PathOfFileWithAnswersToQuestions);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                    //additional specific vehicle details
                    ReadQuestionsFromFileAndGetAnswers(i_LastAddedToGarageVehicle.PathOfFileWithAdditinalQuestionsToAskUser);
                    i_LastAddedToGarageVehicle.InitializeVehicleWithAdditionalProps(k_PathOfFileWithAnswersToQuestions);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                }

                catch (ValueOutOfRangeException outOfRangeException)
                {
                    Console.WriteLine(outOfRangeException.Message);
                    Console.WriteLine("Note: The max is {0}", outOfRangeException.MaxValue);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                    inputIsOk = false;
                }

                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    File.Delete(k_PathOfFileWithAnswersToQuestions);
                    inputIsOk = false;
                }
            }
            while (!inputIsOk);
        }

        public void ReadQuestionsFromFileAndGetAnswers(string i_PathOfFileWithQuestionsToAskUser)
        {
            string answer;
            string question;
            string requiredAnswerType;

            string[] lines = File.ReadAllLines(i_PathOfFileWithQuestionsToAskUser);
            foreach (string line in lines)
            {
                getQuestionToDisplayAndReqAnswerType(line, out question, out requiredAnswerType);
                do
                {
                    Console.WriteLine(question);
                    answer = Console.ReadLine();
                }
                while (!answerIsInCorrectAnswertype(answer, requiredAnswerType));
                
                //add the answer to answers file:
                using (StreamWriter answersFile = new StreamWriter(k_PathOfFileWithAnswersToQuestions, true))
                {
                    answersFile.WriteLine(answer);
                }
            }
            //Important! - always after done using answers file delete it!
        }

        private void getQuestionToDisplayAndReqAnswerType(string lineFromFile, out string question, out string requiredAnswerType)
        {
            int indexOfFirstSpace = lineFromFile.IndexOf(" ");

            requiredAnswerType = lineFromFile.Substring(0, indexOfFirstSpace);
            question = lineFromFile.Substring(indexOfFirstSpace + 1);
        }

        private bool answerIsInCorrectAnswertype(string answer, string requiredAnswerType)
        {
            bool answerInCorrectAnswerType = true;
            int intdummyResult;
            float floatdummyResult;

            if (requiredAnswerType.Equals("int"))
            {
                answerInCorrectAnswerType = int.TryParse(answer, out intdummyResult);
            }
            else if (requiredAnswerType.Equals("float"))
            {
                answerInCorrectAnswerType = float.TryParse(answer, out floatdummyResult);
            }

            return answerInCorrectAnswerType;
        }

        private void changeVehicleStatusFromMainManu()
        {
            bool inputIsOk = true;

            do
            {
                try
                {
                    inputIsOk = true;
                    Console.WriteLine("Please type the licence number of the vehicle you'd like to change its status:");
                    string licenseNum = Console.ReadLine();
                    if (licenseNum.Length <= 0)
                    {
                        throw new FormatException("Input for licence number was not received!");
                    }

                    Console.WriteLine("The vehicle is currently in {0} status, Please choose its new status:", m_Garage.OrdersDictionary[licenseNum].VehicleStatus.ToString());
                    Console.WriteLine(
@"1. {0}
2. {1}
3. {2}", Order.eVehicleStatus.FixingUp.ToString(), Order.eVehicleStatus.Fixed.ToString(), Order.eVehicleStatus.Paid.ToString());
                    Order.eVehicleStatus userChoiceForNewStatus;
                    if (!Enum.TryParse(Console.ReadLine(), out userChoiceForNewStatus))
                    {
                        throw new FormatException("Not valid!");
                    }

                    m_Garage.ChangeVehicleStatus(licenseNum, userChoiceForNewStatus);
                    Console.WriteLine("Status was changed successfully");
                }

                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    inputIsOk = false;
                }
            }
            while (!inputIsOk);
        }

        private void showAllVehiclesLicenseNumberAndStatus()
        {
            foreach(KeyValuePair<string,Order> order in m_Garage.OrdersDictionary)
            {
                Console.WriteLine(String.Format("License Number: {0} Status: {1}",order.Key, order.Value.VehicleStatus.ToString()));
            }
        }

        private void showFixingUpVehiclesLicenseNumber()
        {
            foreach (KeyValuePair<string, Order> order in m_Garage.FixingUpVehicles)
            {
                Console.WriteLine(String.Format("License Number: {0} Status: {1}", order.Key, order.Value.VehicleStatus.ToString()));
            }
        }

        private void showPaidVehiclesLicenseNumber()
        {
            foreach (KeyValuePair<string, Order> order in m_Garage.PaidVehicles)
            {
                Console.WriteLine(String.Format("License Number: {0} Status: {1}", order.Key, order.Value.VehicleStatus.ToString()));
            }
        }

        private void showFixedVehiclesLicenseNumber()
        {
            foreach (KeyValuePair<string, Order> order in m_Garage.FixedgVehicles)
            {
                Console.WriteLine(String.Format("License Number: {0} Status: {1}", order.Key, order.Value.VehicleStatus.ToString()));
            }
        }

        private void showAllVehiclesLicenseNumberWithFiltering()
        {
            bool isInputOk = false;
            while (!isInputOk)
            {
                Console.WriteLine(@"
Which list do you want to see?
1.All vehicles
2.Paid vehicles
3.Fixing up vehicles
4.Fixed vehicles
5.Exit");

                string answer = Console.ReadLine();
                if (answer.Equals("1"))
                {
                    showAllVehiclesLicenseNumberAndStatus();
                    isInputOk = true;
                }
                else if (answer.Equals("2"))
                {
                    showPaidVehiclesLicenseNumber();
                    isInputOk = true;
                }
                else if (answer.Equals("3"))
                {
                    showFixingUpVehiclesLicenseNumber();
                    isInputOk = true;
                }
                else if (answer.Equals("4"))
                {
                    showFixedVehiclesLicenseNumber();
                    isInputOk = true;
                }
                else if (answer.Equals("5"))
                {
                    isInputOk = true;
                }
                else
                {
                    Console.WriteLine("Incorrect input , try again");
                    isInputOk = false;
                }
            }
        }

        private string getLicenseNumberFromUser()
        {
            string licenseNumber;
            bool isExist = true;
            do
            {
                Console.WriteLine("Enter vehicle license number:");
                licenseNumber = Console.ReadLine();
                isExist = true;

                if (m_Garage.OrdersDictionary == null || m_Garage.OrdersDictionary.Count == 0)
                {
                    throw new ArgumentException("No vehicles in garage...");
                }
                else if (!m_Garage.OrdersDictionary.ContainsKey(licenseNumber) || licenseNumber == null)
                {
                    Console.WriteLine("No such license number! try again...\nNote: The vehicles currently in the garage are");
                    showAllVehiclesLicenseNumberAndStatus();
                    isExist = false;
                }
            }
            while (!isExist);

            return licenseNumber;
        }

        private void fillFuelInVehicleFromMainManu()
        {
            bool inputIsOk = true;

            do
            {
                try
                {
                    inputIsOk = true;
                    string licenseNumber = getLicenseNumberFromUser();
                    Console.WriteLine("You have {0}% energy. You can fill up to {1} more. Please enter how may litters to fill: ", 
                                       m_Garage.OrdersDictionary[licenseNumber].TheVehicle.Engine.EnergyRemainedPrecent,
                                       (m_Garage.OrdersDictionary[licenseNumber].TheVehicle.Engine.MaxEnergy - m_Garage.OrdersDictionary[licenseNumber].TheVehicle.Engine.EnerngyRemained));
                    string littersToFillStr = Console.ReadLine();
                    float littersToFill;

                    if (!float.TryParse(littersToFillStr, out littersToFill))
                    {
                        throw new FormatException("Amount of litters is not valid!");
                    }

                    FuelEngine.eFuelTypeName fuelType = getFuelTypeFromUser();
                    m_Garage.FillFuelInVehicle(licenseNumber, fuelType, littersToFill);
                    Console.WriteLine("Your vehicle was filled successfully");
                }

                catch (ArgumentException aException)
                {
                    Console.WriteLine(aException.Message);
                }
                catch (ValueOutOfRangeException outOfRangeException)
                {
                    Console.WriteLine(outOfRangeException.Message);
                    Console.WriteLine("Note: The max is {0}.", outOfRangeException.MaxValue);
                    inputIsOk = false;
                }
                catch (Exception otherException)
                {
                    Console.WriteLine(otherException.Message);
                    inputIsOk = false;
                }
            }
            while (!inputIsOk);
        }

        private FuelEngine.eFuelTypeName getFuelTypeFromUser()
        {
            Console.WriteLine(
@"Please choose the kind of fuel:
1. Octan95,
2. Octan96,
3. Octan98,
4. Soler");
            string fuelTypeStr = Console.ReadLine();
            FuelEngine.eFuelTypeName fuelType;
            if (!Enum.TryParse(fuelTypeStr, out fuelType))
            {
                throw new FormatException("The input for choice is not valid");
            }

            if (!fuelType.Equals(FuelEngine.eFuelTypeName.Octan95) && !fuelType.Equals(FuelEngine.eFuelTypeName.Octan96) &&
                !fuelType.Equals(FuelEngine.eFuelTypeName.Octan98) && !fuelType.Equals(FuelEngine.eFuelTypeName.Soler))
            {
                throw new FormatException("No such fuel type option!");
            }

            return fuelType;
        }

        private void blowWheelsToMaxFromMainMenu()
        {
            try
            {
                string licenseNumber = getLicenseNumberFromUser();
                m_Garage.OrdersDictionary[licenseNumber].TheVehicle.BlowWheelsToMax();
                Console.WriteLine("Wheels maximum blowing was successful");
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }

        private float getHowMuchMinutesToCharge(string i_LicenseNumber)
        {

            float o_minutesToCharge;
            Console.WriteLine(String.Format("You have {0}% energy.You can fill up to {1} more. Please enter how many minutes to fill",
                              m_Garage.OrdersDictionary[i_LicenseNumber].TheVehicle.Engine.EnergyRemainedPrecent,
                              (m_Garage.OrdersDictionary[i_LicenseNumber].TheVehicle.Engine.MaxEnergy - m_Garage.OrdersDictionary[i_LicenseNumber].TheVehicle.Engine.EnerngyRemained)));
        
            string answer = Console.ReadLine();
            if (!float.TryParse(answer, out o_minutesToCharge))
            {
                throw new FormatException("Input not valid");
            }

            return o_minutesToCharge;
        }

        private void chargeBatteryFromMainMenu()
        {
            bool isInputOk = true;

            do
            {
                try
                {
                    isInputOk = true;
                    string licenseNumber = getLicenseNumberFromUser();
                    float minutesToCharge = getHowMuchMinutesToCharge(licenseNumber);
                    m_Garage.ChargeBattery(minutesToCharge, licenseNumber);
                    Console.WriteLine("Your vehicle was chargesd successfully");
                }

                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }

                catch (ValueOutOfRangeException outOfRangeException)
                {
                    Console.WriteLine(outOfRangeException.Message);
                    Console.WriteLine("Note: The max is {0}.", outOfRangeException.MaxValue);
                    isInputOk = false;
                }

                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    isInputOk = false;
                }
            }
            while (!isInputOk);
        }

        private void showDetailsOfVehicle()
        {
            try
            {
                string licenseNum = getLicenseNumberFromUser();
                string finalDisplay = m_Garage.OrdersDictionary[licenseNum].GetOrderPropsForDisplay();
                finalDisplay = finalDisplay + m_Garage.OrdersDictionary[licenseNum].TheVehicle.GetVehicleBasicPropsForDisplay();
                finalDisplay = finalDisplay + m_Garage.OrdersDictionary[licenseNum].TheVehicle.GetVehicleAdditionalPropsForDisplay();

                Console.WriteLine(finalDisplay);
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
