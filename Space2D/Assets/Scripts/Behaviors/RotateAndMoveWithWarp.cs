using UnityEngine;
using System.Collections;

public class RotateAndMoveWithWarp : MonoBehaviour {

	#region Public Properties
	public bool RandomizeSpeed = false;
	public bool RandomizeRotation = false;

	public float xSpeed;
	public float ySpeed;
	public float rotationSpeed;

	public float xSpeedMinimum;
	public float xSpeedMaximum;
	public float ySpeedMinimum;
	public float ySpeedMaximum;
	public float rotationSpeedMinimum;
	public float rotationSpeedMaximum;
	#endregion
	
	#region Private Properties
	private float xMinimum = -1920f;
	private float xMaximum = 1920f;
	private float yMinimum = -1080f;
	private float yMaximum = 1080f;
	#endregion

	// Use this for initialization
	void Start () {
		if (RandomizeSpeed) {
			xSpeed = Random.Range(xSpeedMinimum, xSpeedMaximum);
			ySpeed = Random.Range(ySpeedMinimum, ySpeedMaximum);
		}

		if (RandomizeRotation) {
			rotationSpeed = Random.Range(rotationSpeedMinimum, rotationSpeedMaximum);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Move the object
		float newX = this.transform.position.x;
		float newY = this.transform.position.y;
		
		newX += Time.deltaTime * xSpeed;
		newY += Time.deltaTime * ySpeed;
		
		// Make sure the newX and newY are in bounds -- if they aren't, move to the other side of the screen
		if (newX < xMinimum) newX = xMaximum;
		if (newX > xMaximum) newX = xMinimum;
		if (newY < yMinimum) newY = yMaximum;
		if (newY > yMaximum) newY = yMinimum;
		
		// We move the object by simply updating its position with the newX and newY
		this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		
		// Now for rotation
		float newRotation = this.transform.localEulerAngles.z;
		
		newRotation += (Time.deltaTime * rotationSpeed);
		
		this.transform.localEulerAngles = new Vector3(0,0,newRotation);
	}
}
