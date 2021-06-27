using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecommendedMealNutritionIntake
{
    float[] defaultMealProportion = new float[3] { 0.25f, 0.5f, 0.25f };
    float[] defaultMacroPorportion = new float[3] { 0.25f, 0.5f, 0.25f };
    int[] macroId = new int[6] { 0, 1, 2, 3, 4, 6 };

    public NutritionContent calculateRecommendedDailyNutritionIntake(float al)
    {
        NutritionContent nc = basicDailyNeed();

        nc.MultiplyObject(dailyToMultiply());

        for(int i=0;i<6;i++)   nc.MutiplyFloat(((NutritionNames)macroId[i]), al);

        Debug.Log("------------------------------");
        Debug.Log("Recommended daily intake:");
        nc.printObject();

        return nc;
    }

    public NutritionContent calculateRecommendedMealNutritionIntake(float al, int meal)
    {
        NutritionContent nc = calculateRecommendedDailyNutritionIntake(al);

        bool hasMeal = PlayerPrefs.HasKey("Meal-0");

        if (hasMeal)
        {
            nc.MutiplyFloat((float)PlayerPrefs.GetInt("Meal-" + meal.ToString()) / 100.0f);
        }
        else
        {
            nc.MutiplyFloat(defaultMealProportion[meal]);
        }

        Debug.Log("------------------------------");
        Debug.Log("Recommended Meal Intake:");
        nc.printObject();

        return nc;
    }

    NutritionContent basicDailyNeed()
    {
        NutritionContent nc = new NutritionContent();

        if(!(PlayerPrefs.HasKey("Gender") && PlayerPrefs.HasKey("YearOfBirth")
            && PlayerPrefs.HasKey("Height") && PlayerPrefs.HasKey("Weight")))
        {
            return nc;
        }

        int g = PlayerPrefs.GetInt("Gender");
        int yearOfBirth = PlayerPrefs.GetInt("YearOfBirth");
        int yearNow = DateTime.Now.Year;
        int a = yearNow - yearOfBirth;

        float h = PlayerPrefs.GetFloat("Height");
        float w = PlayerPrefs.GetFloat("Weight");

        float BMR;
        if (g == 1)
        {
            BMR = 10f * w + 6.25f * h - 5f * a + 5f;
        }
        else
        {
            BMR = 10f * w + 6.25f * h - 5f * a - 161f;
        }

        nc.AddAmount(NutritionNames.Calories, BMR);
        nc.AddAmount(NutritionNames.SaturatedFat, BMR * 0.1f/9f);
        nc.AddAmount(NutritionNames.Sugar, BMR * 0.1f/4f);

        new NeededNutritionDAO().readNeededNutrition(ref nc,a,g);

        Debug.Log("------------------------------");
        Debug.Log("Basic Daily Need");
        nc.printObject();

        return nc;
    }

    NutritionContent dailyToMultiply()
    {
        NutritionContent nc = new NutritionContent();

        for(int i = 0; i < 7; i++)
        {
            int num = PlayerPrefs.GetInt("AddMinus-" + i.ToString());
            float ft;
            if (i >= 1 && i <= 3)
            {
                if (num != 0) {

                    ft = (float)num / 100.0f;
                    if (i <= 2)
                    {
                        ft = ft / 4f;
                    }
                    else
                    {
                        ft = ft / 9f;
                    }
                }
                else
                {
                    ft = defaultMacroPorportion[i - 1];
                }
            }
            else
            {
                ft = (float)(100 + num) / 100.0f;
            }
            nc.AddAmount((NutritionNames)i, ft);

        }

        for(int i=7;i<24;i++) nc.AddAmount((NutritionNames)i, 1f);

        for (int i = 0; i < 5; i++)
        {
            int num = PlayerPrefs.GetInt("AddMinus-" + (i+7).ToString());
            float ft = (float)(100 + num) / 100.0f;

            string nu_name_str = PlayerPrefs.GetString("PersonalisedNutrition-" + i.ToString());
            if ((!nu_name_str.Equals("None"))
                && (PlayerPrefs.HasKey("PersonalisedNutrition-" + i.ToString())))
            {
                NutritionNames nu_name = (NutritionNames)System.Enum.Parse(typeof(NutritionNames), nu_name_str);

                nc.AddAmount(nu_name, -1f);
                nc.AddAmount(nu_name, ft);
            }
        }

        Debug.Log("------------------------------");
        Debug.Log("dailyToMultiply:");
        nc.printObject();

        return nc;
    }
}
