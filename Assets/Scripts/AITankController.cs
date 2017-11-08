using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AISandbox 
{
	public class AITankController : MonoBehaviour 
	{

		IActor mTankMovement;
		TankFitness mFitness;
		public NeuralNetwork mTankBrain;
		Vector2 mLookAt;
		float leftTrack;
		float rightTrack;
		public List<float> neuronOutput = new List<float>();
		void Awake()
		{
			mTankMovement =	GetComponent<IActor>();
			mFitness = GetComponent<TankFitness>();
			mTankBrain = GetComponent<NeuralNetwork>();
			mLookAt = Vector2.zero;
				
		}

		public void UpdatePos () 
		{

			Vector3 nearestMine =	mFitness.GetNearestMine();
			nearestMine.Normalize();
			List<float> neuronInput = new List<float>();
			neuronInput.Add(nearestMine.x);
			neuronInput.Add(nearestMine.y);

			mLookAt = transform.up;
			neuronInput.Add(mLookAt.x);
			neuronInput.Add(mLookAt.y);

		

			neuronOutput = mTankBrain.AssignInput(neuronInput);
			leftTrack = neuronOutput[0];
			rightTrack = neuronOutput[1];

			Debug.DrawRay(transform.position,transform.up* 5,Color.green);

			mTankMovement.SetInput(leftTrack,rightTrack);


		}

		void GetClosestMine(){}
		void IncreaseFitness(){}
		void Fitness(){}
		public void PutWeights(List<float> iWeights)
		{
			mTankBrain.PutWeights(iWeights);
		}

		public int GetNumberOfWeights()
		{
			return mTankBrain.GetTotalNumberOfWeights();
		}

		void CheckForMine(){}

		void Reset(){}



	 }
}
