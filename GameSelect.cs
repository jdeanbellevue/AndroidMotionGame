using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Profile;

public class GameSelect : MonoBehaviour {

	public static bool SoomlaInitalized;
	// Use this for initialization
	void Start () {
//have to remove next line after testing
		PlayerPrefs.SetInt ("HighScore", 0);
		if (!SoomlaInitalized) { 
			SoomlaProfile.Initialize ();
			SoomlaInitalized = true;
		}
		PlayerPrefs.SetInt ("AdCountdown", 0);
	}
	 	
/*	void OnMouseDown () {
		switch (gameObject.name) {
		case "Happy":
			Application.LoadLevel("LevelHappy");
			break;
		case "Fuse":
			Application.LoadLevel("LevelFuse");
			break;
		case "Zen":
			Application.LoadLevel("LevelZen");
			break;
		}
	}
*/
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				if (hit.transform.gameObject.name == "Fuse")
				{
					Application.LoadLevel("LevelFuse");
				}
			}
		}
	}
}