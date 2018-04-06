using System.Collections.Generic;
using UnityEngine;

/* ACTOR BUBBLE
 * Sean Ryan
 * April 5, 2018
 * 
 * Container class for the different bubbles that can be on the Actor (HitBubble, HurtBubble, etc).
 */ 

namespace Actor
{
    public class ActorBubble : MonoBehaviour
    {
        public List<GameObject> bubbles;                    //List of GameObjects that add different bubbles
    }
}
