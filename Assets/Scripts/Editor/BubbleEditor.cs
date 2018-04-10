using Actor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Utility.Enums;

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

    private ReorderableList list;

    [SerializeField] private BubbleType bubbleType;                //BubbleType for identifying which type of BubbleBehaviour to create
    private bool foldout;                                           //Foldout can show or hide the Bubbles

    private void OnEnable()
    {
        actorBubble = (ActorBubble)target;

        list = new ReorderableList(actorBubble.bubbles, typeof(GameObject), true, true, true, true);

        list.onAddCallback += AddItem;
        list.onRemoveCallback += RemoveItem;

        list.drawHeaderCallback += DrawHeader;
        list.drawElementCallback += DrawElement;
    }

    private void OnDisable()
    {
        list.onAddCallback -= AddItem;
        list.onRemoveCallback -= RemoveItem;

        list.drawHeaderCallback -= DrawHeader;
        list.drawElementCallback -= DrawElement;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(5f);

        int index = 0;
        foreach (GameObject item in actorBubble.bubbles)
        {
            index++;
            if (item == null)
            {
                actorBubble.bubbles.Remove(item);
                actorBubble.bubbleType.RemoveAt(index);
                break;
            }
        }

        foldout = EditorGUILayout.Foldout(foldout, "Bubbles", true);
        if (foldout)
            list.DoLayoutList();
    }

    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Bubbles");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        GameObject gameObject = actorBubble.bubbles[index];

        rect.y += 2f;
        float singleLine = EditorGUIUtility.singleLineHeight;
        actorBubble.bubbleType[index] = (BubbleType)EditorGUI.EnumPopup(new Rect(rect.x, rect.y, rect.width, singleLine), "Bubble Type", actorBubble.bubbleType[index]);

        if (actorBubble.bubbleType[index] == BubbleType.None)
        {
            if (actorBubble.bubbles[index].GetComponent<Bubble>() != null)
                DestroyImmediate(actorBubble.bubbles[index].GetComponent<Bubble>());
            return;
        }

        AddBubble(gameObject, actorBubble.bubbleType[index]);

        Bubble bubble = gameObject.GetComponent<Bubble>();
        gameObject.name = bubble.Type.ToString() + " " + bubble.BodyArea.ToString();

        list.elementHeight = 160f;

        bubble.BodyArea = (BodyArea)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + 20, rect.width, singleLine), "Body Area", bubble.BodyArea);
        bubble.Offset = EditorGUI.Vector3Field(new Rect(rect.x, rect.y + 40f, rect.width, singleLine), "Offset", bubble.Offset);
        bubble.Link = (Transform)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 80f, rect.width, singleLine), "Link", bubble.Link, typeof(Transform), true);
        bubble.Shape = (Shape)EditorGUI.EnumPopup(new Rect(rect.x, rect.y + 100f, rect.width, singleLine), "Shape", bubble.Shape);

        if (bubble.Link != null)
            gameObject.transform.position = bubble.Link.position + bubble.Offset;
        else
            gameObject.transform.position += bubble.Offset;
        gameObject.transform.SetParent(gameObject.GetComponent<Bubble>().Link);

        AddCollider(gameObject);

        if (gameObject.GetComponent<Collider>() != null)
            SetSize(rect, gameObject);
    }

    private void AddItem(ReorderableList list)
    {
        GameObject gameObject = new GameObject();
        actorBubble.bubbleType.Add(new BubbleType());

        actorBubble.bubbles.Add(gameObject);

        EditorUtility.SetDirty(target);
    }

    private void AddBubble(GameObject gameObject, BubbleType bubbleType)
    {
        if (gameObject.GetComponent<Bubble>() != null && bubbleType != gameObject.GetComponent<Bubble>().Type)
            DestroyImmediate(gameObject.GetComponent<Bubble>());

        if (gameObject.GetComponent<Bubble>() != null && bubbleType == gameObject.GetComponent<Bubble>().Type)
            return;

        switch (bubbleType)
        {
            case BubbleType.HitBubble:
                gameObject.AddComponent<HitBubble>();
                break;
            case BubbleType.HurtBubble:
                gameObject.AddComponent<HurtBubble>();
                break;
            default:
                break;
        }

        gameObject.GetComponent<Bubble>().Type = bubbleType;
    }

    private void RemoveItem(ReorderableList list)
    {
        DestroyImmediate(actorBubble.bubbles[list.index]);
        actorBubble.bubbles.RemoveAt(list.index);
        actorBubble.bubbleType.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
    }

    private void AddCollider(GameObject gameObject)
    {
        Bubble bubble = gameObject.GetComponent<Bubble>();

        DestroyImmediate(gameObject.GetComponent<Collider>());
        switch (bubble.Shape)
        {
            case Shape.Capsule:
                gameObject.AddComponent<CapsuleCollider>();
                break;
            case Shape.Sphere:
                gameObject.AddComponent<SphereCollider>();
                break;
            case Shape.Box:
                gameObject.AddComponent<BoxCollider>();
                break;
            default:
                break;
        }

        if (gameObject.GetComponent<Collider>() != null)
            gameObject.GetComponent<Collider>().isTrigger = true;
    }

    private void SetSize(Rect rect, GameObject gameObject)
    {
        Bubble bubble = gameObject.GetComponent<Bubble>();

        float singleLine = EditorGUIUtility.singleLineHeight;

        switch (bubble.Shape)
        {
            case Shape.Capsule:
                CapsuleCollider capsule = gameObject.GetComponent<CapsuleCollider>();
                bubble.Radius = EditorGUI.FloatField(new Rect(rect.x, rect.y + 120, rect.width, singleLine), "Radius", bubble.Radius);
                gameObject.GetComponent<CapsuleCollider>().radius = bubble.Radius;
                break;
            case Shape.Sphere:
                bubble.Radius = EditorGUI.FloatField(new Rect(rect.x, rect.y + 120, rect.width, singleLine), "Radius", bubble.Radius);
                gameObject.GetComponent<SphereCollider>().radius = bubble.Radius;
                break;
            case Shape.Box:
                bubble.Size = EditorGUI.Vector3Field(new Rect(rect.x, rect.y + 120, rect.width, singleLine), "Size", bubble.Size);
                gameObject.GetComponent<BoxCollider>().size = bubble.Size;
                break;
            default:
                break;
        }
    }
}
