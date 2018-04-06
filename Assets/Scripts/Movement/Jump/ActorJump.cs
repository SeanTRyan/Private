using System.Collections;
using UnityEngine;

/* ACTOR JUMP
 * Sean Ryan
 * April 5, 2018
 * 
 * ActorJump is a component that is responsible for jump motions of the actor
 */ 

namespace Actor
{
    public class ActorJump : MonoBehaviour
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDelay;

        public float JumpDelay { get { return jumpDelay; } }

        private float jumpCounter = 0f;
        private bool onGround = true;

        private new Rigidbody rigidbody;
        private JumpBehaviour jumpBehaviour;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            jumpBehaviour = animator.GetBehaviour<JumpBehaviour>();
            jumpBehaviour.jump = this;

            rigidbody = GetComponent<Rigidbody>();
        }

        public void Jump(bool click, float gradient)
        {
            jumpDelay = jumpBehaviour.JumpDelay - 0.1f;

            if (click && gradient < jumpDelay)
            {
                jumpCounter += (0.2f * jumpHeight);

                StopAllCoroutines();
                StartCoroutine(Initiate(jumpDelay));
            }

            Animations(click);
        }

        private IEnumerator Initiate(float delay)
        {
            yield return new WaitForSeconds(jumpDelay);
            rigidbody.velocity = Vector2.up * jumpCounter;
            jumpCounter = 0f;
        }

        private void Animations(bool value)
        {
            animator.SetBool("HasJumped", value);
            animator.SetBool("OnGround", onGround);
        }
    }
}
