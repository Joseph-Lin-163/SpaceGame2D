using UnityEngine;
using System.Collections;

public class MenuPlay : MonoBehaviour {

	#region Public Properties
	public Transform scoreBoard;
	public Transform GameManager;
	public GameManager GMScript;
	#endregion

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) 
			GMScript = go.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void OnMouseDown() {
		GMScript.transition = true;
		DontDestroyOnLoad(GameManager);
		DontDestroyOnLoad(scoreBoard);
		Application.LoadLevel("Week6");
	}
}
