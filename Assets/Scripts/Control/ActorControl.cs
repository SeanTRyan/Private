using Controller.Mechanism;
using UnityEngine;
using Utility.Enums;

namespace Actor
{
    /// <summary>
    /// A controller class for the Actor. It passes <para/>
    /// input values from buttons and joysticks to the different<para/>
    /// classes associated with the Actor.
    /// </summary>
    public class ActorControl : MonoBehaviour, IControl
    {
        [SerializeField] private PlayerNumber playerNumber;             //An enum that is used to set what the number of the player is
        [SerializeField] private Lever lever = new Lever();             //Lever (or joystick) is responsible for the horizontal and vertical input
        [SerializeField] private Button[] buttons;                      //An array of buttons that register the different input buttons on the controller

        private Movement movement;                                      //An interface that is able to retrieve different movement components that inherit from IMovement
        private ActorJump jump;                                         //An interface that is able to retreive different jump components that inherit from IJump
        private ActorBlock block;                                       //Block
        private Attack attack;

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new Button();

            movement = GetComponent<Movement>();
            jump = GetComponent<ActorJump>();
            block = GetComponent<ActorBlock>();
            attack = GetComponent<Attack>();
        }

        private void Update()
        {
            lever.UpdateLever(playerNumber);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].UpdateButton(playerNumber, i + 1);
        }

        private void FixedUpdate()
        {
            UpdateMovement();

            if(!movement.IsRotating)
                UpdateJump();

            UpdateBlock();
        }

        private void UpdateMovement()
        {
            movement.Halt = attack.IsAttacking;
            movement.IsDashing = (lever.HorizontalTimer < 0.1f && lever.AbsoluteHorizontal > 0.75f);
            movement.Move(lever.Horizontal);
        }

        private void UpdateJump()
        {
            jump.Jump(GetButton(ButtonType.Action1).Hold, GetButton(ButtonType.Action1).HoldTimer);
        }

        private void UpdateBlock()
        {
            block.Block(GetButton(ButtonType.Action2).Hold);
        }

        public PlayerNumber PlayerNumber { get { return playerNumber; } }
        public Button GetButton(ButtonType buttonType) { return buttons[(int)buttonType]; }
        public Lever Lever { get { return lever; } }
    }
}
