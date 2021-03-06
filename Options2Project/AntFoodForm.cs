﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOFT152SteeringLibrary;
using SteeringProject;




namespace SOFT152Steering
{
    

    public partial class AntFoodForm : Form
    {

        //set the number of ants (reccommend that this is not set higher than 128 to minimize graphics flickering)
        private static int noOfAnts = 300;

        // declare a sample agent
        
        private SOFT152Vector anotherObject;

        //make a group of ants dependant on the number of ants
        private AntAgent[] antGroup = new AntAgent[noOfAnts];


        // A bitmap image used for double buffering
        private Bitmap backgroundImage;

        private FoodObject foodObject;

        private NestObject nestObject;

        // the random object given to each Ant agent
        private Random randomGenerator;

        // Declare a stationary object
        private SOFT152Vector someObject;
        public AntFoodForm()
        {        
            InitializeComponent();

            CreateBackgroundImage();

            CreateAnts(); 
        }

        private void CreateAnts()
        {
            Rectangle worldLimits;

            // create a radnom object to pass to the ants
            randomGenerator = new Random();

            
            // define some world size for the ants to move around on
            // assume the size of the world is the same size as the panel
            // on which they are displayed
            worldLimits = new Rectangle(0, 0, drawingPanel.Width, drawingPanel.Height);

           

            for(int i=0; i < noOfAnts; i++)
            {
                antGroup[i] = new AntAgent(new SOFT152Vector(randomGenerator.NextDouble()*drawingPanel.Width, randomGenerator.NextDouble() * drawingPanel.Height), randomGenerator, worldLimits);
                antGroup[i].HasFood = false;

                antGroup[i].KnowsFood = false;
                antGroup[i].KnowsNest = false;

                //set moving values for all the ants in the antGroup
                antGroup[i].AgentSpeed = 1.0;
                antGroup[i].WanderLimits = 0.25;

                //keep all the antAgents in the antGroup in the world
                antGroup[i].ShouldStayInWorldBounds = true;
            }

            

            // create an object at a arbitary position
            someObject = new SOFT152Vector(250, 250);

            anotherObject = new SOFT152Vector(50, 50);
        }

        /// <summary>
        ///  Creates the background image to be used in double buffering 
        /// </summary>
        private void CreateBackgroundImage()
        {
            int imageWidth;
            int imageHeight;

            // the backgroundImage  can be any size
            // assume it is the same size as the panel 
            // on which the Ants are drawn
            imageWidth = drawingPanel.Width;
            imageHeight = drawingPanel.Height;

            backgroundImage = new Bitmap(drawingPanel.Width, drawingPanel.Height);
        }

        private void DrawAgents()
        {

            // using FillRectangle to draw the agents
            // so declare variables to draw with
            float agentXPosition;
            float agentYPosition;

            // some arbitary size to draw the Ant
            float antSize;
            float foodSize;

            antSize = 5.0f;
            foodSize = 2.5f;

            Brush solidBrush;

            // get the graphics context of the panel
            using (Graphics g = drawingPanel.CreateGraphics())
            {
                g.Clear(Color.White);

                

                
                for (int i = 0; i < noOfAnts; i++)
                {
                    // get the 1st agent position
                    agentXPosition = (float)antGroup[i].AgentPosition.X;
                    agentYPosition = (float)antGroup[i].AgentPosition.Y;

                    // create a brush
                    solidBrush = new SolidBrush(Color.DarkRed);

                    // draw the 1st agent
                    g.FillRectangle(solidBrush, agentXPosition, agentYPosition, antSize, antSize);



                    //draw the food for the 1st agent if they have any
                    if (antGroup[i].HasFood)
                    {
                        solidBrush = new SolidBrush(Color.Gold);
                        g.FillRectangle(solidBrush, agentXPosition, agentYPosition, foodSize, foodSize);

                    }
                }


                // now draw the stationary object
                // change colour of brush
                solidBrush = new SolidBrush(Color.Green);
                g.FillRectangle(solidBrush, (float)someObject.X, (float)someObject.Y, 20, 20);


                //draw second stationary object
                solidBrush = new SolidBrush(Color.PeachPuff);
                g.FillRectangle(solidBrush, (float)anotherObject.X, (float)anotherObject.Y, 20, 20);


            }

            // dispose of resources
            solidBrush.Dispose();
        }

