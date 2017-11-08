using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISandbox 
{
	public class GeneticAlgorithm : MonoBehaviour 
	{

	
		
		List<Genome> oldPopulation;
		List<Genome> newPopulation;
		public	List<Genome> mGenomes;
		
		
		public int indexValue ;
		
		float elitismPercent= 10f;
		float mutateProbability = 0.1f;
		int eliteCount;
		int mNumOfweights;

		void Awake()
		{
	
		}
		void Start () 
		{
			
			oldPopulation = new List<Genome>();
			newPopulation = new List<Genome>();
			mGenomes = new List<Genome>();
			
		}
		public void Initialize(int iNumOfWeights, int iPopCount)
		{

			for(int i=0;i<iPopCount;i++)
			{
				Genome g;
				g.fitness = 0;
				g.weights = new List<float>();
				mGenomes.Add(g);
				for(int j=0;j<iNumOfWeights;j++)
				{
					mGenomes[i].weights.Add(UnityEngine.Random.Range(-1f,1f));
				}
			}

			mNumOfweights = iNumOfWeights;
		}
		
		public List<Genome>  GenerateNewPopulation(List<Genome> iOldPopulation )
		{
			ClearPopulation();
			oldPopulation.AddRange(iOldPopulation);
			
			//sort according to fitness
			FitnessComparer fintessComp = new FitnessComparer();
			oldPopulation.Sort(fintessComp);

		
			
			Elitism();
			SelectionChrom();
			Replace();

			return mGenomes;

		}
		
		public void ClearPopulation()
		{
			newPopulation.Clear();
			oldPopulation.Clear();
		}
		
		void Elitism()
		{
			eliteCount = (int)elitismPercent * oldPopulation.Count / 100;
			
			for(int j=0;j< eliteCount;j++)
			{
				newPopulation.Add(oldPopulation[j]);
				oldPopulation.Remove(oldPopulation[j]);
			}
			
		}
		
		void SelectionChrom()
		{

			for(int i=0;i<oldPopulation.Count;i+=2)
			{
				Genome parent1 = RouletteWheel();

				Genome parent2 = RouletteWheel();
				CrossOver(parent1.weights,parent2.weights);

			}
			
		}
		
		void CrossOver(List<float> p1,List<float> p2)
		{

				List<float> childRule1 = new List<float>();
				List<float> childRule2 = new List<float>();

				int crossPoint = Random.Range(0,mNumOfweights - eliteCount);

				for(int j=0;j<crossPoint;j++)
				{
					childRule1.Add(p1[j]);
					childRule2.Add(p2[j]);
				}

				for(int j=crossPoint;j<p1.Count;j++)
				{
					childRule1.Add(p2[j]);
					childRule2.Add(p1[j]);
				}

				
				Mutate(childRule1);
				Mutate(childRule2);
			
			
		}

		void Mutate(List<float> c1)
		{
			
				for(int i=0;i<c1.Count;i++)
				{
					float prob = Random.Range(0.0f,1.0f);
					if(prob < mutateProbability)
					{
						c1[i] +=  Random.Range(-0.2f,0.2f); // small value change

					}
				}

			Genome g;
			g.weights = c1;
			g.fitness = 0;
			newPopulation.Add(g);
		}

		public List<Genome> GetChromosomes()
		{
			return mGenomes;
		}

		Genome RouletteWheel()
		{
			float totalFitness=0;
			for(int i=0;i<oldPopulation.Count;i++)
			{
				totalFitness += oldPopulation[i].fitness;
			}

			float randomNumber = UnityEngine.Random.Range(0,totalFitness);
			Genome chosen;
			chosen.fitness =0;
			chosen.weights = new List<float>();
			float fitnessSofar=0;
			for(int i=0;i<oldPopulation.Count;i++)
			{

				fitnessSofar +=oldPopulation[i].fitness;
				if(fitnessSofar >= randomNumber)
				{
					chosen = oldPopulation[i];
					break;
				}
			}

			return chosen;
		}


		void Replace()
		{
			mGenomes.Clear();
			mGenomes.AddRange(newPopulation);
			
		}
		
		
	}
}