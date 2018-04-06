using System;
using UnityEngine;

/* ACTORANIMATION
 * Sean Ryan
 * April 5, 2018
 * 
 * This class is responsible for all of the animations that are associated with 
 * an Actor
 */

namespace Actor
{
    [Serializable]
    public class ActorAnimation : MonoBehaviour
    {
        [SerializeField] private string speedName;                      //The name of the base animation parameter
        [SerializeField] private string dashName;                       //The name of the dash animation parameter
        
        [SerializeField] private float speedSmoothing;                  //A variable that smoothes the speed in the animator

        private Animator animator;                                      //Animator component for running animations

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void MoveAnimation(float value) { animator.SetFloat(speedName, value, speedSmoothing, Time.deltaTime); }
        public void DashAnimation(bool value) { animator.SetBool(dashName, value); }
    }
}
