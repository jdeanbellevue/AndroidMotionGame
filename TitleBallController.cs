using UnityEngine;
using System.Collections;

public class TitleBallController : MonoBehaviour {
	Color col;
	void Start () {
		switch(Random.Range (0, 6))
		{
		case 0:
			col = new Color(1, 1, 0);
			gameObject.tag = "Yellow";
			break;
		case 1:
			col = new Color(1, 0, 0);
			gameObject.tag = "Red";
			break;
		case 2:
			col = new Color(0.2f, 0.6f, 1);
			gameObject.tag = "Blue";
			break;
		case 3:
			col = new Color(0.8f, 0, 0.8f);
			gameObject.tag = "Purple";
			break;
		case 4:
			col = new Color(1, 0.5f, 0);
			gameObject.tag = "Orange";
			break;
		case 5:
			col = new Color(0, 1, 0);
			gameObject.tag = "Green";
			break;
		}
		GetComponent<SpriteRenderer> ().material.color = col;
	}	

	void Update () {
		if (transform.position.y < (Camera.main.ViewportToWorldPoint (new Vector2 (0.0f, 0f)).y -1f)) {
			Destroy (gameObject);
		}
	}
}
