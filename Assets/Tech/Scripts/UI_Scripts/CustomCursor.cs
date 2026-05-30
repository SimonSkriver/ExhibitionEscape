using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursor;

    private void Start() {
        DontDestroyOnLoad(this);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
