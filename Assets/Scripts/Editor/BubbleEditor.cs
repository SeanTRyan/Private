using Actor;
using Actor.Bubbles;
using UnityEditor;
using UnityEngine;
using Utility.Enums;
using Utility.Identifer;

/* BUBBLE EDITOR
 * Sean Ryan
 * April 5, 2018
 * 
 * BubbleEditor is a custom inspector that is for the ActorBubble component
 * It is responsible for creating GameObjects that make up the different HurtBubbles and HitBubbles on the Actor
 */

[CustomEditor(typeof(ActorBubble))]
public class BubbleEditor : Editor
{
    private ActorBubble actorBubble;                                //MonoBehaviour script that is applied to target

    private BubbleType bubbleType = BubbleType.None;                //BubbleType for identifying which type of BubbleBehaviour to create
    private bool foldout;                                           //Foldout can show or hide the Bubbles

    private void OnEnable()
    {
        actorBubble = (ActorBubble)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(5f);

        foldout = EditorGUILayout.Foldout(foldout, "Bubbles", true);

        if (!foldout)
            return;

        bubbleType = (BubbleType)EditorGUILayout.EnumPopup("Bubble Type", bubbleType);

        if (bubbleType != BubbleType.None)
        {
            if (GUILayout.Button("Add"))
            {
                actorBubble.bubbles.Add(new GameObject());
                AddBubble(bubbleType, actorBubble.bubbles[actorBubble.bubbles.Count - 1]);
            }

            if (GUILayout.Button("Remove"))
            {
                foreach (GameObject item in actorBubble.bubbles)
                    DestroyImmediate(item);
                actorBubble.bubbles.Clear();
            }
        }

        foreach (GameObject item in actorBubble.bubbles)
        {
            if (item == null)
            {
                actorBubble.bubbles.Remove(item);
                break;
            }

            if (item.GetComponent<BubbleBehaviour>() == null)
                break;

            string label = item.GetComponent<IBubble>().BodyArea.ToString() + " " + item.GetComponent<IBubble>().Type.ToString();

            GUILayout.Label(label, EditorStyles.boldLabel);
            GUILayout.Space(10f);

            item.GetComponent<IBubble>().BodyArea = (BodyArea)EditorGUILayout.EnumPopup("Body Area", item.GetComponent<IBubble>().BodyArea);
            item.GetComponent<IBubble>().Offset = EditorGUILayout.Vector3Field("Offset", item.GetComponent<IBubble>().Offset);
            item.GetComponent<IBubble>().Link = (Transform)EditorGUILayout.ObjectField("Link", item.GetComponent<IBubble>().Link, typeof(Transform), true);
            BubbleShape bubbleShape = (BubbleShape)EditorGUILayout.EnumPopup("Bubble Shape", item.GetComponent<IBubble>().Shape);

            if (item.GetComponent<IBubble>().Link != null)
            {
                item.transform.SetParent(item.GetComponent<IBubble>().Link);
                item.layer = item.GetComponent<IBubble>().Link.gameObject.layer;
                item.transform.position = item.GetComponent<IBubble>().Link.position + item.GetComponent<IBubble>().Offset;
            }

            item.transform.position = item.transform.position + item.GetComponent<IBubble>().Offset;

            item.name = label;

            if (item.GetComponent<Collider>() == null || item.GetComponent<IBubble>().Shape != bubbleShape)
            {
                item.GetComponent<IBubble>().Shape = bubbleShape;

                DestroyImmediate(item.GetComponent<Collider>());
                switch (item.GetComponent<IBubble>().Shape)
                {
                    case BubbleShape.Capsule:
                        item.AddComponent<CapsuleCollider>();
                        break;
                    case BubbleShape.Box:
                        item.AddComponent<BoxCollider>();
                        break;
                    case BubbleShape.Sphere:
                        item.AddComponent<SphereCollider>();
                        break;
                    default:
                        break;
                }
            }

            if (GUILayout.Button("Delete"))
            {
                DestroyImmediate(item);
                actorBubble.bubbles.Remove(item);
                break;
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10f);
        }
    }

    //A method that adds either a HitBubble or a HurtBubble component to the GameObject
    private void AddBubble(BubbleType bubbleType, GameObject gameObject)
    {
        switch (bubbleType)
        {
            case BubbleType.HitBubble:
                gameObject.AddComponent<HitBubble>().Type = BubbleType.HitBubble;
                gameObject.tag = Tag.HitBubble;
                break;
            case BubbleType.HurtBubble:
                gameObject.AddComponent<HurtBubble>().Type = BubbleType.HurtBubble;
                gameObject.tag = Tag.HurtBubble;
                break;
            default:
                break;
        }
    }
}
