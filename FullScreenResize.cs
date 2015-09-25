using UnityEngine;
using System.Collections;

public class FullScreenResize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer CurtainSprite;
		float Width;
		float Height;
		float ScreenWidth;
		float ScreenHeight;
		Vector3 NewSize;

		CurtainSprite = GetComponent<SpriteRenderer> ();
		transform.localScale = Vector3.one;
		Width = CurtainSprite.bounds.size.x;
		Height = CurtainSprite.bounds.size.y;
		ScreenHeight = Camera.main.orthographicSize * 2f;
			ScreenWidth = ScreenHeight / Screen.height * Screen.width;
		NewSize = new Vector3 (ScreenWidth / Width, ScreenHeight / Height, transform.localScale.z);
		transform.localScale = NewSize;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
