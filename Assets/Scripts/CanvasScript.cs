using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CanvasScript : MonoBehaviour {

	public Sprite redBulletSprite, blueBulletSprite, yellowBulletSprite;
	public Button airMissileButton;
	public Image specialBulletImage;
	public Text NumberOfSpecialBulletText;
	public int highestScore;
	public Text highestScoreText;
	public int score;
	public Text scoreText;
	public Image scoreImage;
	public Text numberOfRepairItemsText, numberOfAirMissileText;
	public GameObject pauseMenu;
	public AudioSource[] sounds;
	Player player;
	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource> ();
		highestScore = PlayerPrefs.GetInt ("HighestScore");
		highestScoreText.text = "" + highestScore;
		player = FindObjectOfType<Player> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		try 
		{
			if (player.numberOfSpecialBullet > 0)
			{
				if (!specialBulletImage.gameObject.activeInHierarchy)
				{
					specialBulletImage.gameObject.SetActive (true);
				}
				NumberOfSpecialBulletText.text = "" + player.numberOfSpecialBullet;
			}
			else 
			{
				if (specialBulletImage.gameObject.activeInHierarchy)
				{
					specialBulletImage.gameObject.SetActive (false);
				}
			}
			scoreText.text = "" + score;
			numberOfRepairItemsText.text = "" + player.numberOfRepairItems;
			numberOfAirMissileText.text = "" + player.numberOfAirMissile;
		}
		catch (Exception e)
		{
			e.ToString ();
		}
	}

	public void PauseButton ()
	{
		Time.timeScale = 0.0f;
		pauseMenu.SetActive (true);
	}

	public void ResumeButton ()
	{
		Time.timeScale = 1.0f;
		pauseMenu.SetActive (false);
	}

	public void QuitButton (){
		Application.Quit ();
	}

	public void RequestReloadLevel ()
	{
		Invoke ("ReloadLevel", 4.0f); 
	}

	public void ReloadLevel ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void StartButton ()
	{
		LoadingScreenManager.LoadScene (2);
		//Application.LoadLevel (1);
	}

	public void QuitInGameButton ()
	{
		Time.timeScale = 1.0f;
		LoadingScreenManager.LoadScene (0);
	}


}
