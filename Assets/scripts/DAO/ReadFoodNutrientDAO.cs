using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadFoodNutrientDAO
{
    const string fileName = "Food_Nutrient_Content.csv";

    int[] nutEleInCsv = new int[24] { 3,  5,  6,  4,  7,  8,
                                         9,  19, 14, 17, 30, 15,
                                        10, 11, 13, 20, 18, 12,
                                         23, 24, 25, 26, 27, 16};

    public List<float> readNutrition(int food_ID, int page, int size)
    {
        List<float> amountData = new List<float>();

        StreamReader csvFile = null;
        try
        {
            //Debug.Log("reach 0");
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));
            //Debug.Log("reach 1");
            bool endOfFile = false;

            string aLine = csvFile.ReadLine();

            while (!endOfFile)
            {
                aLine = csvFile.ReadLine();

                if (aLine == null)
                {
                    Debug.Log("Food ID not found.");
                    endOfFile = true;
                    break;
                }
                
                if (aLine.Contains(food_ID.ToString()))
                {
                    string[] values = aLine.Split(',');

                    for (int i=0; i < size; i++)
                    {
                        amountData.Add(float.Parse(values[nutEleInCsv[page*6 + i]]));
                    }

                    Debug.Log(amountData.Count);

                    endOfFile = true;
                    break;
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

        return amountData;

    }

    public NutritionContent readToNutritionContent(int food_ID)
    {
        NutritionContent nutritionContent = new NutritionContent();

        StreamReader csvFile = null;
        try
        {
            
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));
            
            bool endOfFile = false;

            string aLine = csvFile.ReadLine();

            while (!endOfFile)
            {
                aLine = csvFile.ReadLine();

                if (aLine == null)
                {
                    Debug.Log("Food ID not found.");
                    endOfFile = true;
                    break;
                }

                if (aLine.Contains(food_ID.ToString()))
                {
                    string[] values = aLine.Split(',');

                    for (int i = 0; i < 24; i++)
                    {
                        nutritionContent.AddAmount((NutritionNames)i,
                                                    float.Parse(values[nutEleInCsv[i]]));
                    }


                    //Debug.Log(amountData.Count);

                    endOfFile = true;
                    break;
                }

            }
            
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


        return nutritionContent;
    }
}
