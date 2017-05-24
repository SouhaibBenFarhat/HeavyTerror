using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

	private int selectedSpecialBullet;
	public int numberOfSpecialBullet;
	public int numberOfAirMissile;
	private float maxHealth;
	public int numberOfRepairItems;
	public GameObject BlackForMissile;
	public float missileRecoverSpeed;
	private float horizontal, buttonHorizontal;
	private bool move, aimAMode, aimingUp, aimingDown, aimingMiddle;
	// Use this for initialization
	public override void Start () {
		base.Start ();
		move = false;
		aimAMode = aimingUp = aimingMiddle = aimingDown = false;
		maxHealth = health;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (!dead) {
			if (numberOfSpecialBullet <= 0) {
				selectedSpecialBullet = 0;
			}
			if (BlackForMissile.GetComponent<Image> ().fillAmount != 0) {
				BlackForMissile.GetComponent<Image> ().fillAmount -= missileRecoverSpeed;
			}

			if (!aimAMode) {
				if (move) {
					buttonHorizontal = Mathf.Lerp (buttonHorizontal, direction, Time.smoothDeltaTime * 5);
					myRigidBody.velocity = new Vector2 (buttonHorizontal * speed, myRigidBody.velocity.y);
				} else {
					horizontal = Input.GetAxis ("Horizontal");
					if (horizontal > 0 && direction < 0 || horizontal < 0 && direction > 0) {
						ChangeDirection ();
					}
					myRigidBody.velocity = new Vector2 (horizontal * speed, myRigidBody.velocity.y);
				}
			}

			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				if (BlackForMissile.GetComponent<Image> ().fillAmount == 0) {
					Fire (1, 4, new Vector3 (0, 0, 0), 2);
					BlackForMissile.GetComponent<Image> ().fillAmount = 1;
				}
			}

			if (Input.GetKeyDown (KeyCode.RightArrow) && direction < 0 || Input.GetKeyDown (KeyCode.LeftArrow) && direction > 0) {
				if (aimAMode) {
					ChangeDirection ();
				}
			}

			if (Input.GetKeyDown (KeyCode.RightShift)) {
				Shoot ();
			}

			if (Input.GetKeyDown (KeyCode.RightAlt)) {
				if (numberOfAirMissile > 0) {
					numberOfAirMissile--;
					Fire (2, 6, Vector3.zero, 2);
					Fire (2, 7, Vector3.zero, 2);
					Fire (2, 8, Vector3.zero, 2);
				}
			}

			if (Input.GetKeyDown (KeyCode.Space)) {
				Aim ();
			}

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				if (aimAMode && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimUp")) {
					if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimDown")) {
						aimingMiddle = true;
						aimingDown = false;
					}
					if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimMiddle")) {
						aimingMiddle = false;
						aimingUp = true;
					}
					myAnimator.SetTrigger ("aimAUp");
				}
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if (aimAMode && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimDown")) {
					if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimUp")) {
						aimingMiddle = true;
						aimingUp = false;
					}
					if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimMiddle")) {
						aimingMiddle = false;
						aimingDown = true;
					}

					myAnimator.SetTrigger ("aimADown");
				}
			}
		} else {
		}

		myAnimator.SetFloat ("speed", Mathf.Abs (myRigidBody.velocity.x));

	}
		

	public void OnTriggerEnter2D (Collider2D col)
	{
		if (!dead) {

			if (col.tag == "EnemyBullet0" || col.tag == "EnemyMissile0" || col.tag == "EnemyBullet1" || col.tag == "Explosion") {
				switch (col.tag) {
				case "EnemyBullet0":
					health -= 10.0f;
					Destroy (col.gameObject);
					break; 
				case "EnemyBullet1": 
					health -= 20.0f;
					Destroy (col.gameObject);
					break;
				case "EnemyMissile0": 
					health -= 40.0f;
					GameObject tmp = Instantiate (sideExplosionPrefab, col.transform.position, Quaternion.identity);
					float direction = col.GetComponent<Bullet> ().direction; 
					tmp.transform.localScale = new Vector3 (-Mathf.Abs (tmp.transform.localScale.x) * direction / Mathf.Abs (direction), tmp.transform.localScale.y, tmp.transform.localScale.z);
					Destroy (col.gameObject);
					break; 
				case "Explosion": 
					health -= 60.0f;
					break;
				}


				if (health > 0) {
					TakeDamage ();
					if (col.tag.Contains ("Bullet")) {
						canvas.sounds [UnityEngine.Random.Range (1, 4)].Play ();
					} else {
						canvas.sounds [4].Play ();
					}
				} else {
					dead = true;
					CanvasScript canvas = FindObjectOfType<CanvasScript> ();
					Instantiate (explosionPrefab, transform.position, Quaternion.identity);
					if (canvas.score > canvas.highestScore) {
						PlayerPrefs.SetInt ("HighestScore", canvas.score);
					}
					canvas.RequestReloadLevel ();
					Destroy (gameObject);
				}
			}

			if (col.tag == "RepairItem") {
				Destroy (col.gameObject);
				numberOfRepairItems++;
			}

			if (col.tag == "Box")
			{
				CanvasScript canvas = FindObjectOfType<CanvasScript> ();
				int whichAmmo = UnityEngine.Random.Range (1, 3);
				if (whichAmmo == 1) {
					numberOfAirMissile ++;
					canvas.airMissileButton.GetComponent<Animator> ().SetTrigger ("animate");
				} else {
					numberOfSpecialBullet = 100;
					int whichSpecialBullet = UnityEngine.Random.Range (1, 4);
					switch (whichSpecialBullet) {
					case 1:
						selectedSpecialBullet = 3;
						canvas.specialBulletImage.GetComponent<Image> ().sprite = canvas.redBulletSprite;
						break;
					case 2: 
						selectedSpecialBullet = 4;
						canvas.specialBulletImage.GetComponent<Image> ().sprite = canvas.blueBulletSprite;
						break; 
					case 3: 
						selectedSpecialBullet = 5;
						canvas.specialBulletImage.GetComponent<Image> ().sprite = canvas.yellowBulletSprite;
						break;
					}
				}
				Destroy (col.gameObject.transform.parent.gameObject);

			}
		}
	}

	public void Shoot ()
	{
		if (numberOfSpecialBullet > 0) {
			numberOfSpecialBullet--;
		}
		if (!aimAMode) {
			Fire (selectedSpecialBullet, 0, new Vector3 (0, 0, 90), 1);
		} else {
			if (aimingUp) {
				if (direction > 0) {
					Fire (selectedSpecialBullet, 1, new Vector3 (0, 0, 122), 1);
				} else {
					Fire (selectedSpecialBullet, 1, new Vector3 (0, 0, 60), 1);
				}

			} else {
				if (aimingMiddle) {
					Fire (selectedSpecialBullet, 2, new Vector3 (0, 0, 90), 1);
				} else {
					//We are aiming down here: 
					if (direction > 0) {
						Fire (selectedSpecialBullet, 3, new Vector3 (0, 0, 45), 1);
					} else {
						Fire (selectedSpecialBullet, 3, new Vector3 (0, 0, 135), 1);
					}
				}
			}
		}
	}

	public void Aim ()
	{
		if (aimAMode) {
			aimingMiddle = aimingDown = aimingUp = false;
			myAnimator.SetBool ("aimAMode", false);
		} else {
			aimingMiddle = true;
			myRigidBody.velocity = new Vector2 (0, myRigidBody.velocity.y);
			myAnimator.SetBool ("aimAMode", true);
		}
		aimAMode = !aimAMode;
	}



	//Handling Buttons here
	public void ShootButton ()
	{
		Shoot ();
	}

	public void MoveButton (float direction)
	{
		move = true;
		this.direction = direction;
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * direction / Mathf.Abs (direction), transform.localScale.y, transform.localScale.z);
	}

	public void ResetMove ()
	{
		move = false;
		buttonHorizontal = 0.0f;
		myRigidBody.velocity = new Vector2 (0, myRigidBody.velocity.y);
	}

	public void AimButton ()
	{
		Aim ();
	}

	public void MissileButton ()
	{
		if (BlackForMissile.GetComponent<Image> ().fillAmount == 0) {
			Fire (1, 4, new Vector3 (0, 0, 0), 2);
			BlackForMissile.GetComponent<Image> ().fillAmount = 1;
		}
	}
	//End of handling Buttons


	public void ReloadLevel ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}


	public void Repair ()
	{
		if (numberOfRepairItems > 0) {
			health = maxHealth;
			numberOfRepairItems--;
		}
	}


	public void AimUpButton ()
	{
		if (aimAMode && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimUp")) {
			if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimDown")) {
				aimingMiddle = true;
				aimingDown = false;
			}
			if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimMiddle")) {
				aimingMiddle = false;
				aimingUp = true;
			}
			myAnimator.SetTrigger ("aimAUp");
		}
	}

	public void AimDownButton ()
	{
		if (aimAMode && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimDown")) {
			if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimUp")) {
				aimingMiddle = true;
				aimingUp = false;
			}
			if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("AimMiddle")) {
				aimingMiddle = false;
				aimingDown = true;
			}

			myAnimator.SetTrigger ("aimADown");
		}
	}


	public void AirMissileButton ()
	{
		if (numberOfAirMissile > 0) {
			numberOfAirMissile--;
			Fire (2, 6, Vector3.zero, 2);
			Fire (2, 7, Vector3.zero, 2);
			Fire (2, 8, Vector3.zero, 2);
		}
	}
		
}
