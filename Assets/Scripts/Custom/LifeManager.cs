using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {



	public static float maxLife = 100.0f;
	public static float life;
	public float barDisplay = 0;
	public Vector2 pos = new Vector2(20,40);
	public Vector2 size = new Vector2(60,20);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	public Light shield;
	public static Light Sshield;

	public static  PlayerController player;
	public  PlayerController playe;
	
	public static State state = State.Playing;
	
	public enum State
	{
		Playing,
		Invurnerable
	}
	
	 
	void Awake(){
		life = 1;
	}

	void Start(){
		player = playe;
		Sshield = shield;	
	}
	
	void OnGUI()
	{

	    // draw the background:
	    GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			GUI.Box (new Rect (0f,0f, size.x, size.y),progressBarEmpty);

        	// draw the filled-in part:
        	GUI.BeginGroup (new Rect (0f, 0f, size.x * life, size.y));
       		GUI.Box (new Rect (0f,0f, size.x, size.y),progressBarFull);
      		GUI.EndGroup ();

    	GUI.EndGroup ();
	} 

	public static void getHit(int damage){
		damage = damage *2;
		if(state != State.Invurnerable)
		{
		if(life <= 0)
			player.die();
		else
		{	
		LifeManager.state = LifeManager.State.Invurnerable;
			
		life = (life* maxLife - damage) /maxLife;	
		if(life <= 0)
			turnOffShield();

		player.Invulnerability ();
		}
		}
	}




	static void  turnOffShield(){
		Sshield.enabled = false;	
	}
	
	
}
