using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

	#region Public Properties
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		hotSpot = Input.mousePosition;
	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 30f, 33f), cursorTexture);
	}

	void OnMouseEnter() {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}

}
