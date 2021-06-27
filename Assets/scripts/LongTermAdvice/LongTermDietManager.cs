using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LongTermDietManager : MonoBehaviour
{
    [SerializeField]
    InputField activityLevelInputField;

    [SerializeField]
    Dropdown activityLevelDropdown;

    MealHistoryDAO mealDAO;
    ReportMgr reportMgr;
    ReadFoodNutrientDAO readFoodNutrientDAO;

    public NutritionContent totalNutritionContent;
    public NutritionContent recommendMealNutritionIntake;

    float activityLevel = 0;
    float[] activityLevelList = new float[4] { 1.5f, 1.7f, 2.2f, 2.4f };

    float[] defaultMealProportion = new float[3] { 0.25f, 0.5f, 0.25f };

    void Start()
    {
        reportMgr = Camera.main.GetComponent<ReportMgr>();

        mealDAO = new MealHistoryDAO();
        readFoodNutrientDAO = new ReadFoodNutrientDAO();

    }


    //----Activity Level Handling----

    public void OnUserTypeAL()
    {
        string al_str = activityLevelInputField.text;

        if (al_str.Length == 0)
        {
            activityLevel = 0;
            return;
        }

        activityLevel = float.Parse(al_str);

        Debug.Log("set AL = " + activityLevel.ToString());
    }

    public void onUserSelectAL()
    {
        activityLevel = activityLevelList[activityLevelDropdown.value];

        activityLevelInputField.text = activityLevel.ToString();
        Debug.Log("set AL = " + activityLevel.ToString());
    }

    //----Header Buttons Handling----
    public void OnClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickReport()
    {
        //prepare 2 nc
        GetTotalNutritionContent();

        recommendMealNutritionIntake = new RecommendedMealNutritionIntake()
                                        .calculateRecommendedDailyNutritionIntake(activityLevel);

        //instantiate report object
        reportMgr.instantiateObject(totalNutritionContent, recommendMealNutritionIntake);

    }



    void GetTotalNutritionContent()
    {
        totalNutritionContent = new NutritionContent();

        NutritionContent totalDayNC = new NutritionContent();

        bool hasMeal = PlayerPrefs.HasKey("Meal-0");

        
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("------ i = " + i.ToString() + "------");

            List<List<FoodPair>>  mealRecords = mealDAO.readFoodsOfMeal(i.ToString());


            Debug.Log("# of meal records: " + mealRecords.Count.ToString());

            if (mealRecords.Count == 0) return;

            NutritionContent totalMealNC = new NutritionContent();

            foreach (List<FoodPair> oneMeal in mealRecords)
            {
                for(int j = 0; j < oneMeal.Count; j++)
                {
                    NutritionContent food_nc = readFoodNutrientDAO.readToNutritionContent(oneMeal[j].ID);
                    food_nc.MutiplyFloat(((float)oneMeal[j].Amount) / 100f);

                    totalMealNC.AddObject(food_nc);
                }
            }

            totalMealNC.MutiplyFloat(1f / (float)mealRecords.Count);

            totalDayNC.AddObject(totalMealNC);
        }


        totalNutritionContent = totalDayNC;

    }

}
