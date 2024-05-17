using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Texture2D InteractionCursor;

    public void OnMouseEnter()
    {
        if (UiScript.Instance.Hidden || LayerMask.LayerToName(gameObject.layer) == "UI")
        {
            Cursor.SetCursor(InteractionCursor, Vector2.zero, CursorMode.Auto);
        }        
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }    
}

