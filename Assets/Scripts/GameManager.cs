using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace AISandbox {
public class GameManager : MonoBehaviour {


	public Text generationText;
	public Text timeLeftText;
	public Text averageText;
	public Text oldAverageText;

	[SerializeField]
	Transform tankSpawnPoint;


	public List<GameObject> tanksList;
	[SerializeField]
	int minesCount ; 

	[SerializeField]
	int tanksCount ; 

	public int GameLoopTimer;
	
	public int rulesPerSecond;
	
	int mGeneration = 1;
	float newTime ;
	float gameTime;
	float resetCounter =0;

	bool newPopulationGenerated;

	public float averageScore;
	public float totalFitness;


	public GeneticAlgorithm geneticAlgoObj;

	private static GameManager instance;

	List<Genome> tankChromo;

	List<AITankController> mTankController;
	public static GameManager Instance
	{
			get{
				return instance;
			}
	}


	void Awake()
	{
			instance = this;
			averageScore =0;
			totalFitness =0;
			GameLoopTimer = 30;
			mTankController = new List<AITankController>();
			tankChromo = new List<Genome>();
	}

	void Start () 
	{
			for(int i=0;i<tanksList.Count;i++)
			{
				mTankController.Add(tanksList[i].GetComponent<AITankController>());
			}


			int weightCount = tanksList[0].GetComponent<AITankController>().GetNumberOfWeights();
			geneticAlgoObj.Initialize(weightCount,tanksList.Count);
			tankChromo = geneticAlgoObj.GetChromosomes();

			for(int i=0;i<tanksList.Count;i++)
			{
				mTankController[i].PutWeights(tankChromo[i].weights);
			}

	}

	// Update is called once per frame
	void Update () 
	{
		gameTime = Time.time - resetCounter;
		if(gameTime >= GameLoopTimer && !newPopulationGenerated)
		{

			//geneticAlgoObj.ClearPopulation();
			tankChromo	 = geneticAlgoObj.GenerateNewPopulation(tankChromo);
			for(int i=0;i<tanksList.Count;i++)
			{
				mTankController[i].PutWeights(tankChromo[i].weights);
			}

			
			mGeneration++;
			ResetTimer();
			ResetTankData();
			return;
		}
		else
		{
				for(int i=0;i<mTankController.Count;i++)
				{
					mTankController[i].UpdatePos();
					Genome a;
					a.weights = tankChromo[i].weights;
					a.fitness = tanksList[i].GetComponent<TankFitness>().TankFitnessValue;
					tankChromo[i] = a;
				}



		}

		newPopulationGenerated = false;
		newTime =(int) gameTime;
		CalculateAverage();
		UpdateUI();
	}

	void CalculateAverage()
	{
			totalFitness =0;
			for(int i=0;i<tanksList.Count;i++)
			{
				totalFitness += tankChromo[i].fitness;
			}
			averageScore = totalFitness /tanksList.Count;
			averageText.text = "Average Score:"+averageScore.ToString();

	}

	void ResetTimer()
	{
		resetCounter = GameLoopTimer * (mGeneration-1);
	}

		void ResetTankData()
	{

			for(int i=0;i<tanksList.Count;i++)
			{

				tanksList[i].GetComponent<TankFitness>().TankFitnessValue = 0;
			}


			oldAverageText.text = "Old Average:"+averageScore.ToString();
			totalFitness =0;
			averageScore = 0;
	}


	void UpdateUI()
	{
		generationText.text= "Generation:"+mGeneration;
		timeLeftText.text= "Time Left:"+(GameLoopTimer - newTime);
	}
  }
}
