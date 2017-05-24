using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bar : MonoBehaviour {

	private Player player;
	private float maxHealth;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player> ();
		maxHealth = player.health;
	}
	
	// Update is called once per frame
	void Update () {
		try {
			GetComponent<Image> ().fillAmount = player.health / maxHealth;
		}
		catch (Exception e) {
		}
	}
}
