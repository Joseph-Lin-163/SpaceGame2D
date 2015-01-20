using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	#region Public Properties
	public Transform playerShipPrefab;
	#endregion

	#region Private Properties
	private bool RestartCondition;
	private GameManager gameManager;
	#endregion

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) 
			gameManager = go.GetComponent<GameManager>();
		RestartCondition = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (RestartCondition == true) {
			RestartCondition = false;
			StartCoroutine(_Destroy());
		}
	}

	public void SetRestart(bool tf) {
		RestartCondition = tf;
	}

	IEnumerator _Destroy() {
		GameObject[] destroyThese = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];

		for (int i = 0; i < destroyThese.Length; i++) {
			if (destroyThese[i].GetComponent<Asteroid>() != null || destroyThese[i].GetComponent<UFO>() != null 
			    || destroyThese[i].GetComponent<UFOBullet>() != null)
				Destroy(destroyThese[i]);
		}

		gameManager.Score = 0;
		gameManager.Lives = 3;
		Instantiate(playerShipPrefab);

		yield return 0;
	}
}
