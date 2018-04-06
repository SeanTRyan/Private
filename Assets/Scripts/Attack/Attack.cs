using Controller.Mechanism;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [RequireComponent(typeof(Animator))]
    public class Attack : MonoBehaviour
    {
        public float damage;

        private AttackBehaviour attackBehaviour;
        private Animator animator;
        private IControl control;

        // Use this for initialization
        void Awake()
        {
            animator = GetComponent<Animator>();
            control = GetComponent<IControl>();

            attackBehaviour = animator.GetBehaviour<AttackBehaviour>();
            attackBehaviour.actorAttack = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (control.GetButton(Utility.Enums.ButtonType.Action2).Click)
                animator.SetInteger("Attack", 1);
            else if (control.GetButton(Utility.Enums.ButtonType.Action3).Click)
                animator.SetInteger("Attack", 2);
            else
                animator.SetInteger("Attack", 0);
        }

        public bool IsAttacking { get; set; }
    }
}