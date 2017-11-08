using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork : MonoBehaviour {


	public struct sNeuron
	{
		public int mNumOfInputs;
		public	List<float> mWeights;
	}

	public struct sNeuronLayer
	{
		public int mNumOfNeurons;
		public List<sNeuron> mNeurons;
	}


	int mNumberOfInputs;
	int mNumberOfOutputs;
	int mNumberOfHiddenLayer;
	int mNeuronsPerHiddenLayer;

	List<sNeuronLayer> mNeuronLayers;


	// Use this for initialization
	void Start ()
	{
		mNumberOfHiddenLayer = 1;
		mNeuronsPerHiddenLayer = 6;
		mNumberOfInputs = 4;
		mNumberOfOutputs = 2;

		mNeuronLayers = new List<sNeuronLayer>();
		CreateNeuralNetwork();
	
	}

	void CreateNeuralNetwork()
	{
		if(mNumberOfHiddenLayer>0)
		{

			//create Input layer
			mNeuronLayers.Add(CreateLayers(mNeuronsPerHiddenLayer,mNumberOfInputs));

			//create hidden layer
			for(int i=0;i<mNumberOfHiddenLayer;i++)
			{
				mNeuronLayers.Add(CreateLayers(mNeuronsPerHiddenLayer,mNeuronsPerHiddenLayer));
			}

			//create output layer
			mNeuronLayers.Add(CreateLayers(mNumberOfOutputs,mNeuronsPerHiddenLayer));

		}
	}

	sNeuronLayer CreateLayers(int iNumOfNeuronsPerLayer, int iNumOfInputPerNeuron)
	{

			sNeuronLayer nlayer;
			nlayer.mNumOfNeurons = iNumOfNeuronsPerLayer;
			nlayer.mNeurons = new List<sNeuron>();
			for(int j=0;j<iNumOfNeuronsPerLayer;j++)
			{
				sNeuron neuron;
				neuron.mNumOfInputs = iNumOfInputPerNeuron+1; //+1 for bias
				neuron.mWeights = new List<float>();
				for(int k=0;k<iNumOfInputPerNeuron+1;k++)
				{
					neuron.mWeights.Add(Random.Range(-1f,1f)); //Add randomweight
				}
				nlayer.mNeurons.Add(neuron);
			}
			return nlayer;
	}
	
	// Update is called once per frame
	public 	List<float> AssignInput (List<float> iNeuronInputs) 
	{
		List<float> output = new List<float>();
		int weight=0;


		if(iNeuronInputs.Count != mNumberOfInputs)
		{
			print("Input is wrong");
			return output;
		}

		for(int i=0;i<mNumberOfHiddenLayer;i++)
		{
			//skipping the input layer
			if(i>0)
			{
				iNeuronInputs = output;
			}
			output.Clear();
			weight =0;
			for(int j=0;j<mNeuronLayers[i].mNumOfNeurons;j++)
			{
				float totalInput =0;
				int numOfInputs = mNeuronLayers[i].mNeurons[j].mNumOfInputs;
				for(int k=0;k<numOfInputs-1;k++)
				{
					totalInput +=mNeuronLayers[i].mNeurons[j].mWeights[k] * iNeuronInputs[weight++];
				
				}
				//add bias
				totalInput +=mNeuronLayers[i].mNeurons[j].mWeights[numOfInputs-1] * -1;

				output.Add(Sigmoid(totalInput));
				weight =0;
			}

		}

		return output;

	}

	public int GetTotalNumberOfWeights()
	{
		int weights=0;
		for(int i=0;i<mNumberOfHiddenLayer;i++)
		{
			for(int j=0;j<mNeuronLayers[i].mNumOfNeurons;j++)
			{
				for(int k=0;k<mNeuronLayers[i].mNeurons[j].mNumOfInputs ;k++)
				{
					weights++;
				}

			}
		}
		return weights;
	}

	void GetWeights(){}

	public void PutWeights(List<float> iWeights)
	{
		int weights =0;
		for(int i=0;i<mNumberOfHiddenLayer;i++)
		{
			for(int j=0;j<mNeuronLayers[i].mNumOfNeurons;j++)
			{
				for(int k=0;k<mNeuronLayers[i].mNeurons[j].mNumOfInputs ;k++)
				{
					mNeuronLayers[i].mNeurons[j].mWeights[k] = iWeights[weights++];
				}
				
			}
		}

	}
	float Sigmoid(float netInput)
	{
	
		return (1/(1+Mathf.Exp(-netInput/1)));
	}

}
