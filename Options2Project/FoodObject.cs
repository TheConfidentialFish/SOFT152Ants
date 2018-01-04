using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOFT152SteeringLibrary;

namespace SteeringProject
{
    /// <summary>
    /// FoodObject 
    /// </summary>
    class FoodObject : SOFT152Vector
    {
        /// <summary>
        /// Function that returns true or false depending on whether there is food left
        /// </summary>
        /// <returns>Returns true if there is food, otherwise it returns false</returns>
        public bool FoodLeft()
        {
            if (FoodAmount < 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Getter and Setter Methods for FoodAmount
        /// </summary>
        public int FoodAmount
        {
            set
            {
                FoodAmount = value;
            }
            get
            {
                return FoodAmount;
            }

        }


        /// <summary>
        /// Initiates the FoodObject with a standard amount of food for ants to take
        /// </summary>
        public FoodObject()
        {
            FoodAmount = 8;
        }

        /// <summary>
        /// Initiates the FoodObject with a specified location
        /// </summary>
        /// <param name="X">The specified X Coordinate</param>
        /// <param name="Y">The specified Y Coordinate</param>
        public FoodObject(double xLocation,double yLocation)
        {
            FoodAmount = 8;

            X = xLocation;
            Y = yLocation;
        }
        
        /// <summary>
        /// Initiates the FoodObject with a specified amount of food for ants to take
        /// </summary>
        /// <param name="amount">The amount of food for the ants to take</param>
        public FoodObject(int amount)
        {
            FoodAmount = amount;            
        }

        /// <summary>
        /// Called when food is taken. If no food is left, the FoodObject is then deactivated
        /// </summary>
        public void takeFood()
        {           
            FoodAmount--;//Decrement the amount of food available 

            if (FoodAmount < 1)
            {
                //code to deactive the instance of the class
            }
        }
    }
}
