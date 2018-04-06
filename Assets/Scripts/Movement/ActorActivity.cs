using UnityEngine;
using Actor.Activities;
using Controller.Mechanism;

/* MOTION
 * Sean Ryan
 * March 17, 2018
 * 
 * This class is responsible for the movement behaviour of the character.
 * Movement includes physical movement, as well as animations.
 */

namespace Actor
{
    #region Required Components
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IControl))]
    #endregion
    public class ActorActivity : MonoBehaviour
    {
        [SerializeField] private ForceMode movementForce;
        [SerializeField] private float aerialSpeed;

        [SerializeField] private MoveActivity movement = new MoveActivity();
        [SerializeField] private CrouchActivity crouch = new CrouchActivity();
        [SerializeField] private DashActivity dash = new DashActivity();

        private new Rigidbody rigidbody;
        private IControl control;
        private Animator animator;

        private float rotation = 0f;

        private bool onGround = true;
        private bool isRotating = false;

        //Awake is called when the script instance is being loaded.
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            control = GetComponent<IControl>();

            movement.Initialize(GetComponent<Component>(), control);
            crouch.Initialize(GetComponent<Component>(), control);
            dash.Initialize(GetComponent<Component>(), control);
        }

        // Update is called once per frame.
        private void Update()
        {
            movement.OnUpdate();
            crouch.OnUpdate();
            dash.OnUpdate();

            Animation();
        }

        //FixedUpdate is called every fixed framerate frame.
        private void FixedUpdate()
        {
            MotionUpdate();
        }

        private void MotionUpdate()
        {
            if (dash.IsDashing)
            {
                SetMotion(dash);
                SetRotation(dash);
            }
            else if (crouch.IsCrouching)
            {
                SetMotion(crouch);
                SetRotation(crouch);
            }
            else
            {
                SetMotion(movement);
                SetRotation(movement);
            }
        }

        private void Animation()
        {
            animator.SetFloat("Speed", control.Lever.AbsoluteHorizontal);
            animator.SetBool("IsDashing", dash.IsDashing);
        }

        private void SetMotion(Activity activity)
        {
            float speed = ((onGround) ? activity.Speed : aerialSpeed) * control.Lever.Horizontal;
            Vector2 move = new Vector2(speed, 0f) * Time.deltaTime;
            rigidbody.AddForce(move, movementForce);
        }

        private void SetRotation(Activity activity)
        {
            if (!onGround && !isRotating)
                return;

            float rotationSpeed = activity.RotationSpeed;
            Quaternion endRotation = Rotate(ref rotation);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, endRotation, rotationSpeed);

            isRotating = (transform.localRotation.eulerAngles.y != rotation);
        }

        private Quaternion Rotate(ref float rotation)
        {
            rotation = (control.Lever.Horizontal > 0.1f) ? 0f : (control.Lever.Horizontal < -0.1f) ? -180f : rotation;
            Quaternion quaternionRotation = Quaternion.Euler(0f, rotation, 0f);
            return quaternionRotation;
        }
    }
}
