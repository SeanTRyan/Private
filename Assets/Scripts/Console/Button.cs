using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility.Enums;

/* BUTTON
 * Sean Ryan
 * March 29, 2018
 * 
 * This class is responsible for all button related tasks. These tasks include, checking if the user has pressed a button,
 * released a button, or held a button. It also counts how many times a button was pressed in a 1 second. Finally, it
 * is capable of returning how long a button is held for.
 * 
 */

namespace Controller.Mechanism
{
    [Serializable]
    public sealed class Button
    {
        [SerializeField] private float pressThreshold = 1.0f;           //The max amount of time that a button will count presses before it is reset to 0

        //Different input properties
        public bool Click { get; private set; }                         //A bool for determining whether the button is clicked
        public bool Release { get; private set; }                       //A bool that determines whether the button is released
        public bool Hold { get; private set; }                          //A bool that determines whether the button is being held

        public float HoldTimer { get; private set; }                    //Measures how long a button is held for
        public int Press { get; private set; }                          //An int that stores how many times the same button was pressed

        private float pressTimer = 0f;                                  //A variable that is added to time when a button is first clicked

        //Updates all of the buttons to their correct values every frame. The playerNumber variable
        //passes the controller number of the player and the keyNum is which button is being updated.
        public void UpdateButton(PlayerNumber playerNumber, int keyNum)
        {
            if (keyNum <= 0)
                return;

            Click = Input.GetKeyDown("joystick " + (int)playerNumber + " button " + keyNum);
            Release = Input.GetKeyUp("joystick " + (int)playerNumber + " button " + keyNum);
            Hold = Input.GetKey("joystick " + (int)playerNumber + " button " + keyNum);

            HoldTimer = (Hold) ? TimerUpdate(HoldTimer) : 0f;
            Press = (Click) ? Press + 1 : (pressTimer > pressThreshold) ? 0 : Press;

            TimesPressed();
        }

        //A method that adds a float to the Time.deltaTime
        private float TimerUpdate(float time)
        {
            return time += Time.deltaTime;
        }

        //Updates the press timer while the pressTimer is below the pressThreshold
        private void TimesPressed()
        {
            if (pressTimer < pressThreshold && Press > 0)
            {
                pressTimer = TimerUpdate(pressTimer);
                return;
            }
            pressTimer = 0f;
        }
    }
}
