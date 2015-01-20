using UnityEngine;
using System.Collections;

public class GameManager_Done : MonoBehaviour {

	#region Public Properties
	public AudioSource Music;
	#endregion

	#region Private Properties

	#endregion

	// Use this for initialization
	void Start () {
		AudioSource audioSource = this.transform.GetComponent<AudioSource>();
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoseLife() {

	}
}
