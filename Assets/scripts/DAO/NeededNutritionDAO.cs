using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NeededNutritionDAO
{
    const string fileName = "Human_Daily_Nutrition_Requirements.csv";
    static int[] ageList = new int[6] { 6, 11, 16, 25, 40, 85 };

    public void readNeededNutrition(ref NutritionContent nc, int age, int gender)
    {
        int col_a = 0;
        int col_b = 0;

        int age_a = 0;
        int age_b = 0;

        FindUsefulColumns(ref col_a, ref col_b, ref age_a, ref age_b, age, gender);

        StreamReader csvFile = null;
        try
        {

            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));

            bool endOfFile = false;

            string aLine = csvFile.ReadLine();
            aLine = csvFile.ReadLine();
            aLine = csvFile.ReadLine();

            while (!endOfFile)
            {
                aLine = csvFile.ReadLine();

                if (aLine == null)
                {
                    endOfFile = true;
                    break;
                }
                string[] values = aLine.Split(',');

                if (System.Enum.IsDefined(typeof(NutritionNames), values[0]))
                {
                    NutritionNames nu_name = (NutritionNames)System.Enum.Parse(typeof(NutritionNames), values[0]);
                    float ft;
                    if (col_a == col_b)
                    {
                        ft = float.Parse(values[col_a]);
                    }

                    else
                    {
                        float ft_a = float.Parse(values[col_a]);
                        float ft_b = float.Parse(values[col_b]);

                        int diff = age_b - age_a;

                        float pa = ((float)(age_b - age)) / ((float)diff) * ft_a;
                        float pb = ((float)(age - age_a)) / ((float)diff) * ft_b;

                        ft = pa + pb;
                    }

                    nc.AddAmount(nu_name, ft);

                }
                

            }
            //Debug.Log("reach 2");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            if (csvFile != null)
                csvFile.Close();
        }
    }

    void FindUsefulColumns(ref int col_a, ref int col_b, ref int age_a, ref int age_b, int age, int gender)
    {
        int age_aid = 0;
        int age_bid = 0;

        if (age <= 6)
        {
            age_aid = 0;
            age_bid = 0;
        }
        if (age >= 85)
        {
            age_aid = 5;
            age_bid = 5;
        }

        if (age > 6 && age < 85)
        {
            int i;

            for (i = 0; ageList[i] < age; i++) continue;

            age_aid = i-1;

            age_bid = i;

            if (ageList[age_bid] == age) age_aid = age_bid;

        }

        age_a = ageList[age_aid];
        age_b = ageList[age_bid];

        Debug.Log("Age: a="+ age_a.ToString()+ ", b=" + age_b.ToString());

        col_a = age_aid * 2 + 1;
        col_b = age_bid * 2 + 1;

        if (gender == 1)
        {
            col_a += 1;
            col_b += 1;
        }
    }

}
