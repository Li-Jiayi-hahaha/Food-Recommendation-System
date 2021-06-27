using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SearchFoodName
{
    public List<FoodPair> search(string userInput)
    {
        List<FoodPair> searchedFoods = new List<FoodPair>();
        const string fileName = "Food_Nutrient_Content.csv";

        StreamReader csvFile = null;
        try
        {
            //Debug.Log("reach 0");
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));
            //Debug.Log("reach 1");
            bool endOfFile = false;
            csvFile.ReadLine();
            //Debug.Log("reach 2");
            int cnt = 0;

            while (! endOfFile)
            {
                string aLine = csvFile.ReadLine();
                if(aLine == null)
                {
                    endOfFile = true;
                    break;
                }

                string[] values = aLine.Split(',');

                int foodID = int.Parse(values[0]);
                string foodName = values[1];

                if(foodName.ToLower().Contains(userInput.ToLower()) )
                {
                    searchedFoods.Add(new FoodPair(foodID, foodName));
                }

                cnt += 1;

            }

            Debug.Log("line of the file: " + cnt);
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

        return searchedFoods;
    }
}
