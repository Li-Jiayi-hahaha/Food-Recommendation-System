using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadPlanManager : MonoBehaviour
{
    

    [SerializeField]
    Button[] mealsButton;

    [SerializeField]
    InputField[] foodInputFields;

    [SerializeField]
    InputField[] amountInputFields;

    [SerializeField]
    Dropdown[] dropdowns;

    [SerializeField]
    InputField activityLevelInputField;

    [SerializeField]
    Dropdown activityLevelDropdown;


    MealHistoryDAO mealDAO;
    SearchFoodName searchFoodName;
    ReadFoodNutrientDAO readFoodNutrientDAO;
    RecommendedMealNutritionIntake calRecommendedMealNutritionIntake;
    ReportMgr reportMgr;

    NutritionContent totalNutritionContent;
    NutritionContent recommendMealNutritionIntake;

    int meal;  // 0,1,2

    int[] foodID;
    string[] foodName;
    int[] amount;

    Color normalTint = Color.white;
    Color selectedTint = new Color(87, 163, 245);

    List<FoodPair> currentFoods;
    int currentFoodsRealSize;

    List<List<FoodPair>> relatedFoods;

    List<string> firstblank;

    float activityLevel;
    float[] activityLevelList = new float[4] { 1.5f, 1.7f, 2.2f, 2.4f };


// Start is called before the first frame update
    void Start()
    {
        mealDAO = new MealHistoryDAO();
        searchFoodName = new SearchFoodName();
        readFoodNutrientDAO = new ReadFoodNutrientDAO();
        calRecommendedMealNutritionIntake = new RecommendedMealNutritionIntake();
        reportMgr = Camera.main.GetComponent<ReportMgr>();

        firstblank = new List<string>();
        firstblank.Add("-------");

        refreshPage();

        meal = 0;
        selectMeal(0);
    }

    void refreshPage()
    {
        relatedFoods = new List<List<FoodPair>>();
        currentFoods = new List<FoodPair>();
        currentFoodsRealSize = 0;
        for (int i = 0; i < 5; i++)
        {
            relatedFoods.Add(new List<FoodPair>());
            currentFoods.Add(new FoodPair(-1, null));

            foodInputFields[i].text = "";
            amountInputFields[i].text = "";
            dropdowns[i].ClearOptions();
        }

        activityLevel = 0;

    }

    public void selectMeal(int meal_num)
    {
        meal = meal_num;

        mealsButton[0].GetComponent<Image>().color = Color.white;
        mealsButton[1].GetComponent<Image>().color = Color.white;
        mealsButton[2].GetComponent<Image>().color = Color.white;
        
        mealsButton[meal_num].GetComponent<Image>().color = Color.yellow;
    }

    public void onUserTypeFood(int list_id)
    {
        
        if (list_id > currentFoodsRealSize)
        {
            foodInputFields[list_id].text = "";
            return;
        }

        string user_type = foodInputFields[list_id].text;



        if (user_type.Length < 3) return;


        //search related foods
        

        relatedFoods[list_id] = searchFoodName.search(user_type);

        

        List<string> relatedFoodNames = new List<string>();

        for (int i = 0; i < relatedFoods[list_id].Count; i++)
        {
            relatedFoodNames.Add(relatedFoods[list_id][i].Name);
        }

        


        //call dropdown list
        dropdowns[list_id].ClearOptions();
        dropdowns[list_id].AddOptions(firstblank);

        dropdowns[list_id].AddOptions(relatedFoodNames);

        dropdowns[list_id].Show();
    }


    public void onUserTypeAmount(int list_id)
    {
        
        //check if list_id valid
        if (list_id >= currentFoodsRealSize)
        {
            amountInputFields[list_id].text = "";
            return;
        }

        string amount_str = amountInputFields[list_id].text;
        Debug.Log("Amount = " + amount_str);

        if (amount_str.Length == 0)
        {
            currentFoods[list_id].Amount = 0;
            return;
        }

        //change curentFoods

        currentFoods[list_id].Amount = int.Parse(amount_str);

    }

    public void onUserSelectFood(int list_id)
    {
        
        int selection_id = dropdowns[list_id].value;

        if (selection_id == 0) return;

        //change currentFoods
        selection_id -= 1;

        currentFoods[list_id].Name = relatedFoods[list_id][selection_id].Name;
        currentFoods[list_id].ID = relatedFoods[list_id][selection_id].ID;

        Debug.Log("set text: " + currentFoods[list_id].Name);
        //foodTexts[list_id].text = currentFoods[list_id].Name;

        foodInputFields[list_id].text = currentFoods[list_id].Name;

        if (list_id == currentFoodsRealSize)
        {
            currentFoodsRealSize += 1;
        }
        Debug.Log("Current foods real size = " + currentFoodsRealSize.ToString());
    }

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


    public void onClickNext()
    {
        
        totalNutritionContent = new NutritionContent();
        for(int i = 0; i < currentFoodsRealSize; i++)
        {
            NutritionContent single_nc = readFoodNutrientDAO.readToNutritionContent(currentFoods[i].ID);

            single_nc.MutiplyFloat(((float)currentFoods[i].Amount) / 100f);

            totalNutritionContent.AddObject(single_nc);

        }

        Debug.Log("Nutrition Content in plan");
        totalNutritionContent.printObject();

        //calculate recemmended meal nutrition intake
        recommendMealNutritionIntake = calRecommendedMealNutritionIntake.calculateRecommendedMealNutritionIntake(activityLevel, meal);

        //Instantiate Prefab

        reportMgr.instantiateObject(totalNutritionContent,recommendMealNutritionIntake);
    }

    public void onClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    

}
