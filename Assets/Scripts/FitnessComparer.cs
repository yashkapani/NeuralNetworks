using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISandbox 
{
	public class FitnessComparer : IComparer<Genome>
	{
		public int Compare(Genome other1,Genome other2)
		{
			float comp1 =	other1.fitness;
			float comp2 =	other2.fitness;
			if(comp1 < comp2) return 1;
			else if (comp1 > comp2) return -1;
			else return 0;

		}
	}
}
