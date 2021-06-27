using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectFoodManager : MonoBehaviour
{
    [SerializeField]
    Button[] daysButton;

    [SerializeField]
    Button[] mealsButton;

    [SerializeField]
    InputField[] foodInputFields;

    [SerializeField]
    InputField[] amountInputFields;

    [SerializeField]
    Dropdown[] dropdowns;



    MealHistoryDAO mealDAO;
    SearchFoodName searchFoodName;

    int day;   //2,1,0
    int meal;   //0,1,2,3

    int[] foodID;
    string[] foodName;
    int[] amount;

    Color normalTint = Color.white;
    Color selectedTint = new Color(87,163,245);

    List<FoodPair> currentFoods;
    int currentFoodsRealSize;

    List<List<FoodPair>> relatedFoods;

    List<string> firstblank;

    bool edited;

    // Start is called before the first frame update
    void Start()
    {
        day = 0;
        meal = 0;

        mealDAO = new MealHistoryDAO();
        searchFoodName = new SearchFoodName();

        firstblank = new List<string>();
        firstblank.Add("-------");

        edited = false;

        selectDay(0);
        selectMeal(0);
    }

    void saveUpdated()
    {
        DateTime date = DateTime.Today.AddDays(0 - day);
        string date_str = date.ToString("yyyy-MM-dd");
        string meal_str = meal.ToString();

        mealDAO.writeFoodsOfDayMeal(date_str, meal_str, currentFoods, currentFoodsRealSize);

    }

    public void selectDay(int day_num)
    {
        if (edited) saveUpdated();

        day = day_num;

        daysButton[0].GetComponent<Image>().color = Color.white;
        daysButton[1].GetComponent<Image>().color = Color.white;
        daysButton[2].GetComponent<Image>().color = Color.white;

        daysButton[day_num].GetComponent<Image>().color = Color.yellow;

        updateBelows();

    }

    public void selectMeal(int meal_num)
    {
        if (edited) saveUpdated();

        meal = meal_num;

        mealsButton[0].GetComponent<Image>().color = Color.white;
        mealsButton[1].GetComponent<Image>().color = Color.white;
        mealsButton[2].GetComponent<Image>().color = Color.white;
        mealsButton[3].GetComponent<Image>().color = Color.white;

        mealsButton[meal_num].GetComponent<Image>().color = Color.yellow;

        updateBelows();
    }

    void updateBelows()
    {
        DateTime date = DateTime.Today.AddDays(0 - day);
        string date_str = date.ToString("yyyy-MM-dd");
        string meal_str = meal.ToString();

        //currentFoods = new List<FoodPair>();

        relatedFoods = new List<List<FoodPair>>();
        for (int i = 0; i < 5; i++)
        {
            relatedFoods.Add(new List<FoodPair>());

            //foodTexts[i].text = "set text " + i.ToString();
        }

        currentFoods = mealDAO.readFoodsOfDayMeal(date_str, meal_str);
        currentFoodsRealSize = currentFoods.Count;

        Debug.Log("After reading, Current foods real size = " + currentFoodsRealSize.ToString());

        for (int i=0;i< currentFoodsRealSize; i++)
        {
            foodInputFields[i].text = currentFoods[i].Name;
            amountInputFields[i].text = currentFoods[i].Amount.ToString();

        }

        for (int i = currentFoodsRealSize; i < 5; i++)
        {
            foodInputFields[i].text = "";
            amountInputFields[i].text = "";

        }

        for (int i = currentFoodsRealSize; i < 5; i++)
        {
            currentFoods.Add(new FoodPair(-1, null));
        }

        for(int i = 0; i < 5; i++)
        {
            dropdowns[i].ClearOptions();
        }

    }


    public void onUserTypeFood(int list_id)
    {
        edited = true;
        if (list_id > currentFoodsRealSize)
        {
            foodInputFields[list_id].text = "";
            return;
        }

        string user_type = foodInputFields[list_id].text;

        

        if (user_type.Length < 3) return;


        //search related foods
        print(user_type);

        relatedFoods[list_id] = searchFoodName.search(user_type);

        print(relatedFoods[list_id].Count);

        List<string> relatedFoodNames = new List<string>();

        for(int i=0; i<relatedFoods[list_id].Count;i++)
        {
            relatedFoodNames.Add(relatedFoods[list_id][i].Name);
        }

        print(relatedFoodNames.Count);


        //call dropdown list
        dropdowns[list_id].ClearOptions();
        dropdowns[list_id].AddOptions(firstblank);

        dropdowns[list_id].AddOptions(relatedFoodNames);
        
        dropdowns[list_id].Show();
    }

    public void onUserTypeAmount(int list_id)
    {
        edited = true;
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

    public void onUserSelect(int list_id)
    {
        edited = true;
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


    public void onClickConfirm()
    {
        //write from currentFoods to file
        saveUpdated();
        //back to main scene
        SceneManager.LoadScene("MainMenu");
    }

    public void onClickClear()
    {
        mealDAO.deleteAll();
    }

    
}
