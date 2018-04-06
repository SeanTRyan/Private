using Actor.Bubbles;
using System;
using System.Text;
using UnityEngine;
using Utility.Enums;

/* BUBBLE BEHAVIOUR
 * Sean Ryan
 * April 5, 2018
 * 
 * BubbleBehaviour is an abstract class that contains common elements that are shared among its 
 * derived classes.
 */

namespace Actor
{
    [Serializable]
    public abstract class BubbleBehaviour : MonoBehaviour, IBubble
    {
        [SerializeField] protected StringBuilder bubbleName;
        [SerializeField] protected BodyArea bodyArea;
        [SerializeField] protected Vector3 offset;
        [SerializeField] protected Transform link;
        [SerializeField] protected BubbleShape shape;
        [SerializeField] protected BubbleType type;

        [SerializeField] public StringBuilder Name { get; set; }
        [SerializeField] public BodyArea BodyArea { get { return bodyArea; } set { bodyArea = value; } }
        [SerializeField] public Vector3 Offset { get { return offset; } set { offset = value; } }
        [SerializeField] public Transform Link { get { return link; } set { link = value; } }
        [SerializeField] public BubbleShape Shape { get { return shape; } set { shape = value; } }
        [SerializeField] public BubbleType Type { get { return type; } set { type = value; } }

        protected virtual void Awake() { GetComponent<Collider>().isTrigger = true; }
        protected abstract void OnTriggerEnter(Collider other);
        protected abstract void OnTriggerExit(Collider other);
    }
}
