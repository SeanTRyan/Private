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
    public class ActorJump : MonoBehaviour, IJump
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDelay;

        public float JumpDelay { get { return jumpDelay; } }

        private float jumpCounter = 0f;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Jump(bool click, float gradient)
        {
            if (click && gradient < jumpDelay)
            {
                jumpCounter += (0.2f * jumpHeight);

                StopAllCoroutines();
                StartCoroutine(Initiate(jumpDelay));
            }
        }

        private IEnumerator Initiate(float delay)
        {
            yield return new WaitForSeconds(jumpDelay);
            rigidbody.velocity = Vector2.up * jumpCounter;
            jumpCounter = 0f;
        }
    }
}
