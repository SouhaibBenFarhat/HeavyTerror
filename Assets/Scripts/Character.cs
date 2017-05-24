using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


	public bool onGround;

	[SerializeField]
	private LayerMask whatIsGround;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private Transform[] groundPoints;

	public string type;
	protected CanvasScript canvas;
	public GameObject sideExplosionPrefab;
	public GameObject explosionPrefab;
	public GameObject[] bullets;
	public bool dead, attack, damage;
	public float direction, speed, health;
	public Rigidbody2D myRigidBody;
	public Animator myAnimator;
	private AudioSource[] sounds;
	private bool countForDamage;
	public float countForDamageDelay;
	private float countForDamageTimer;
	// Use this for initialization
	public virtual void Start () {
		//Character and Character
		Physics2D.IgnoreLayerCollision (8, 8, true);// ignore collision with objects in the same layer
		canvas = FindObjectOfType<CanvasScript> ();
		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
		direction = transform.localScale.x / Mathf.Abs (transform.localScale.x);
		dead = false;
		attack = false;
		sounds = GetComponents<AudioSource> ();
		countForDamage = false;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (countForDamage) {
			countForDamageTimer += Time.deltaTime; 
			if (countForDamageTimer >= countForDamageDelay) {
				countForDamageTimer = 0.0f;
				ReturnBackToNormal ();
			}
		}
	}

	public void ChangeDirection ()
	{
		direction = -direction;
		transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	public void PlaySound (int whichSound)
	{
		sounds [whichSound].Play ();
	}

	public void Fire (int whichBullet, int whichPosition, Vector3 rotation, int whichSound)
	{
		if (this is Player) {
			sounds [whichSound].Play ();
		}
		GameObject tmp = Instantiate (bullets[whichBullet], transform.GetChild (whichPosition).transform.position, Quaternion.Euler (rotation.x, rotation.y, rotation.z));
		tmp.GetComponent<Bullet> ().Initialize (direction);
	}

	public void FireForAnimation (int whichBullet)
	{
		GameObject tmp = Instantiate (bullets[whichBullet], transform.GetChild (0).transform.position, Quaternion.identity);
		tmp.GetComponent<Bullet> ().Initialize (direction);
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

	public void PlayCanvasSound (int whichCanvasSound)
	{
		canvas.sounds [whichCanvasSound].Play ();
	}


	protected bool IsGrounded()
	{
		if (myRigidBody.velocity.y <= 0) 
		{
			foreach (Transform point in groundPoints) 
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) 
				{
					if (colliders [i].gameObject != gameObject) 
					{
						return true; 
					}
				}
			}
		}
		return false; 
	}

	public void HandleLayers ()
	{
		if (onGround) {
			myAnimator.SetLayerWeight (1, 0);
		} else {
			myAnimator.SetLayerWeight (1, 1);
		}
	}
}
