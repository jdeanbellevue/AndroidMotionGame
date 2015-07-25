using UnityEngine;
using System.Collections;

public class FuseBallController : MonoBehaviour {
	int colorpicker;
	Rigidbody2D rigbod;
	Color col;
	bool isDestroyed;
	public static int NumberOfColors = 6;
	public static int NumberForPop = 4;
	void Start () {
		switch(Random.Range (0, NumberOfColors))
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

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == gameObject.tag) {
			if (transform.localScale.x > coll.gameObject.transform.localScale.x && !isDestroyed) {
				transform.localScale += new Vector3 (coll.gameObject.transform.localScale.x - 0.5f, coll.gameObject.transform.localScale.y - 0.5f, 0);
				GetComponent<Rigidbody2D>().mass = 0.3f;
				coll.gameObject.GetComponent<FuseBallController>().isDestroyed = true;
				Destroy (coll.gameObject);
			} else if (transform.localScale.x == coll.gameObject.transform.localScale.x 
			           && coll.gameObject.transform.position.y >= transform.position.y 
			           && !isDestroyed) {
				transform.localScale += new Vector3 (coll.gameObject.transform.localScale.x - 0.5f, coll.gameObject.transform.localScale.y - 0.5f, 0);
				GetComponent<Rigidbody2D>().mass = 0.3f;
				coll.gameObject.GetComponent<FuseBallController>().isDestroyed = true;
				Destroy (coll.gameObject);
			}
			if (transform.localScale.x >= 0.6f + (0.2f * NumberForPop)) {
				ExplodeMe(gameObject);
				BounceBalls(transform.position, GetComponent<SpriteRenderer>().bounds.extents.x + 0.5f);
			}
		}
	}

	public void BounceBalls(Vector3 point, float radius) {
		float px;
		Collider2D[] nearby = Physics2D.OverlapCircleAll (point, radius);
		if (nearby != null) {
			foreach (Collider2D nearball in nearby) {
				if (nearball.gameObject.layer == LayerMask.NameToLayer ("Ball")) {
					px = nearball.transform.position.x - point.x;
					nearball.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (px, 1.5f), ForceMode2D.Impulse);
				}
			}
		}
	}
	


	public void ExplodeMe(GameObject Victim){
		Victim.GetComponent<FuseBallController> ().isDestroyed = true;
		Victim.GetComponent<Rigidbody2D>().isKinematic = true;
		Victim.GetComponent<CircleCollider2D> ().enabled = false;
		Victim.name = "Dying Ball";
		Victim.layer = LayerMask.NameToLayer("Explosion");
		Victim.transform.position = new Vector3 (Victim.transform.position.x, Victim.transform.position.y, Victim.transform.position.z +1);
		StartCoroutine(GrowFade(Victim));
	}
	
	IEnumerator GrowFade (GameObject obj)
	{
		Renderer rend = obj.GetComponent<Renderer>();
		Color newColor;
		float alphaValue = 1.0f;
		rend.sortingOrder = 1;
		if (Time.timeScale == 0f) {
			yield return null;
		}
		while (alphaValue >= 0.3f)
		{
			alphaValue -= Time.deltaTime;
			newColor = rend.material.color;
			newColor.a = Mathf.Min ( newColor.a, alphaValue ); 
			newColor.a = Mathf.Clamp (newColor.a, 0.0f, 1.0f); 				
			rend.material.SetColor("_Color", newColor) ; 
			obj.transform.localScale += new Vector3(0.07f, 0.07f, 0);
			yield return null; 
		}
		Destroy (obj);
		yield return null;
	}
}
