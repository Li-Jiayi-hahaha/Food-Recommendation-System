using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutritionContent
{
    public Dictionary<NutritionNames, float> nameValuePairs { get; }

    public NutritionContent()
    {
        nameValuePairs = new Dictionary<NutritionNames, float>();

        for (int i = 0; i < 24; i++)
        {
            nameValuePairs.Add((NutritionNames)i, 0.0f);
        }
    }

    public void AddAmount(NutritionNames nu_name, float amount)
    {
        nameValuePairs[nu_name] += amount;
    }

    public void AddObject(NutritionContent nc)
    {
        for (int i = 0; i < 24; i++)
        {
            NutritionNames nu_name = (NutritionNames)i;

            nameValuePairs[nu_name] += nc.nameValuePairs[nu_name];

        }
    }

    public void MutiplyFloat(float mp)
    {
        for (int i = 0; i < 24; i++)
        {
            NutritionNames nu_name = (NutritionNames)i;

            nameValuePairs[nu_name] *= mp;

        }
    }

    public void MutiplyFloat(NutritionNames nu_name, float mp)
    {
        nameValuePairs[nu_name] *= mp;
    }

    public void MultiplyObject(NutritionContent nc)
    {
        for (int i = 0; i < 24; i++)
        {
            NutritionNames nu_name = (NutritionNames)i;

            if ((i < 1) || (i > 3))
            {
                nameValuePairs[nu_name] *= nc.nameValuePairs[nu_name];
            }

            else
            {
                nameValuePairs[nu_name] = nameValuePairs[NutritionNames.Calories] * nc.nameValuePairs[nu_name];
            }
        }
    }

    public void printObject()
    {
        foreach (KeyValuePair<NutritionNames, float> kvp in nameValuePairs)
        { 
           Debug.Log("Key = " + kvp.Key.ToString()+", Value = " + kvp.Value.ToString());
        }
    }
}
