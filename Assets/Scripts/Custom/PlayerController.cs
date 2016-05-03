using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	public float blinkRate = 0.1f;
	public int numberOfTimesToBlink = 50;
	private int blinkCount =0 ;
	public GameObject ExplosionPrefab;
	public  GameObject spaceship;
	Renderer[] renderers;
	IEnumerator CreateInv(){

			LifeManager.state = LifeManager.State.Invurnerable;
			blinkCount = 0;			
			while( blinkCount < numberOfTimesToBlink)
			{
				ToggleVisibility();
				blinkCount++;
				yield return new WaitForSeconds(blinkRate);
			}
			//	yield return new WaitForSeconds(blinkRate);
			LifeManager.state = LifeManager.State.Playing;
	}
		
	
	public void Invulnerability ()
	{
	StartCoroutine (CreateInv());
	}
	void ToggleVisibility ()
	{
		// toggles the visibility of this gameobject and all it's children
		foreach (Renderer re in renderers) {
			re.enabled = !re.enabled;
		}
	}
	// Use this for initialization
	void Start () {
		renderers = spaceship.GetComponentsInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void  die(){
	
	Instantiate (ExplosionPrefab, spaceship.transform.position, spaceship.transform.rotation);	
	Destroy(spaceship);
	StartCoroutine (ReallyDie());
	}
	
	
	IEnumerator ReallyDie(){

			yield return new WaitForSeconds(4);
			Application.LoadLevel ("Lose");

	}

}
