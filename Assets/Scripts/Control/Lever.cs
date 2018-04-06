using System;
using UnityEngine;
using Utility.Enums;

/* LEVER
 * Sean Ryan
 * April 5, 2018
 * 
 * The Lever class is responsible for retrieving joystick input from the controller and letting the player control
 * the character in the way that he/she wants
 */

namespace Controller.Mechanism
{
    [Serializable]
    public sealed class Lever
    {
        [SerializeField] private float directionDistance = 0.75f;       //The distance that the joystick needs to go before timer is stopped

        //Horizontal input properties
        public float Horizontal { get; set; }                           //The base horizontal input of the joystick
        public float AbsoluteHorizontal { get; private set; }           //The absolute value of the horizontal input (Never negative)
        public float RawHorizontal { get; private set; }                //The raw value of horizontal input. Value goes directly from 0 to 1/-1

        //Vertical input properties
        public float Vertical { get; set; }                             //The base vertical input of the joystick
        public float AbsoluteVertical { get; private set; }             //The absolute value of the vertical input (Never negative)
        public float RawVertical { get; private set; }                  //The raw value of vertical input. Value goes directly from 0 to 1/-1

        //Horizontal and Vertical property
        public Vector2 Circle { get; private set; }                     //The Vector2 property of the Horizontal and Vertical input properties

        //Timers that measure how fast the stick is moved
        public float HorizontalTimer { get; private set; }              //Returns the time it takes for the lever to go from 0 to past 0.75(Abs) in the horizontal direction
        public float VerticalTimer { get; private set; }                //Returns the time it takes for the lever to go from 0 to past 0.75(Abs) in the vertical direction

        //Updates all of the different values of the lever. The parameter playerNumber is for mapping the controller
        //to the correct player (Player One, Player Two, Player Three, or Player Four)
        public void UpdateLever(PlayerNumber playerNumber)
        {
            Horizontal = Input.GetAxis("joystick " + (int)playerNumber + " axis " + 0);
            AbsoluteHorizontal = Math.Abs(Horizontal);
            RawHorizontal = Input.GetAxisRaw("joystick " + (int)playerNumber + " axis " + 0);

            Vertical = Input.GetAxis("joystick " + (int)playerNumber + " axis " + 1);
            AbsoluteVertical = Math.Abs(Vertical);
            RawVertical = Input.GetAxisRaw("joystick " + (int)playerNumber + " axis " + 1);

            Circle = new Vector2(Horizontal, Vertical);

            MeasureInput(AbsoluteHorizontal, HorizontalTimer);
            MeasureInput(AbsoluteVertical, VerticalTimer);
        }

        //Measures how fast the joystick is moved in a direction
        private void MeasureInput(float direction, float timer)
        {
            if (direction == 0f)
            {
                timer = 0f;
                return;
            }

            timer = (direction > 0f && direction < directionDistance) ? timer + Time.deltaTime : timer;
        }
    }
}
