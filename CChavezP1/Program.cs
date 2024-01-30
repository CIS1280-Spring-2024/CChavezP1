using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Xml.Linq;
using System;
using System.Collections.Specialized;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace CChavezP1
{
    internal class Program
    {
        //Displays header for pressure calc program.
        static void DisplayHeader()
        {
            Console.WriteLine(".NET I/C SHARP (U01) Class Header\nProgramed by: Colby Chavez\nEmail:cchavez572@cnm.edu\nProgram goal: The program will use the Ideal Gas Equation to will calculate the pressure\nexerted by a gas in a container. All from inputs selected by the user.\nExciting right?\n\nFirst, here is a list of all the gasses aviable for you to calulate!\n\n");
        }
        static void GetMolecularWeights(ref string[] gasNames, ref double[] molecularWeights, out int count)
        //GETMOLECULARWEIGHTS
        //  This method gets Molecular Weights arrays from file.
        //  “gasNames" is a string array that will be filled by the function with gas names.
        //  "molecularWeights" is a double array that will be filled by this function with molecular weights.
        //  "count" is a double that is passed in as an out parameter and will be loaded with the count of elements in array.
        //        Use a StreamReader to open the file and read its contents.You can use the string split function to split the data on a comma.

        {
            string filePath = "MolecularWeightsGasesAndVapors.csv";
            StreamReader reader = null;
            count = 0;
            if (File.Exists(filePath))
            {
                reader = new StreamReader(File.OpenRead(filePath));
                reader.ReadLine(); // this line Discards header row from CSV
                while (!reader.EndOfStream)
                {
                    var inputLine = reader.ReadLine();
                    var inputValues = inputLine.Split(',');
                    gasNames[count] = inputValues[0];
                    molecularWeights[count] = double.Parse(inputValues[1]);
                    count++;
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }

        }

        private static void DisplayGasNames(string[] gasNames, int countGases)
        //private static void DisplayGasNames(string[] gasNames, int countGases)
        //“gasNames” is an array of strings with gas names.
        //“countGases is the count of names in the array.
        //This function should display the names of the gases to the console in three columns.
        {
            countGases = 0;
            int outputCount = 0;
            string lastGasLenthBuffer;
            Console.WriteLine("---------------------AVAILABLE GASSES---------------------");
            for (int i = 0; i < gasNames.Length; i++)
            {
                if (outputCount == 0)
                {

                    Console.Write("{0,-20}", gasNames[i]);
                    countGases++;
                    outputCount++;
                }
                else if (outputCount == 1)
                {
                    Console.Write("{0,-20}", gasNames[i]);
                    countGases++;
                    outputCount++;

                }
                else
                {
                    Console.Write("{0,-20} \n", gasNames[i]);
                    countGases++;
                    outputCount = 0;
                }

            }
            Console.WriteLine("\n----------------------------------------------------\n");
        }
        private static double GetMolecularWeightFromName(string gasName, string[] gasNames, double[] molecularWeights, out int countGases) //  DONE: countGasses should be an out or ref variable. RJG    
        //This function looks up the name of a gas in an array of gas names then returns the molecular weight of that gas in mols
        //“gasName” is the name of gas to search for as a string.
        //“gasNames" is an array of gas names
        //“molecularWeights" is an array of molecular weights. This array is parallel to the gasNames array.
        //"countGases" is the count of elements in gasNames and molecularWeights arrays.
        {
            countGases = 0;
            for (int i = 0; i < gasNames.Length; i++)
            {

                if (gasNames[i] == gasName)
                {
                    return molecularWeights[i];
                }

            }
            //TODO: Because this method is a double it needs to return something. You could return 0 or -1 to indicate an error. RJG

            return 0;
        }
        static double Pressure(double mass, double vol, double temp, double molecularWeight)
        //  Given mass, volume, temperature and molecular weight returns pressure of a gas in pascals.
        //  "mass" is mass in grams.
        //  "vol" is volume in cubic meters.
        //  “temp" is temperature in celcius.
        //  This method will use the parameters to calculate pressure of the gas.
        //  Pressure will call NumberOfMoles to get n for the formula.
        //  This method will call CelciusToKelvin to convert degrees celcius into degrees
        //  kelvin since the formula requires temperature to be in degrees kelvin.
        {
            double pressureOutput;
            double n = NumberOfMoles(mass, molecularWeight);
            double r = 8.3145; // constant value using metric units
            double kTemp = CelciusToKelvin(temp);
            pressureOutput = n * r * kTemp / vol;
            return pressureOutput;
        }
        static double NumberOfMoles(double mass, double molecularWeight)
        //Given the mass of a gas and the molecular weight of that gas, returns the number of moles of air from mass.
        //"mass" is mass of the gas in grams.
        //“molecularWeight” is the molecular weight of the gas in mols.
        {
            double outputMoles = mass / molecularWeight;
            return outputMoles;
        }
        static double CelciusToKelvin(double celcius)
        //Converts Celsius to kelvin.
        //Prototype: static double CelciusToKelvin(double celcius)
        //“celcius" is degrees Celsius.
        {
            double kelvin = celcius + 273.15;
            return kelvin;
        }
        private static void DisplayPresure(double pressure)
        //Given pressure in Pascals, this function displays pressure in Pascals and PSI to the consol.
        //"pressure" is pressure in Pascals.
        //This function will call PaToPsi to convert the pressure passed to it in Pascals to PSI.
        //It should display pressure in both Pascals and PSI.
        {

            Console.Write($"The pressure in pascals is: {pressure}\n");
            double psiPressure = PaToPSI(pressure);
            Console.WriteLine($"The pressure in PSI is: {psiPressure}\n");
        }

        static double PaToPSI(double pascals)
        //Converts Pascals to PSI.
        //"pascals" is the pressure in pascals.
        //The method returns Pressure in PSI as a double.
        {
            return pascals * 0.000145038; // The onversion I found online for this was 1 pascal = 0.000145038 PSI
        }

        static bool DoAnother()
        {
            string doAnother = " ";

            Console.WriteLine($"\nWould you like to do another loop(yes or no)?");
            doAnother = Console.ReadLine();
            return doAnother == "yes";
        }

        static void Main(string[] args)
        {
            //  The main method will do the following:
            //  Declare double arrays for gas names and molecular weights.
            string[] gasNames = new string[100];
            double[] molecularWeights = new double[100];
            //  Declare an int to keep track of number of elements in the arrays.
            int count;
            // Variable declaration
            int countGases = 0;
            double mass = 0;
            double vol = 0;
            double temp = 0;
            double molecularWeight = 0;
            string gasName = "";
            //  Call DisplayHeader to show the program header.
            DisplayHeader();
            //  Call GetMolecularWeights to fill the arrays and get the count of items in the list.
            GetMolecularWeights(ref gasNames, ref molecularWeights, out count); //  DONE: countGasses should be an out or ref variable. RJG
            //Call DisplayGasNames to display the gas names to the user in three columns.
            DisplayGasNames(gasNames, countGases);
            //In a do another loop do the following:
            do
            {
                //  Ask the user the name of the gas.
                Console.WriteLine($"\nWhich of the gasses would you like to run a calculation on?\n");
                gasName = Console.ReadLine();

                //  Use GetMolecularWeightFromName method to get the molecular weight of the gas
                //  selected by the user.
                molecularWeight = GetMolecularWeightFromName(gasName, gasNames, molecularWeights, out countGases);
                if (molecularWeight == 0)
                {
                    Console.WriteLine("You did not select a gas on the list.");
                }
                else
                {
                    //  If the gas is not found display an error message, and drop out to the do another loop.
                    //  Ask the user for the volume of gas in cubic meters, mass of the gas in grams and temperature in celcius.
                    string inputBuffer;
                    Console.WriteLine($"\nWhat is the volume of {gasName} in cubic meters?\n");
                    inputBuffer = Console.ReadLine();
                    vol = double.Parse(inputBuffer);
                    Console.WriteLine($"\nWhat is the mass of {gasName} in grams?\n");
                    inputBuffer = Console.ReadLine();
                    mass = double.Parse(inputBuffer);
                    Console.WriteLine($"\nWhat is the tempeture of the {gasName} in celcius?\n");
                    string volBuffer = Console.ReadLine();
                    temp = double.Parse(inputBuffer);

                    //Use the Pressure method to get the pressure of the gas in Pascals.
                    double pressure = Pressure(mass, vol, temp, molecularWeight);
                    //Pass the pressure in pascals to the DisplayPressure method to display the pressure in Pascals and PSI.
                    DisplayPresure(pressure);
                    //Ask the user if they want to do another.
                }

            } while (DoAnother());

            //Display a good bye message when they are done.
            Console.WriteLine($"\nThanks for using CChavezP1 for your gas calulation needs\n");
        }
    }
}
