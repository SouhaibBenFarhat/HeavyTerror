using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraScript : MonoBehaviour {

	private Player player;
	public float minx, maxx, miny,maxy, distance;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		try 
		{
			transform.position = new Vector3 (Mathf.Clamp (player.transform.position.x, minx, maxx), Mathf.Clamp(player.transform.position.y, miny, maxy), distance);
		}
		catch (Exception e) {
			e.ToString ();
		}
	}
}
