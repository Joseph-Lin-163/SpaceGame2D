using UnityEngine;
using System.Collections;

public class UFO_AI : MonoBehaviour {

	#region Public Properties
	public float xSpeed;
	public float ySpeed;
	public float rotationSpeed;
	
	public Direction xDirection = Direction.Left;
	public Direction yDirection = Direction.Right;
	#endregion
	
	#region Private Properties
	private float xMinimum = -1920f;
	private float xMaximum = 1920f;
	private float yMinimum = -1080f;
	private float yMaximum = 1080f;
	#endregion
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Move the object
		float newX = this.transform.position.x;
		float newY = this.transform.position.y;
		
		if (xDirection == Direction.Right) {
			newX += Time.deltaTime * xSpeed;
		} else {
			newX -= Time.deltaTime * xSpeed;
		}
		
		if (yDirection == Direction.Up) {
			newY += Time.deltaTime * ySpeed;
		} else {
			newY -= Time.deltaTime * ySpeed;
		}
		
		// Make sure the newX and newY are in bounds -- if they aren't, change direction
		if (newX < xMinimum) {
			newX = xMinimum;
			xDirection = Direction.Right;
		}
		
		if (newX > xMaximum) {
			newX = xMaximum;
			xDirection = Direction.Left;
		}
		
		if (newY < yMinimum) {
			newY = yMinimum;
			yDirection = Direction.Up;
		}
		
		if (newY > yMaximum) {
			newY = yMaximum;
			yDirection = Direction.Down;
		}
		
		// We move the object by simply updating its position with the newX and newY
		this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		
		// Now for rotation
		float newRotation = this.transform.localEulerAngles.z;
		
		newRotation += (Time.deltaTime * rotationSpeed);
		
		this.transform.localEulerAngles = new Vector3(0,0,newRotation);
	}
}
