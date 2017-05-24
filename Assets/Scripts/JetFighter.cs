using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JetFighter : MonoBehaviour {
	
	private bool isMoving;
	private Player target;
	public string type;
	public GameObject sideExplosionPrefab, explosionPrefab;
	private bool countForDamage, damage;
	public float health;
	public float missilelaunchDelay, countForDamageDelay;
	public GameObject missile;
	public Rigidbody2D myRigidBody;
	public Animator myAnimator;
	public float speed;
	private float missileLaunchTimer, countForDamageTimer;
	public float direction;
	// Use this for initialization
	void Start () {
		//Limit and Plane 
		Physics2D.IgnoreLayerCollision (10, 9, true);
		//Plane and Plane 
		Physics2D.IgnoreLayerCollision (9, 9, true);
		//Plane and Character 
		Physics2D.IgnoreLayerCollision (8, 9, true);
		myAnimator = GetComponent<Animator> ();
		myRigidBody = GetComponent<Rigidbody2D> ();
		direction = transform.localScale.x / Mathf.Abs (transform.localScale.x);
		missileLaunchTimer = 0.0f;
		countForDamage = false;
		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {



		if (countForDamage) {
			countForDamageTimer += Time.deltaTime; 
			if (countForDamageTimer >= countForDamageDelay) {
				countForDamageTimer = 0.0f;
				ReturnBackToNormal ();
			}
		}

		missileLaunchTimer += Time.deltaTime;
		if (missileLaunchTimer >= missilelaunchDelay) {
			missileLaunchTimer = 0.0f;
			if (type.Equals ("Heli")) {
				Instantiate (missile, transform.GetChild (0).transform.position, Quaternion.identity);
			} else {
				Instantiate (missile, transform.GetChild (0).transform.position, Quaternion.Euler (0, 0, -90.0f));
			}

		}

		if (type.Equals ("Heli")) {
			try 
			{
				target = FindObjectOfType<Player> ();
				if (transform.position.x > target.transform.position.x && direction > 0 || transform.position.x < target.transform.position.x && direction < 0) {
					ChangeDirection ();
				}
				if (Mathf.Abs (transform.position.x - target.transform.position.x) > 6.0f) {
					isMoving = true;
				}

				if (Mathf.Abs (transform.position.x - target.transform.position.x) < 4.0f) {
					isMoving = false;
				}

				if (isMoving) {
					myRigidBody.velocity = new Vector2 (direction * speed, myRigidBody.velocity.y);
				} else {
					myRigidBody.velocity = new Vector2 (0.0f, myRigidBody.velocity.y);
				}
			}catch (Exception e) {
				e.ToString ();
			}

		} else {
			myRigidBody.velocity = new Vector2 (direction * speed, myRigidBody.velocity.y);
		}

	}

	public void Initilize (float direction)
	{
		this.direction = direction;
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * direction / Mathf.Abs (direction), transform.localScale.y, transform.localScale.z);
	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Finish") {
			Destroy (gameObject);
		}

		if (col.tag == "PlayerBullet0" || col.tag == "PlayerMissile0" || col.tag == "PlayerBullet1" || col.tag == "Explosion" || col.tag == "PlayerTouch" || col.tag == "PlayerBullet2" || col.tag == "PlayerBullet3") {
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


			if (health > 0) {
				TakeDamage ();
				TakeDamage ();
				if (col.tag.Contains ("Bullet")) {
					//This is the bullet sound on metal
					FindObjectOfType<CanvasScript> ().sounds [UnityEngine.Random.Range (1, 4)].Play ();
				}
				else {
					//This is the missile Hit sound
					FindObjectOfType<CanvasScript> ().sounds [4].Play ();
				}
			} else {
				FindObjectOfType<CanvasScript> ().score++;
				FindObjectOfType<CanvasScript> ().scoreImage.GetComponent<Animator> ().SetTrigger ("animate");
				if (type.Equals ("Heli")) {
					myAnimator.SetTrigger ("fall");
					myRigidBody.gravityScale = 0.5f;
				} else {
					Instantiate (explosionPrefab, transform.position, Quaternion.identity);
					Destroy (gameObject);
				}
			}
		}

		if (col.tag == "Ground") {
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}


	public void TakeDamage ()
	{
		countForDamage = true;
		GetComponent<SpriteRenderer> ().color = new Color (1, 0.5f, 0.5f, 1);
		countForDamageTimer = 0.0f;
		damage = true;
		myRigidBody.velocity = new Vector2 (0, myRigidBody.velocity.y);
	}

	public void ReturnBackToNormal ()
	{
		countForDamage = false;
		damage = false;
		GetComponent<SpriteRenderer> ().color = new Color (1, 1f, 1f, 1);
	}

	public void ChangeDirection ()
	{
		direction = -direction;
		transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}
}
