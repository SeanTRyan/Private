using Actor.Animation;
using Controller.Mechanism;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [RequireComponent(typeof(Animator))]
    public class ActorAttack : MonoBehaviour
    {
        public float damage;

        [SerializeField] private new AttackAnimation animation;

        private int attackNumber = 0;
        private float timer = 0f;


        // Use this for initialization
        void Awake()
        {
            animation.Init(this);
        }

        public void Attack(bool click)
        {
            SetAttackNumber(click);

            Animation(click);
        }

        private void SetAttackNumber(bool click)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f)
                attackNumber = 0;

            if (click && attackNumber < 4)
            {
                attackNumber++;
                timer = 0f;
            }
        }

        private void Animation(bool click)
        {
            animation.SetAttack(attackNumber);
        }

        public bool IsAttacking { get; set; }
    }
}