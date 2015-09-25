using UnityEngine;
using System.Collections;

public class UIControls : MonoBehaviour {

	public Sprite PlaySprite;
	public Sprite PauseSprite;
	public Sprite MuteSprite;
	public Sprite NoMuteSprite;
	AudioSource SoundSource;
	// Use this for initialization
	void Start () {
		SoundSource = GetComponent<AudioSource> ();
		if (PlayerPrefs.GetInt ("IsMuted") == 1) {
			GameObject.Find ("Mute").GetComponent<SpriteRenderer> ().sprite = MuteSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.NameToLayer ("UI"));
			if (hit.collider != null) {
				if (hit.transform.gameObject.name == "Pause" && FuseBallCreator.ShowGameOver == false) {
					if (Time.timeScale > 0f) {
						Time.timeScale = 0f;
						SoundSource.Pause();
						GameObject.Find ("Pause").GetComponent<SpriteRenderer> ().sprite = PlaySprite;
					} else {
						Time.timeScale = 1.3f;
						SoundSource.UnPause();
						GameObject.Find ("Pause").GetComponent<SpriteRenderer> ().sprite = PauseSprite;
					}
				}
				if (hit.transform.gameObject.name == "Mute") {
					if (PlayerPrefs.GetInt ("IsMuted") == 1) {
						PlayerPrefs.SetInt ("IsMuted", 0);
						GameObject.Find ("Mute").GetComponent<SpriteRenderer> ().sprite = NoMuteSprite;
						SoundSource.mute = false;
					} else {
						PlayerPrefs.SetInt ("IsMuted", 1);
						GameObject.Find ("Mute").GetComponent<SpriteRenderer> ().sprite = MuteSprite;
						SoundSource.mute = true;
					}
				}
			}
		}
	}
}
