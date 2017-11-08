using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizePos : MonoBehaviour {


	public List<Transform> mineList;
	float SSMaxX = 34;
	float SSMaxY = 18;

	// Use this for initialization
	void Start () 
	{

		for(int i=0;i<mineList.Count;i++)
		{
			mineList[i].transform.position = new Vector2(Random.Range(-SSMaxX,SSMaxX),Random.Range(-SSMaxY,SSMaxY));
		}
	
	}
	

}
