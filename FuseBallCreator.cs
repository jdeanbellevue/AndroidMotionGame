using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Profile;
using GoogleMobileAds.Api;

public class FuseBallCreator : MonoBehaviour {
	
	public static int BallsSpawned;
	public Rigidbody2D prefab;
	public float TimeBetweenSpawns;
	public float VerticalGravity;
	public float HorizontalGravityMagnitude;
	public GUISkin FontSkin;
	public float CounterDowner;
	bool Preload;
	Vector3 AccelInput;
	Vector2 BallPlacement;
	Vector2 BallStartPlacement;
	Vector2 GravitySet;
	int HighScore;
	int AdCountdown;
	public InterstitialAd interstitial;
	public static bool ShowGameOver;
	// Use this for initialization
	void Start () {
		RequestInterstitial ();
		AdCountdown = PlayerPrefs.GetInt ("AdCountdown");
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		if (!PlayerPrefs.HasKey ("HighScore")) {
			PlayerPrefs.SetInt ("HighScore", 0);
		}
		ShowGameOver = false;
		HighScore = PlayerPrefs.GetInt ("HighScore");
		Preload = true;
		BallsSpawned = 0;
		BallPlacement = Camera.main.ViewportToWorldPoint (new Vector2 (0.1f, 1f));
		BallPlacement.x += 0.3f;
		BallPlacement.y -= 0.6f + 2f;
		BallStartPlacement = BallPlacement;
		CounterDowner = 3;
		StartCoroutine (MakeBall ());
		Time.timeScale = 6;
	}

	// Update is called once per frame
	void Update () {
		float CountArea;
		float CountAreaShow;
		SpriteRenderer[] all_balls = FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];



		AccelInput = Input.acceleration.normalized;
		AccelInput.x = Mathf.Clamp (AccelInput.x, -0.7f, 0.7f);
		AccelInput.x *= HorizontalGravityMagnitude;
		AccelInput.y = Mathf.Clamp (AccelInput.y, -0.1f, 0.1f);
		AccelInput.y *= VerticalGravity * 10;
		// Give random gravity if run in Unity
		if (AccelInput.x == 0f) {
			AccelInput.x = Random.Range (-10f,10f);
			AccelInput.y = -VerticalGravity;
		}
		GravitySet = new Vector2 (AccelInput.x, -VerticalGravity);
		Physics2D.gravity = GravitySet;
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel("GameSelect"); }

		CountArea = 0f;
		foreach (SpriteRenderer spt in all_balls) {
			if (spt.gameObject.name == "FuseBall(Clone)"){
				CountArea += (spt.bounds.extents.x * spt.bounds.extents.y * Mathf.PI);
			}
		}
		CountAreaShow = CountArea / 48f * 100f;
		if (ShowGameOver == false && CountAreaShow >= 75) {
			Time.timeScale = 0;
			if (BallsSpawned > HighScore) {
				HighScore = BallsSpawned;
				PlayerPrefs.SetInt ("HighScore", HighScore);
				GameObject.Find("HighScore").transform.position = new Vector2(0f, GameObject.Find("HighScore").transform.position.y);
			} else {
				GameObject.Find("Fuse").transform.position = new Vector2(0f, GameObject.Find("Fuse").transform.position.y);
			}
			if (AdCountdown < 1) {
				if (interstitial.IsLoaded()) {
					interstitial.Show();
				}
				AdCountdown = 4;
			}
			AdCountdown--;
			PlayerPrefs.SetInt ("AdCountdown", AdCountdown);
			ShowGameOver = true;
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero);
			if (hit.collider != null) {
				if (hit.transform.gameObject.name == "Fuse")
				{
					Application.LoadLevel("LevelFuse");
				}
				if (hit.transform.gameObject.name == "FuseHigh")
				{
					Application.LoadLevel("LevelFuse");
				}
				if (hit.transform.gameObject.name == "TwitterShare")
				{
					if (SoomlaProfile.IsLoggedIn (Provider.TWITTER) == false) {
						SoomlaProfile.Login (
							Provider.TWITTER, "", null);
					} else {
						SoomlaProfile.UpdateStory(
							Provider.TWITTER,
							"I just scored " + PlayerPrefs.GetInt ("HighScore").ToString() + " in FuseBall!",
							"FuseBall Name",
							"FuseBall Caption",
							"FuseBall Description",
							"http://fuseball.net",
							"http://fuseball.com",
							"",
							null);
						GameObject.Find("TYTweet").transform.position = new Vector2(0f, GameObject.Find("TYTweet").transform.position.y);
					}
				}
			}
		}
	}

	void OnGUI()
	{

		GUI.skin = FontSkin;
		GUI.skin.label.fontSize = Screen.height / 16;
		GUI.skin.button.fontSize = Screen.height / 16;
		if (Preload != true) {
			GUI.Label (new Rect (Screen.width / 5 * 2, Screen.height / 32, Screen.width / 4 * 3, Screen.height / 12), BallsSpawned.ToString ());
		} else {
			GUI.Label (new Rect (Screen.width / 2 - 5, Screen.height / 3, Screen.width / 4 * 3, Screen.height / 6), CounterDowner.ToString ());
		}
		if (ShowGameOver == true) {
			GUI.Label (new Rect ((Screen.width) / 10 * 3, Screen.height / 32 * 3, Screen.width / 5 * 3, Screen.height / 12), "High: " + PlayerPrefs.GetInt ("HighScore"));
		}
	}

	IEnumerator MakeBall(){
		while(true){
			yield return new WaitForSeconds(TimeBetweenSpawns);
			Instantiate(prefab, BallPlacement, transform.rotation);
			BallsSpawned++;
			if (TimeBetweenSpawns > 0.1f) {
				TimeBetweenSpawns -= TimeBetweenSpawns * 0.0025f;
			}
//			Time.timeScale += 0.005f;
			if (BallsSpawned == 7 && Preload) {
				CounterDowner = 2;
			}
			if (BallsSpawned == 14 && Preload) {
				CounterDowner = 1;
			}
			if (BallsSpawned == 21 && Preload) {
				CounterDowner = 0;
				BallsSpawned = 1;
				GameObject.Find("Curtain").GetComponent<SpriteRenderer>().sortingOrder = 0;
//				GameObject.Find("Score").GetComponent<MeshRenderer>().enabled = true;
				Preload = false;
				VerticalGravity = 5f;
				Time.timeScale = 1.3f;

			}
			BallPlacement.x += Mathf.Floor(Random.Range (1f,5f));
			if (BallPlacement.x >= 2.1f){
				BallPlacement.x -= 5f;
			}
			if (BallPlacement.x < BallStartPlacement.x){
				BallPlacement.x = BallStartPlacement.x;
			}
		}
	}	

	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6119541993541839/3021072108";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice("8EE2A9018985B31E306B47C7D3719F7D")
			.AddTestDevice(AdRequest.TestDeviceSimulator) 
			.Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}
}
