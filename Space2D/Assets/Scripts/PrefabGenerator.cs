using UnityEngine;
using System.Collections;

public class PrefabGenerator : MonoBehaviour {

	#region Public Properties
	public Transform PrefabToSpawn;
	public float TimeToSpawn;
	#endregion

	#region Private Properties
	private float timeToSpawnTimer;
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Count down the timer and spawn the prefab when we reach 0
		timeToSpawnTimer -= Time.deltaTime;
		if (timeToSpawnTimer < 0f) {
			Instantiate(PrefabToSpawn, this.transform.position, Quaternion.identity);
			timeToSpawnTimer += TimeToSpawn;
		}
	}
}
