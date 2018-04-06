using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* ACTOR MOVEMENT
 * Sean Ryan
 * April 5, 2018
 * 
 * Derives from the Movement class and allows the controller to control the Actor
 */ 

namespace Actor
{
    public class ActorMovement : Movement
    {

        //Overrides the Move method of the base class and moves the rigid body
        public override void Move(float direction)
        {
            Forward = new UnityEngine.Vector2(direction, 0f) * UnityEngine.Time.deltaTime;
            rigidbody.AddForce(Forward * speed, forceMode);
        }
    }
}
