using UnityEngine;
using Utility.Enums;
using Utility.Identifer;

/* HURT BUBBLE
 * Sean Ryan
 * April 5, 2018
 * 
 * HurtBubble is component that is attached to the different areas that can be hurt on the Actor
 * It derives from BubbleBehaviour and is responsible for identifying if the player gets hurt
 */ 

namespace Actor
{
    public class HurtBubble : Bubble
    {
        public delegate void OnHurt(bool hurt, BodyArea area);
        public event OnHurt HurtChange;

        private bool isHurt = false;

        private void Awake()
        {
            Type = BubbleType.HurtBubble;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.HitBubble) && other.gameObject.layer != gameObject.layer)
                HurtChange(true, bodyArea);
        }

        protected override void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tag.HitBubble) && other.gameObject.layer == gameObject.layer)
                Hurt_Event(false, BodyArea.None);
        }

        private void Hurt_Event(bool hurt, BodyArea area)
        {
            if (isHurt == hurt)
                return;

            isHurt = hurt;

            if (HurtChange != null)
                HurtChange(isHurt, area);
        }
    }
}
