using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private EdgeCollider2D collider;
	// Use this for initialization
	void Start () {
		collider = GetComponent<EdgeCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateCollider ()
	{
		collider.enabled = true;
		collider.transform.Translate (new Vector3 (0.01f, 0, 0));
		collider.transform.Translate (new Vector3 (-0.01f, 0, 0));
	}

	public void PlayCanvasSound (int whichCanvasSound)
	{
		FindObjectOfType<CanvasScript> ().sounds [whichCanvasSound].Play ();
	}

	public void DeActivateCollider ()
	{
		collider.enabled = false;
	}

	public void Kill ()
	{
		Destroy (gameObject);
	}


}
