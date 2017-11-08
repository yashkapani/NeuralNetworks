using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISandbox 
{
	public class TankFitness : MonoBehaviour {


		private float _fitnessValue =0;
		Vector3 mNearestMineDir;
		public float mCheckRadius;
		float SSMaxX = 34;
		float SSMaxY = 18;
		public Collider2D[] listOfNearestMines;
		public GameObject MineData;
		public GameObject mineObj;

		public float TankFitnessValue
		{
			get
			{
				return _fitnessValue;
			}
			set
			{
				_fitnessValue = value;
			}

		}

		void Start()
		{
			mCheckRadius= 5.0f;
		}

	
		public Vector3 GetNearestMine()
		{
			listOfNearestMines = Physics2D.OverlapCircleAll(transform.position,mCheckRadius,1 << LayerMask.NameToLayer("Mines") );
			if(listOfNearestMines.Length == 0)
			{
				return Vector3.zero;
			}
		
			float minDist = mCheckRadius*mCheckRadius;
			foreach(Collider2D c in listOfNearestMines)
			{

				Vector3 direction = c.transform.position - transform.position;
				float distanceSquare = direction.sqrMagnitude;
				if(distanceSquare< minDist)
				{
					minDist = distanceSquare;
					mNearestMineDir = transform.position - c.transform.position;
				}
			}

			return mNearestMineDir;
		}

			

		void OnTriggerEnter2D(Collider2D obj)
		{

			if(obj.gameObject.tag.Equals("Mines"))
			{
					_fitnessValue += 200;
					Destroy(obj.gameObject);
					GameObject mObj = Instantiate(mineObj,new Vector2(Random.Range(-SSMaxX,SSMaxX),Random.Range(-SSMaxY,SSMaxY)),Quaternion.identity) as GameObject;
					mObj.transform.parent = MineData.transform;	
			}
		}


	}
}