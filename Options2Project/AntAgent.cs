﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Threading;
using System.Drawing;

using SOFT152SteeringLibrary;

namespace SOFT152Steering
{
    class AntAgent
    {

        /// <summary>
        /// Current postion of the agent, updated by the three
        /// movment methods
        /// </summary>
        private SOFT152Vector agentPosition;

        /// <summary>
        /// The random object passed to the agent. 
        /// Used only in the Wander() method to generate a 
        /// random direction to move in
        /// </summary>
        private Random randomNumberGenerator;

        // --------------------------------------------
        // Private fields 
        /// <summary>
        /// used in conjunction with the Wander() method
        /// to detemin the next position an agent should be in 
        /// Should remain a private field and do not edit within this class
        /// </summary>
        private SOFT152Vector wanderPosition;

        /// <summary>
        /// The size of the world the agent lives on as a Rectangle object.
        /// Used in conjunction with ShouldStayInWorldBounds, which if true
        /// will mean the agents position will be kept within the world bounds 
        /// (i.e. the  world width or the world height)
        /// </summary>
        private Rectangle worldBounds;

        public AntAgent(SOFT152Vector position, Random random)
        {
            agentPosition = new SOFT152Vector(position.X, position.Y);

            randomNumberGenerator = random;

            InitialiseAgent();
        }

        // random number used for wandering
        public AntAgent(SOFT152Vector position, Random random, Rectangle bounds)
        {
            agentPosition = new SOFT152Vector(position.X, position.Y);

            worldBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            randomNumberGenerator = random;

            InitialiseAgent();
        }

        public SOFT152Vector AgentPosition
        {
            set
            {
                agentPosition = value;
            }

            get
            {
                return agentPosition;
            }
        }

        /// <summary>
        /// The speed of the agent as used in all three movment methods 
        /// Ideal value depends on timer tick interval and realistic motion of
        /// agents needed. Suggest though in range 0 ... 2
        /// </summary>
        public double AgentSpeed { set; get; }

        /// <summary>
        /// If the agent is using the the ApproachAgent() method, this property defines
        /// at what point the agent will reduce the speed of approach to miminic a 
        /// more realistic approach behaviour
        /// </summary>
        public double ApproachRadius { set; get; }

        public double AvoidDistance { set; get; }

        /// <summary>
        /// Whether the agent is carrying food or not
        /// </summary>
        public bool HasFood { set; get; }
        /// <summary>
        /// Whether or not the ant knows the location of a foodObject
        /// </summary>
        public bool KnowsFood { set; get; }

        /// <summary>
        /// Whether or not the ant knows the location of a nest
        /// </summary>
        public bool KnowsNest { set; get; }

        /// <summary>
        /// The Vector that corresponds the location of the nearest FoodObject
        /// </summary>
        public SOFT152Vector NearestFood { set; get; }

        /// <summary>
        /// The Vector that corresponds the location of the nearest nest
        /// </summary>
        public SOFT152Vector NearestNest { set; get; }

        /// <summary>
        /// Used in conjunction worldBounds to determine if
        /// the agents position will stay within the world bounds 
        /// </summary>
        public bool ShouldStayInWorldBounds { set; get; }

        /// <summary>
        /// Property defines how 'random' the agent movement is whilst 
        /// the agent is using the Wander() method
        /// Suggest range of WanderLimits is 0 ... 1
        /// </summary>
        public double WanderLimits { set; get; }
        /// <summary>
        /// Causes the agent to make one step towards the object at objectPosition
        /// The speed of approach will reduce one this agent is within
        /// an ApproachRadius of the objectPosition
        /// </summary>
        /// <param name="agentToApproach"></param>
        public void Approach(SOFT152Vector objectPosition)
        {

            Steering.MoveTo(agentPosition, objectPosition, AgentSpeed, ApproachRadius);

            StayInWorld();
        }

        /// <summary>
        /// Deposits the food that ant has
        /// </summary>
        public void DepositFood()
        {
            HasFood = false;
        }

        /// <summary>
        /// Returns the Distance between the ant and a given SOFT152Vector
        /// </summary>
        /// <param name="objectPosition">Current position of SOFT152Vector that you want to work out the distance to</param>
        /// <returns>The distance between the ant agent and the SOFT152Vector</returns>
        public double DistanceTo(SOFT152Vector objectPosition)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow((agentPosition.X - objectPosition.X), 2) + Math.Pow((agentPosition.Y - objectPosition.Y), 2));

            return distance;
        }

        /// <summary>
        /// Causes the agent to make one step away from  the objectPosition
        /// The speed of avoid is goverened by this agents speed
        /// </summary>
        public void FleeFrom(SOFT152Vector objectPosition)
        {

            Steering.MoveFrom(agentPosition, objectPosition, AgentSpeed, AvoidDistance);

            StayInWorld();
        }

        public void GrabFood()
        {
            HasFood = true;
        }

        /// <summary>
        /// Causes the agent to make one random step.
        /// The size of the step determined by the value of WanderLimits
        /// and the agents speed
        /// </summary>
        public void Wander()
        {
            Steering.Wander(agentPosition, wanderPosition, WanderLimits, AgentSpeed, randomNumberGenerator);

            StayInWorld();
        }

        // To keep track of the obejcts bounds i.e. ViewPort dimensions
        /// <summary>
        /// Initialises the Agents various fields
        /// with default values
        /// </summary>
        private void InitialiseAgent()
        {
            wanderPosition = new SOFT152Vector();

            ApproachRadius = 10;

            AvoidDistance = 25;

            AgentSpeed = 1.0;

            ShouldStayInWorldBounds = true;

            WanderLimits = 0.5;
        }
        private void StayInWorld()
        {
            // if the agent should stay with in the world
            if (ShouldStayInWorldBounds == true)
            {
                // and the world has a positive width and height
                if (worldBounds.Width >= 0 && worldBounds.Height >= 0)
                {
                    // now adjust the agents position if outside the limits of the world
                    if (agentPosition.X < 0)
                        agentPosition.X = worldBounds.Width;

                    else if (agentPosition.X > worldBounds.Width)
                        agentPosition.X = 0;

                    if (agentPosition.Y < 0)
                        agentPosition.Y = worldBounds.Height;

                    else if (AgentPosition.Y > worldBounds.Height)
                        agentPosition.Y = 0;
                }
            }
        }
    }  // end class AntAgent
}
