using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour {

	public float distanceToPingPong; 
	private float min, max;
	public float rotationSpeed;
	public bool canPingPong;
	// Use this for initialization
	void Start () {
		min = transform.position.y; 
		max = transform.position.y + distanceToPingPong;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, rotationSpeed, 0));
		if (canPingPong) {
			transform.position = new Vector3 (transform.position.x, Mathf.PingPong (Time.time, max - min) + min, transform.position.z);
		}
	}
}
