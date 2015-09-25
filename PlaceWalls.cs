using UnityEngine;
using System.Collections;

public class PlaceWalls : MonoBehaviour {

	Vector2 pos;
	// Use this for initialization
	void Start () {
		if (gameObject.name == "West Wall") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0f, 0.5f));
			pos.y = transform.position.y;
			pos.x = pos.x - 0.1f;
			transform.position = pos;
		} else if (gameObject.name == "East Wall") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (1f, 0.5f));
			pos.y = transform.position.y;
			pos.x = pos.x + 0.1f;
			transform.position = pos;
		} else if (gameObject.name == "South Wall") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 0f));
			pos.x = transform.position.x;
			pos.y = pos.y - 0.1f;
			transform.position = pos;
		} else if (gameObject.name == "North Wall") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 1f));
			pos.x = transform.position.x;
			pos.y = pos.y + 0.1f - 2f;
			transform.position = pos;
		} else if (gameObject.name == "Ramp") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.5f, 0.23f));
			transform.position = pos;
		} else if (gameObject.name == "Pause") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.9f, 0.9f));
			transform.position = pos;
		} else if (gameObject.name == "Mute") {
			pos = Camera.main.ViewportToWorldPoint (new Vector2 (0.1f, 0.9f));
			transform.position = pos;
		}
	}
}
