using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnquireMealHistoryManager : MonoBehaviour
{
    [SerializeField]  Button[] daysButton;

    [SerializeField]  Button[] mealsButton;

    [SerializeField]  Text[] foodNameText;

    [SerializeField]  Text[] foodAmountText;

    [SerializeField]  Text[] nutritionAmouontTexts;

    int day;   //6,5,4,3,2,1,0
    int meal;   //0,1,2,3

    List<FoodPair> foods;

    const string head_format = "dd MMM";

    MealHistoryDAO mealDAO;
    ReadFoodNutrientDAO readFoodNutrientDAO;

    void Start()
    {
        day = 0;
        meal = 0;

        DateTime date;
        string date_str;

        for(int i=1;i<7;i++)
        {
            date = DateTime.Today.AddDays(0 - i);
            date_str = date.ToString(head_format, DateTimeFormatInfo.InvariantInfo);

            //Debug.Log(date_str);

            daysButton[i].GetComponentInChildren<Text>().text = date_str;
        }


        mealDAO = new MealHistoryDAO();
        readFoodNutrientDAO = new ReadFoodNutrientDAO();

        selectDay(0);
        selectMeal(0);
    }

    public void selectDay(int day_num)
    {
        day = day_num;

        for(int i=0;i<7;i++)  daysButton[i].GetComponent<Image>().color = Color.white;

        daysButton[day_num].GetComponent<Image>().color = Color.yellow;

        updateBelows();
    }

    public void selectMeal(int meal_num)
    {
        meal = meal_num;

        for(int i=0;i<4;i++) mealsButton[i].GetComponent<Image>().color = Color.white;

        mealsButton[meal_num].GetComponent<Image>().color = Color.yellow;

        updateBelows();
    }

    void updateBelows()
    {
        //update food info

        DateTime date = DateTime.Today.AddDays(0 - day);
        string date_str = date.ToString("yyyy-MM-dd");
        string meal_str = meal.ToString();

        foods = mealDAO.readFoodsOfDayMeal(date_str, meal_str);

        for(int i=0;i<foods.Count; i++)
        {
            foodNameText[i].text = foods[i].Name;
            foodAmountText[i].text = foods[i].Amount.ToString() + " g";
        }

        for(int i=foods.Count;i<5;i++)
        {
            foodNameText[i].text = "";
            foodAmountText[i].text = "";
        }

        //update nutrition info
        List<float> totalNuAmount = new List<float>();

        for (int i = 0; i < 6; i++) totalNuAmount.Add(0.0f);

        for (int i = 0; i < foods.Count; i++)
        {
            List<float> singleNuAmount = readFoodNutrientDAO.readNutrition(foods[i].ID, 0, 6);

            Debug.Log(singleNuAmount);

            for (int j = 0; j < 6; j++) totalNuAmount[j] += singleNuAmount[j] * (foods[i].Amount / 100.0f);

        }

        for (int i = 0; i < 6; i++)
        {
            if (Mathf.Approximately(totalNuAmount[i], Mathf.RoundToInt(totalNuAmount[i])))
            {
                float n_Amount = totalNuAmount[i];

                nutritionAmouontTexts[i].text = Mathf.RoundToInt(n_Amount).ToString();
            }
            else
            {
                float n_Amount = totalNuAmount[i];

                nutritionAmouontTexts[i].text = n_Amount.ToString("0.00");
            }

        }

    }

    public void onClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
