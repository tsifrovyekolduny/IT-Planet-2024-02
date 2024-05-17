using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Texture2D InteractionCursor;

    private void OnMouseEnter()
    {                
        Cursor.SetCursor(InteractionCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }    
}

