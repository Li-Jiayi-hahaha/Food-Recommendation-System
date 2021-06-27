using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MealHistoryDAO
{
    const string fileName = "MealHistory.csv";

    public List<FoodPair> readFoodsOfDayMeal(string date, string meal)
    {
        List<FoodPair> searchedFoods = new List<FoodPair>();

        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.dataPath);

        StreamReader csvFile = null;
        try
        {
            //Debug.Log("reach 0");
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));
            //Debug.Log("reach 1");
            bool endOfFile = false;

            while (!endOfFile)
            {
                string aLine = csvFile.ReadLine();

                if(aLine == null)
                {
                    endOfFile = true;
                    break;
                }
                string[] values = aLine.Split(',');


                if ((values[0]==date)&&(values[1]==meal))
                {
                    searchedFoods.Add(new FoodPair(int.Parse(values[2]), values[3], int.Parse(values[4])));
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

        return searchedFoods;
    }

    public List<List<FoodPair>> readFoodsOfMeal(string meal)
    {
        List<List<FoodPair>> mealRecords = new List<List<FoodPair>>();

        List<FoodPair> oneMeal = new List<FoodPair>();
        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.dataPath);

        StreamReader csvFile = null;
        try
        {
            //Debug.Log("reach 0");
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));
            //Debug.Log("reach 1");
            bool endOfFile = false;

            string curent_date = "None";


            while (!endOfFile)
            {
                string aLine = csvFile.ReadLine();

                if (aLine == null)
                {
                    endOfFile = true;
                    break;
                }
                string[] values = aLine.Split(',');

                if ((values[0] != curent_date) && (values[1] == meal))
                {
                    Debug.Log("# of foods in one meal: " + oneMeal.Count);
                    mealRecords.Add(oneMeal);

                    oneMeal = new List<FoodPair>();
                    curent_date = values[0];
                }

                if ((values[0] == curent_date) && (values[1] == meal))
                {
                    oneMeal.Add(new FoodPair(int.Parse(values[2]), values[3], int.Parse(values[4])));
                }

            }

            Debug.Log("# of foods in one meal: " + oneMeal.Count);
            mealRecords.Add(oneMeal);
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

        if(mealRecords.Count!=1) mealRecords.RemoveAt(0);

        return mealRecords;
    }

    public void writeFoodsOfDayMeal(string date, string meal, List<FoodPair> foodList, int foodNum)
    {
        deleteOldDayMealRecord(date, meal);

        Debug.Log("Old records are deleted.");

        FileStream fs = null;
        StreamWriter csvFile = null;
        try
        {
            fs = new FileStream(Path.Combine(Application.streamingAssetsPath, fileName), FileMode.Append);

            csvFile = new StreamWriter(fs);


            for(int i=0; i<foodNum; i++)
            {
                csvFile.WriteLine(date +','+meal+','+foodList[i].ID.ToString()+','+
                    foodList[i].Name +',' + foodList[i].Amount.ToString());
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

        Debug.Log("New records are added.");

    }

   

    void deleteOldDayMealRecord(string date, string meal)
    {
        DateTime today = DateTime.Today;

        List<string> fileContent = new List<string>();

        StreamReader csvFile = null;
        try
        {
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));

            bool endOfFile = false;

            fileContent.Add(csvFile.ReadLine());

            while (!endOfFile)
            {
                string aLine = csvFile.ReadLine();

                if (aLine == null)
                {
                    endOfFile = true;
                    break;
                }
                string[] values = aLine.Split(',');


                DateTime dateRecord = Convert.ToDateTime(values[0]);

                TimeSpan span = today.Subtract(dateRecord);

                int dayDiff = span.Days;

                if ((dayDiff <= 31)&& ((values[0]!=date)||(values[1]!=meal))  ) 
                {
                    fileContent.Add(aLine);
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


        FileStream fs = null;
        StreamWriter newFile = null;
        try
        {
            fs = new FileStream(Path.Combine(Application.streamingAssetsPath, fileName), FileMode.Create);

            newFile = new StreamWriter(fs);

            for (int i = 0; i < fileContent.Count; i++)
            {
                newFile.WriteLine(fileContent[i]);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            if (newFile != null)
                newFile.Close();
        }


    }

    public void deleteAll()
    {
        DateTime today = DateTime.Today;

        string tableHead = "Date,Meal,Food_ID,Food_name,Amount,,";

        StreamReader csvFile = null;
        try
        {
            csvFile = File.OpenText(Path.Combine(Application.streamingAssetsPath, fileName));

            bool endOfFile = false;

            tableHead =  csvFile.ReadLine();
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


        FileStream fs = null;
        StreamWriter newFile = null;
        try
        {
            fs = new FileStream(Path.Combine(Application.streamingAssetsPath, fileName), FileMode.Create);

            newFile = new StreamWriter(fs);

            newFile.WriteLine(tableHead);

            
        }
        catch (System.Exception e)
        {
            Debug.Log(e.StackTrace);
        }
        finally
        {
            if (newFile != null)
                newFile.Close();
        }


    }
}
