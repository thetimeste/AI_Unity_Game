using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAWalker //helper class that does not inherit from monobehaviour
{
    List<int> genesList;
    int dnaLength = 0;
    int maxValues = 0;

    public DNAWalker(int length,int value)
    {
        genesList = new List<int>();
        dnaLength = length;
        maxValues = value;
        SetRandomGenes();
    }

    public void SetRandomGenes()
    {
        genesList.Clear();
        for(int i = 0; i < dnaLength; i++)
        {
            genesList.Add(Random.Range(0, maxValues));
        }
    }

    public void SetGenesInt(int pos, int value)
    {
        genesList[pos] = value; //useful for hardcoding genes into genes list
    }

    public void MixDna (DNAWalker dna1,DNAWalker dna2) 
    {
        for(int i = 0; i < dnaLength; i++)
        {
            // it tankes half of the genes of dna1 and half of dna2
            if (i < dnaLength / 2)
            {
                genesList[i] = dna1.genesList[i];
            }
            else
            {
                genesList[i] = dna2.genesList[i];
            }
        }
    }

    public void Mutate()
    {
        genesList[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }

    public int GetGene(int pos)
    {
        return genesList[pos];
    }
}
