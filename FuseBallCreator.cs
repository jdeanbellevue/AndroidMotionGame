using UnityEngine;
using System.Collections;
//using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class FuseBallCreator : MonoBehaviour {
	
	public static int BallsSpawned;
	public Rigidbody2D prefab;
	public float TimeBetweenSpawns;
	public float VerticalGravity;
	public float HorizontalGravityMagnitude;
	public GUISkin FontSkin;
	public float CounterDowner;
	public static bool Preload;
	Vector3 AccelInput;
	Vector2 BallPlacement;
	Vector2 BallStartPlacement;
	Vector2 GravitySet;
	int HighScore;
	int AdCountdown;
//	public InterstitialAd interstitial;
	public static bool ShowGameOver;
	public AudioClip CountdownSound;
	AudioSource CountDownSource;
	public AudioSource BGMSource;
	string TweetURL;
	string FBURL;
	// Use this for initialization
	void Start () {
//		RequestInterstitial ();
		Advertisement.Initialize ("67341");
		AdCountdown = PlayerPrefs.GetInt ("AdCountdown");
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		ShowGameOver = false;
		HighScore = PlayerPrefs.GetInt ("HighScore");
		BGMSource = GetComponent<AudioSource> ();
		if (PlayerPrefs.GetInt ("IsMuted") == 1) {
			BGMSource.mute = true;
		} else {
			BGMSource.mute = false;
		}
		Preload = true;
		BallsSpawned = 0;
		BallPlacement = Camera.main.ViewportToWorldPoint (new Vector2 (0.1f, 1f));
		BallPlacement.x += 0.3f;
		BallPlacement.y -= 0.6f + 2f;
		BallStartPlacement = BallPlacement;
		CounterDowner = 3;
		StartCoroutine (MakeBall ());
		CountDownSource = GetComponent<AudioSource>();
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
		GravitySet = new Vector2 (AccelInput.x, -VerticalGravity);
		Physics2D.gravity = GravitySet;

		CountArea = 0f;
		foreach (SpriteRenderer spt in all_balls) {
			if (spt.gameObject.name == "FuseBall(Clone)"){
				CountArea += (spt.bounds.extents.x * spt.bounds.extents.y * Mathf.PI);
			}
		}
		CountAreaShow = CountArea / 48f * 100f;
		if (ShowGameOver == false && CountAreaShow >= 75) {
			Time.timeScale = 0;
			BGMSource.Stop();
			if (BallsSpawned > HighScore) {
				HighScore = BallsSpawned;
				PlayerPrefs.SetInt ("HighScore", HighScore);
				GameObject.Find("HighScore").transform.position = new Vector2(0f, GameObject.Find("HighScore").transform.position.y);
			} else {
				GameObject.Find("GameOver").transform.position = new Vector2(0f, GameObject.Find("GameOver").transform.position.y);
			}
			GameObject.Find("Fuse").transform.position = new Vector2(0f, GameObject.Find("Fuse").transform.position.y);
			if (AdCountdown < 1) {
				if(Advertisement.IsReady()) { 
					Advertisement.Show(); 
				}
//				if (interstitial.IsLoaded()) {
//					interstitial.Show();
//				}
				AdCountdown = 2;
			}
			AdCountdown--;
			PlayerPrefs.SetInt ("AdCountdown", AdCountdown);
			ShowGameOver = true;
		}
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.NameToLayer("UI"));
			if (hit.collider != null) {
				if (hit.transform.gameObject.name == "Fuse")
				{
					Application.LoadLevel("LevelFuse");
				}
				if (hit.transform.gameObject.name == "TwitterShare")
				{
					if (PlayerPrefs.GetInt ("IsMuted") == 0) {
						CountDownSource.PlayOneShot (CountdownSound);
					}
					TweetURL = "https://twitter.com/intent/tweet?text=I just scored " + PlayerPrefs.GetInt ("HighScore").ToString() + " in FuseBall! Unique new motion controlled action-puzzle game! Play FREE &url=http://play.google.com/store/apps/details?id=com.Ablyvion.FuseBall&via=ablyvion&hashtags=FuseBall";
					Application.OpenURL(TweetURL);
					GameObject.Find("TYTweet").transform.position = new Vector2(0f, GameObject.Find("TYTweet").transform.position.y);
					GameObject.Find("BEST").transform.position = new Vector2(0f, GameObject.Find("BEST").transform.position.y);
				}
//				if (hit.transform.gameObject.name == "FBshare")
//				{
//					if (PlayerPrefs.GetInt ("IsMuted") == 0) {
//						CountDownSource.PlayOneShot (CountdownSound);
//					}
// THIS NEEDS A PROPER SCREENSHOT
//					FBURL = "https://m.facebook.com/dialog/feed?app_id=591291461013332&link=http%3A%2F%2Fplay.google.com%2Fstore%2Fapps%2Fdetails%3Fid%3Dcom.Ablyvion.FuseBall&name=Play FuseBall on Android Free! Unique new motion controlled action-puzzle gaming experience!&caption=%20&description=I%20just%20scored%20" + PlayerPrefs.GetInt ("HighScore").ToString() + "%20in%20FuseBall!%20%20Play%20Fuseball%20free%20at%20http%3A%2F%2Fplay.google.com%2Fstore%2Fapps%2Fdetails%3Fid%3Dcom.Ablyvion.FuseBall or check out the gameplay at http://gfycat.com/SmugHoarseConure you control the motion of the balls by rotating your device!&redirect_uri=http%3A%2F%2Fwww.facebook.com%2F&ref=InGameShare&picture=https://fuseballgame.files.wordpress.com/2015/08/fuseballfeature.png";
//					Application.OpenURL(FBURL);
//					GameObject.Find("TYFB").transform.position = new Vector2(0f, GameObject.Find("TYFB").transform.position.y);
//				}
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
			GUI.Label (new Rect ((Screen.width) / 4, Screen.height / 32 * 3, Screen.width / 5 * 3, Screen.height / 12), "BEST: " + PlayerPrefs.GetInt ("HighScore"));
		}
	}

	IEnumerator MakeBall(){
		while(true){
			yield return new WaitForSeconds(TimeBetweenSpawns);
			Instantiate(prefab, BallPlacement, transform.rotation);
			BallsSpawned++;
			if (TimeBetweenSpawns > 0.1f) {
				TimeBetweenSpawns -= TimeBetweenSpawns * 0.00125f;
			}
			if (BallsSpawned == 1 && Preload) {
				if (PlayerPrefs.GetInt ("IsMuted") == 0) {
					CountDownSource.PlayOneShot (CountdownSound);
				}
			}
			if (BallsSpawned == 7 && Preload) {
				CounterDowner = 2;
				if (PlayerPrefs.GetInt ("IsMuted") == 0) {
					CountDownSource.PlayOneShot (CountdownSound);
				}
			}
			if (BallsSpawned == 14 && Preload) {
				CounterDowner = 1;
				if (PlayerPrefs.GetInt ("IsMuted") == 0) {
					CountDownSource.PlayOneShot (CountdownSound);
				}
			}
			if (BallsSpawned == 21 && Preload) {
				CounterDowner = 0;
				BallsSpawned = 1;
				GameObject.Find("Curtain").GetComponent<SpriteRenderer>().sortingOrder = 0;
				Preload = false;
				VerticalGravity = 5f;
				Time.timeScale = 1.3f;
				BGMSource.Play();
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

//	private void RequestInterstitial()
//	{
//		#if UNITY_ANDROID
//		string adUnitId = "ca-app-pub-6119541993541839/3021072108";
//		#elif UNITY_IPHONE
//		string adUnitId = "ca-app-pub-6119541993541839/1405274507";
//		#else
//		string adUnitId = "unexpected_platform";
//		#endif
//		
//		// Initialize an InterstitialAd.
//		interstitial = new InterstitialAd(adUnitId);
//		// Create an empty ad request.
//		AdRequest request = new AdRequest.Builder()
//			.AddTestDevice("8EE2A9018985B31E306B47C7D3719F7D")
//			.AddTestDevice(AdRequest.TestDeviceSimulator) 
//			.Build();
//		// Load the interstitial with the request.
//		interstitial.LoadAd(request);
//	}
}
