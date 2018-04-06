using Controller.Mechanism;
using UnityEngine;
using Utility.Enums;

/* ACTORCONTROL
 * Sean Ryan
 * April 5, 2018
 * 
 * A controller class that controls the different actions of an Actor
 */

namespace Actor
{
    public class ActorControl : MonoBehaviour, IControl
    {
        [SerializeField] private PlayerNumber playerNumber;             //An enum that is used to set what the number of the player is
        [SerializeField] private Lever lever = new Lever();             //Lever (or joystick) is responsible for the horizontal and vertical input
        [SerializeField] private Button[] buttons;                      //An array of buttons that register the different input buttons on the controller

        private IMovement movement;                                     //An interface that is able to retrieve different movement components that inherit from IMovement
        private IJump jump;                                             //An interface that is able to retreive different jump components that inherit from IJump
        private ActorBlock block;                                       //Block

        private float jumpIncrement = 0f;

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new Button();

            movement = GetComponent<IMovement>();
            jump = GetComponent<IJump>();
            block = GetComponent<ActorBlock>();
        }

        private void Update()
        {
            lever.UpdateLever(playerNumber);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].UpdateButton(playerNumber, i + 1);
        }

        private void FixedUpdate()
        {
            movement.Move(lever.Horizontal);
            jump.Jump(GetButton(ButtonType.Action1).Hold, GetButton(ButtonType.Action1).HoldTimer);
            block.Block(GetButton(ButtonType.Action2).Hold);
        }

        public PlayerNumber PlayerNumber { get { return playerNumber; } }
        public Button GetButton(ButtonType buttonType) { return buttons[(int)buttonType]; }
        public Lever Lever { get { return lever; } }
    }
}
