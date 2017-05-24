using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector2 addForce;
	public GameObject explosion;
	public float direction;
	public float speed;
	public string type;
	// Use this for initialization
	void Start () {
		if (type.Equals ("AirMissile")) {
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (addForce.x * direction, addForce.y));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (type.Equals ("Bullet")) {
			transform.Translate (new Vector3 (0, -speed * direction, 0));
		} else {
			transform.Translate (new Vector3 (speed * direction, 0, 0));
		}

		if (type.Equals ("AirMissile")) {
			transform.rotation = Quaternion.Euler (transform.rotation.x, transform.rotation.y, direction * GetComponent<Rigidbody2D> ().velocity.y * 8);
		}
	}

	public void Initialize (float direction)
	{
		this.direction = direction;
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * direction / Mathf.Abs (direction), transform.localScale.y, transform.localScale.z);
	}

	public void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Ground")
		{
			if (type.Equals ("JetMissile") || type.Equals ("AirMissile"))
			{
				Instantiate (explosion, transform.position, Quaternion.identity);
				Destroy (gameObject);
			}
		}

		if (col.tag == "Enemy" && type.Equals ("AirMissile")) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