        /// <summary>
        /// Draws the ants and any stationary objects using double buffering
        /// </summary>
        private void DrawAgentsDoubleBuffering()
        {

            // using FillRectangle to draw the agents
            // so declare variables to draw with
            float agentXPosition;
            float agentYPosition;

            // some arbitary size to draw the Ant
            float antSize;

            float foodSize;

            antSize = 5.0f;

            foodSize = 2.5f;

            Brush solidBrush;

            // get the graphics context of the background image
            using (Graphics backgroundGraphics = Graphics.FromImage(backgroundImage))
            {
                backgroundGraphics.Clear(Color.White);

                
                for (int i = 0; i < noOfAnts; i++)
                {
                    // get the 1st agent position
                    agentXPosition = (float)antGroup[i].AgentPosition.X;
                    agentYPosition = (float)antGroup[i].AgentPosition.Y;

                    // create a brush
                    solidBrush = new SolidBrush(Color.DarkRed);

                    // draw the agent number i
                    backgroundGraphics.FillRectangle(solidBrush, agentXPosition, agentYPosition, antSize, antSize);



                    //draw the food for the 1st agent if they have any
                    if (antGroup[i].HasFood)
                    {
                        solidBrush = new SolidBrush(Color.Gold);
                        backgroundGraphics.FillRectangle(solidBrush, agentXPosition, agentYPosition, foodSize, foodSize);

                    }
                }



                // now draw the stationary object
                // change colour of brush
                solidBrush = new SolidBrush(Color.Green);
                backgroundGraphics.FillRectangle(solidBrush, (float)someObject.X, (float)someObject.Y, 20, 20);

                //second stationary object
                solidBrush = new SolidBrush(Color.PeachPuff);
                backgroundGraphics.FillRectangle(solidBrush, (float)anotherObject.X, (float)anotherObject.Y, 20, 20);

            }

            // now draw the image on the panel
            using (Graphics g = drawingPanel.CreateGraphics())
            {
                g.DrawImage(backgroundImage, 0, 0, drawingPanel.Width, drawingPanel.Height);
            }

            // dispose of resources
            solidBrush.Dispose();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

                     


            antCarry(5);
            antDiscover(50);
            antCommunication(15);
            antMovementBehaviour();
      
            // after making a movement, now draw the agents
            DrawAgents();

            DrawAgentsDoubleBuffering();
        }

        /// <summary>
        /// handles the behaviour for if an ant has food and it can be dropped off, or if it can collect food
        /// </summary>
        /// <param name="distance">the distance at which the ant can pick up or deposit food</param>
        private void antCarry(int distance)
        {
            for (int i = 0; i < noOfAnts; i++)
            {
                //Conditional Statement for the ant picking up food at a FoodObject
                if (antGroup[i].DistanceTo(someObject) < distance)
                {
                    antGroup[i].GrabFood();
                }

                //conditional statement for the any depositing food at a Nest Object
                if (antGroup[i].DistanceTo(anotherObject) < distance)
                {
                    antGroup[i].DepositFood();
                }
            }
        }

        /// <summary>
        /// Handles the ants discovering the location of a FoodObject or NestObject
        /// </summary>
        /// <param name="distance">the distance at which ants discover food</param>
        private void antDiscover (int distance)
        {
            for (int i = 0; i < noOfAnts; i++)
            {
                //conditional statement for discovering the location of a nest
                if (antGroup[i].DistanceTo(anotherObject) < distance)
                {
                    antGroup[i].KnowsNest = true;

                    antGroup[i].NearestNest = anotherObject;
                }

                //conditional statement for discovering the location of a FoodObject
                if (antGroup[i].DistanceTo(someObject) < distance)
                {
                    antGroup[i].KnowsFood = true;

                    antGroup[i].NearestFood = someObject;
                }
            }
        }

        /// <summary>
        /// Handles the decision making process of the end depending on what it should do next
        /// </summary>
        private void antMovementBehaviour()
        {
            for (int i = 0; i < noOfAnts; i++)
            {
                //Conditional Block that determines where the ant moves to next
                if (antGroup[i].HasFood && antGroup[i].KnowsNest)
                {
                    antGroup[i].Approach(antGroup[i].NearestNest);
                }
                else if (antGroup[i].HasFood)
                {
                    antGroup[i].Wander();
                }
                else if (antGroup[i].KnowsFood)
                {
                    antGroup[i].Approach(antGroup[i].NearestFood);
                }
                else
                {
                    antGroup[i].Wander();
                }
            }
        }
        /// <summary>
        /// Queries all the ants within a certain distance of another ant to see if they know either the location of a FoodObject or a NestObject 
        /// </summary>
        /// <param name="distance">the distance the ants have to be at in order to communicate</param>
        private void antCommunication(int distance)
        {
            
            for (int i = 0; i < noOfAnts; i++)
            {
                
                for (int j = 0; j < noOfAnts; j++)
                {
                    if ((antGroup[i].DistanceTo(antGroup[j].AgentPosition) < distance) && antGroup[j].KnowsFood)
                    {
                        antGroup[i].NearestFood = antGroup[j].NearestFood;

                        antGroup[i].KnowsFood = true;
                    }
                }

                for (int j = 0; j < noOfAnts; j++)
                {
                    if ((antGroup[i].DistanceTo(antGroup[j].AgentPosition) < distance) && antGroup[j].KnowsNest)
                    {
                        antGroup[i].NearestNest = antGroup[j].NearestNest;

                        antGroup[i].KnowsNest = true;
                    }
                }
            }
        }

    }
}
