using UnityEngine;
using System.Collections;

public class MoveCube : MonoBehaviour
{
	private float accumTime;
	
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		accumTime += Time.deltaTime;

		transform.position = new Vector3(Mathf.Sin(accumTime) * 10.0f, transform.position.y, transform.position.z);
	
	}
}
