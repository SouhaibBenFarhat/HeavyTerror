using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject soldier, tank, bomberJet, jet, heli, repairItem;
	public float soldierSummonDelay, tankSummonDelay, bomberJetSummonDelay, heliSummonDelay, repairItemSummonDelay;
	private float soldierSummonTimer, tankSummonTimer, bomberJetSummonTimer, heliSummonTimer, repairItemSummonTimer;

	public float jetSummonY, jetSummonX1, jetSummonX2;
	public float airSummonY; 
	public float minX, maxX; 
	public float groundSummonY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		soldierSummonTimer += Time.deltaTime; 
		tankSummonTimer += Time.deltaTime;
		bomberJetSummonTimer += Time.deltaTime;
		//heliSummonTimer += Time.deltaTime;
		repairItemSummonTimer += Time.deltaTime;

		if (soldierSummonTimer >= soldierSummonDelay) {
			soldierSummonTimer = 0.0f;
			Instantiate (soldier, new Vector3 (UnityEngine.Random.Range (minX, maxX), airSummonY, 0), Quaternion.identity);
		}

		if (bomberJetSummonTimer >= bomberJetSummonDelay) {
			bomberJetSummonTimer = 0.0f;
			int whichDirection = UnityEngine.Random.Range (1, 3);
			int whichJet = UnityEngine.Random.Range (1, 3);
			if (whichDirection == 1) {
				if (whichJet == 1) {
					GameObject tmp = Instantiate (bomberJet, new Vector3 (jetSummonX1, jetSummonY, 0), Quaternion.identity);
					tmp.GetComponent<JetFighter> ().Initilize (-1);
				} else {
					GameObject tmp = Instantiate (jet, new Vector3 (jetSummonX1, jetSummonY, 0), Quaternion.identity);
					tmp.GetComponent<JetFighter> ().Initilize (-1);
				}
			} else {
				if (whichJet == 1) {
					GameObject tmp = Instantiate (bomberJet, new Vector3 (jetSummonX2, jetSummonY, 0), Quaternion.identity);
					tmp.GetComponent<JetFighter> ().Initilize (1);
				} else {
					GameObject tmp = Instantiate (jet, new Vector3 (jetSummonX2, jetSummonY, 0), Quaternion.identity);
					tmp.GetComponent<JetFighter> ().Initilize (1);
				}
			}

		}


		if (heliSummonTimer >= heliSummonDelay) {
			heliSummonTimer = 0.0f;
			int whichDirection = UnityEngine.Random.Range (1, 3);
			if (whichDirection == 1) {
				GameObject tmp = Instantiate (heli, new Vector3 (jetSummonX1, jetSummonY, 0), Quaternion.identity);
				tmp.GetComponent<JetFighter> ().Initilize (-1);
			} else {
				GameObject tmp = Instantiate (heli, new Vector3 (jetSummonX2, jetSummonY, 0), Quaternion.identity);
				tmp.GetComponent<JetFighter> ().Initilize (1);
			}

		}

		if (repairItemSummonTimer >= repairItemSummonDelay) {
			repairItemSummonTimer = 0.0f;
			Instantiate (repairItem, new Vector3 (UnityEngine.Random.Range (minX, maxX), groundSummonY, 0), Quaternion.identity);
		}
			


		if (tankSummonTimer >= tankSummonDelay) {
			tankSummonTimer = 0.0f;
			if (UnityEngine.Random.Range (1, 3) == 1) {
				Instantiate (tank, new Vector3 (minX, groundSummonY, 0), Quaternion.identity);
			} else {
				Instantiate (tank, new Vector3 (maxX, groundSummonY, 0), Quaternion.identity);
			}
		}
	}

}
