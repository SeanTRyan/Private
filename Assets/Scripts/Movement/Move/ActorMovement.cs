using Actor.Animation;
using UnityEngine;

namespace Actor
{
    /// <summary>
    /// An abstract class that contains common variables and functionality <para/>
    /// that is associated with movement.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ActorMovement : MonoBehaviour
    {
        [SerializeField] private ForceMode forceMode = ForceMode.VelocityChange;                 //Sets which ForceMode to apply to the Rigidbody of the Actor
        [SerializeField] private float acceleration = 150f;                  //The acceleration of the Actor
        [SerializeField] private float deceleration = 3f;                  //The drag of the Rigidbody of the Actor
        [SerializeField] private float rotationSpeed = 10f;                 //The speed that the actor rotates at

        [SerializeField] private float dashAcceleration = 250f;
        [SerializeField] private float dashRotationSpeed = 20f;

        private bool isTurning = false;

        private new Rigidbody rigidbody;                              //Rigidbody for getting the component of the Actor
        private new Transform transform;                              //Transform of the GameObject attached to this component
        [SerializeField] private new MovementAnimation animation;     //The movement animations

        public Vector2 Forward { get; protected set; }                  //A Vector2 that is the forward movement of the Actor
        public float Rotation { get; protected set; }                   //The rotation of the Actor
        public bool IsRotating { get; protected set; }                  //Returns true while the Actor is rotating/turning
        public bool Stop { get; set; }                                  //Halts the Actor's movement completely
        public bool IsDashing { get; set; }                             //Property for checking if the Actor is dashing

        private float dashTimer = 0f;
        private float dashLength = 0.1f;

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            transform = GetComponent<Transform>();
            rigidbody.drag = deceleration;

            animation.Init(this);

            IsRotating = false;
            Rotation = 0f;
        }

        //An abstract method for moving the Actor
        public virtual void Move(float direction)
        {
            if (Stop) return;

            if(!IsRotating)
            {
                Forward = new Vector2(direction, 0f);

                dashTimer = (IsDashing) ? dashAcceleration : acceleration;
                rigidbody.drag = (dashTimer < dashLength && IsDashing) ? 0f : deceleration;

                float speed = (IsDashing) ? dashAcceleration : acceleration;

                Vector2 forward = (Forward * speed) * Time.deltaTime;
                rigidbody.AddForce(forward, forceMode);
            }

            Rotate(direction);
            Animation(IsDashing, direction);
        }
        

        //A virtual method for rotating the Actor
        protected virtual void Rotate(float direction)
        {
            Rotation = (direction > 0.01f) ? 0f : (direction < -0.01f) ? -180f : Rotation;
            Quaternion endRotation = Quaternion.Euler(0f, Rotation, 0f);

            float speed = (IsDashing) ? dashRotationSpeed : rotationSpeed;

            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, endRotation, speed);

            IsRotating = (transform.localRotation.eulerAngles.y != -Rotation);
        }

        protected virtual void Animation(bool isDashing, float direction)
        {
            float animSpeed = Mathf.Abs(direction);
            bool isTurning = (direction * transform.forward.x < 0f);

            animation.SetDash(isDashing);
            animation.SetSpeed(animSpeed);
            animation.SetTurn(isTurning);
        }
    }
}
