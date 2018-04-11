using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Actor
{
    /// <summary>
    /// ActorBlock is the blocking aspects of the Actor
    /// </summary>
    public class ActorBlock : MonoBehaviour
    {
        [SerializeField] private float shieldStrength;
        [SerializeField] private float shieldDuration;

        private float currentShield;
        private float stunDuration = 3.0f;

        public bool IsStunned { get; private set; }
        public bool IsBlocking { get; private set; }

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            currentShield = shieldStrength;
        }

        public void Block(bool block)
        {
            IsBlocking = block;

            if (block && currentShield > 0f && !IsStunned)
                currentShield -= shieldDuration;
            else if (currentShield != shieldStrength && currentShield > 0 && !IsStunned)
                currentShield += shieldDuration;
            else if (currentShield <= 0f && !IsStunned)
            {
                StopAllCoroutines();
                StartCoroutine(Stun());
            }

            print(currentShield);

            if(!IsStunned)
                animator.SetBool("IsBlocking", block);
            if(IsStunned)
                animator.SetBool("IsBlocking", false);

            animator.SetBool("IsDisabled", IsStunned);
        }

        public void ResetBlock()
        {
            currentShield += shieldDuration;
        }

        private IEnumerator Stun()
        {
            IsStunned = true;
            yield return new WaitForSeconds(stunDuration);
            currentShield = shieldStrength;
            IsStunned = false;
        }
    }
}