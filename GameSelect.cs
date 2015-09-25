using UnityEngine;
using System.Collections;

public class GameSelect : MonoBehaviour {

	public AudioSource BGMSource;

	// Use this for initialization
	void Start () {
		BGMSource = GetComponent<AudioSource> ();
		if (PlayerPrefs.GetInt ("IsMuted") == 1) {
			BGMSource.mute = true;
		} else {
			BGMSource.mute = false;
		}
		if (!PlayerPrefs.HasKey ("HighScore")) {
			PlayerPrefs.SetInt ("HighScore", 0);
		}
		if (!PlayerPrefs.HasKey ("IsMuted")){
			PlayerPrefs.SetInt ("IsMuted", 0);
		}
		//have to remove next line after testing
		PlayerPrefs.SetInt ("AdCountdown", 0);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.NameToLayer("UI"));
			if (hit.collider != null) {
				if (hit.transform.gameObject.name == "Fuse")
				{
					Application.LoadLevel("LevelFuse");
				}
			}
		}
	}

	void OnGUI () {
		GUI.skin.label.fontSize = Screen.height / 64;
		GUI.skin.button.fontSize = Screen.height / 64;
		if (GUI.Button (new Rect (Screen.width / 10 * 3, Screen.height / 18 * 17, Screen.width / 5 * 2, Screen.height / 18), "Audio by Austin Shannon\nhttp://www.austinshannon.com")) {
			Application.OpenURL("http://www.austinshannon.com");
		}
	}
}