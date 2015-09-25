using UnityEngine;
using System.Collections;

public class FullScreenResize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer WaterSprite;
		float Width;
		float Height;
		float ScreenWidth;
		float ScreenHeight;
		Vector3 NewSize;

		WaterSprite = GetComponent<SpriteRenderer> ();
		transform.localScale = Vector3.one;
		Width = WaterSprite.bounds.size.x;
		Height = WaterSprite.bounds.size.y;
		ScreenHeight = Camera.main.orthographicSize * 2f;
			ScreenWidth = ScreenHeight / Screen.height * Screen.width;
		NewSize = new Vector3 (ScreenWidth / Width, ScreenHeight / Height, transform.localScale.z);
		transform.localScale = NewSize;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
