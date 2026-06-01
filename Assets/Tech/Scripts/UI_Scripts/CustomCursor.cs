using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursor;
    private void Start() => Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
}
