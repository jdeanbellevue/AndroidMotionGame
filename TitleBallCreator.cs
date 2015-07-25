using UnityEngine;
using System.Collections;

public class TitleBallCreator : MonoBehaviour {
	
	public Rigidbody2D prefab;
	public float TimeBetweenSpawns;
	public float VerticalGravity;
	public float HorizontalGravityMagnitude;
	Vector3 AccelInput;
	Vector2 BallPlacement;
	Vector2 BallStartPlacement;
	Vector2 GravitySet;
	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		BallPlacement = Camera.main.ViewportToWorldPoint (new Vector2 (0.1f, 1f));
		BallPlacement.x += 0.3f;
		BallStartPlacement = BallPlacement;
		StartCoroutine (MakeBall ());
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update () {
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

	}

	IEnumerator MakeBall(){
		while(true){
			yield return new WaitForSeconds(TimeBetweenSpawns);
			Instantiate(prefab, BallPlacement, transform.rotation);
			BallPlacement.x += Mathf.Floor(Random.Range (1f,5f));
			if (BallPlacement.x >= 2.1f){
				BallPlacement.x -= 5f;
			}
			if (BallPlacement.x < BallStartPlacement.x){
				BallPlacement.x = BallStartPlacement.x;
			}
		}
	}	
}
