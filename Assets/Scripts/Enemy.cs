using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character {

	private Player target;
	public float attackDistance;
	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (type.Equals ("Soldier")) {
			onGround = IsGrounded ();
			HandleLayers ();
			if (myRigidBody.velocity.y < 0.0f) {
				myAnimator.SetBool ("land", true);
			}
		}
		try 
		{
			if (!type.Equals ("Base"))
			{
				target = FindObjectOfType<Player> ();
				if (!attack && !damage) {
					Move (); 
				}

				if (transform.position.x > target.transform.position.x && direction > 0 || transform.position.x < target.transform.position.x && direction < 0)
				{
					ChangeDirection ();
				}

				if (Mathf.Abs (transform.position.x - target.transform.position.x) <= attackDistance && !attack)
				{
					if (type.Equals ("Soldier"))
					{
						if (onGround)
						{
							myAnimator.SetTrigger ("attack");
						}
					}
					else 
					{
						myAnimator.SetTrigger ("attack");
					}
				}
			}
		}
		catch (Exception e)
		{
			e.ToString ();
		}
	}

	public void Move ()
	{
		myRigidBody.velocity = new Vector2 (speed * direction, myRigidBody.velocity.y);
	}


	public void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "PlayerBullet0" || col.tag == "PlayerMissile0" || col.tag == "PlayerBullet1" || col.tag == "Explosion" || col.tag == "PlayerTouch" || col.tag == "PlayerBullet2" || col.tag == "PlayerBullet3") {

			if (!type.Equals ("Base")) {
				switch (col.tag) {
				case "PlayerBullet0":
					health -= 10.0f;
					Destroy (col.gameObject);
					break; 

				case "PlayerBullet1": 
					health -= 20.0f;
					Destroy (col.gameObject);
					break;

				case "PlayerBullet2": 
					health -= 30.0f;
					Destroy (col.gameObject);
					break;

				case "PlayerBullet3": 
					health -= 35.0f;
					Destroy (col.gameObject);
					break;

				case "PlayerMissile0": 
					health -= 40.0f;
					GameObject tmp = Instantiate (sideExplosionPrefab, col.transform.position, Quaternion.identity);
					float direction = col.GetComponent<Bullet> ().direction; 
					tmp.transform.localScale = new Vector3 (-Mathf.Abs (tmp.transform.localScale.x) * direction / Mathf.Abs (direction), tmp.transform.localScale.y, tmp.transform.localScale.z);
					Destroy (col.gameObject);
					break; 

				case "Explosion": 
					health -= 60.0f;
					break;
				case "PlayerTouch":
					health -= 200.0f;
					break;
				}
			} else {
				//this is the bast
				myAnimator.SetTrigger ("changeState");
			}


			if (health > 0) {
				TakeDamage ();
				//TakeDamage ();
				if (col.tag.Contains ("Bullet")) {
					if (!type.Equals ("Soldier") && !type.Equals ("Base")) {
						//This is the bullet sound on metal
						FindObjectOfType<CanvasScript> ().sounds [UnityEngine.Random.Range (1, 4)].Play ();
					}
				}
				else {
					//This is the missile hit sound 
					if (!type.Equals ("Base")) {
						FindObjectOfType<CanvasScript> ().sounds [4].Play ();
					}
				}
			} else {
				if (!type.Equals ("Base")) {
					dead = true;
					FindObjectOfType<CanvasScript> ().score++;
					FindObjectOfType<CanvasScript> ().scoreImage.GetComponent<Animator> ().SetTrigger ("animate");
					Instantiate (explosionPrefab, transform.position, Quaternion.identity);
					Destroy (gameObject);
				}
			}
		}

	}



}
