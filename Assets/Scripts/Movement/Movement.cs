using UnityEngine;

/* MOVEMENT
 * Sean Ryan
 * April 5, 2018
 * 
 * An abstract class that holds common variables associated with movement.
 * It also inherits from the IMovement interface
 */ 

namespace Actor
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Movement : MonoBehaviour, IMovement
    {
        [SerializeField] protected ForceMode forceMode;                 //Sets which ForceMode to apply to the Rigidbody of the Actor
        [SerializeField] protected float speed;                         //The speed of the Actor
        [SerializeField] protected float drag;                          //The drag of the Rigidbody of the actor

        protected new Rigidbody rigidbody;                              //Rigidbody for getting the component of the Actor

        public Vector2 Forward { get; protected set; }                  //A Vector2 that is the forward movement of the Actor

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        //An abstract method that is implemented by derived classes
        public abstract void Move(float direction);
    }
}
