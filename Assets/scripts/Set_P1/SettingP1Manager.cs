using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingP1Manager : MonoBehaviour
{
    [SerializeField] Dropdown GenderSelect;
    [SerializeField] Dropdown YearSelect;
    [SerializeField] InputField HeightInput;
    [SerializeField] InputField WeightInput;

    [SerializeField] Text[] MealProportionTexts;

    List<string> firstblank;
    List<String> years_str;
    List<int> mealProportionList;

    int[] defaultMealProportion = new int[4] { 25, 50, 25, 0 };

    // Start is called before the first frame update
    void Start()
    {
        firstblank = new List<string>();
        firstblank.Add("-------");

        setYearOptions();

        updateBelows();
    }

    void setYearOptions()
    {
        int yearNow = DateTime.Now.Year;

        YearSelect.ClearOptions();
        YearSelect.AddOptions(firstblank);

        years_str = new List<string>();

        for(int i = 4; i <= 90; i++)
        {
            years_str.Add((yearNow - i).ToString());
        }
        YearSelect.AddOptions(years_str);

    }

    void updateBelows()
    {
        GenderSelect.value = PlayerPrefs.GetInt("Gender");
        int yearOfBirth = PlayerPrefs.GetInt("YearOfBirth");

        if (yearOfBirth != 0)
        {
            int yearNow = DateTime.Now.Year;
            int drop_value = yearNow - yearOfBirth - 4 + 1;
            YearSelect.value = drop_value;
        }

        float height = PlayerPrefs.GetFloat("Height");
        if (height != 0)
        {
            HeightInput.text = height.ToString();
        }

        float weight = PlayerPrefs.GetFloat("Weight");
        if (weight != 0)
        {
            WeightInput.text = weight.ToString();
        }

        mealProportionList = new List<int>();

        bool hasMeal = PlayerPrefs.HasKey("Meal-0");
        if (hasMeal)
        {
            for (int i = 0; i < 4; i++)
            {
                int num = PlayerPrefs.GetInt("Meal-" + i.ToString());
                mealProportionList.Add(num);
                MealProportionTexts[i].text = num.ToString();

            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                mealProportionList.Add(defaultMealProportion[i]);
                MealProportionTexts[i].text = defaultMealProportion[i].ToString();

            }
        }


    }

    public void OnSelectGender()
    {
        PlayerPrefs.SetInt("Gender", GenderSelect.value);
        //male = 1, female = 2
        //Debug.Log("Gender: "+ PlayerPrefs.GetInt("Gender").ToString());
    }

    public void OnSelectYear()
    {
        PlayerPrefs.SetInt("YearOfBirth", int.Parse(YearSelect.options[YearSelect.value].text));

        //Debug.Log("YearOfBirth: "+ PlayerPrefs.GetInt("YearOfBirth").ToString());
    }

    public void onInputHeight()
    {
        string height_str = HeightInput.text;
        if (height_str.Length == 0)
        {
            PlayerPrefs.SetFloat("Height", 0.0f);
        }
        else
        {
            PlayerPrefs.SetFloat("Height", float.Parse(height_str));
        }

        //Debug.Log("Height: " + PlayerPrefs.GetFloat("Height").ToString());
    }

    public void onInputWeight()
    {
        string weight_str = WeightInput.text;
        if (weight_str.Length == 0)
        {
            PlayerPrefs.SetFloat("Weight", 0.0f);
        }
        else
        {
            PlayerPrefs.SetFloat("Weight", float.Parse(weight_str));
        }

        //Debug.Log("Weight: " + PlayerPrefs.GetFloat("Weight").ToString());
    }

    void updateAmountText(int id, int num)
    {
        mealProportionList[id] = num;
        MealProportionTexts[id].text = num.ToString();

        int otherLeft = 100 - mealProportionList[0] - mealProportionList[1] - mealProportionList[2];
        mealProportionList[3] = otherLeft;
        MealProportionTexts[3].text = otherLeft.ToString();
    }


    public void OnClickAdd(int numID)
    {
        int total3 = mealProportionList[0] + mealProportionList[1] + mealProportionList[2] + 5;
        if (total3 > 100) return;

        updateAmountText(numID, mealProportionList[numID] + 5);
    }

    public void OnClickMinus(int numID)
    {
        if (mealProportionList[numID] - 5 < 0) return;

        updateAmountText(numID, mealProportionList[numID] - 5);
    }

    void saveMealProportionPlayerPrefs()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetInt("Meal-" + i.ToString(), mealProportionList[i]);
        }
    }


    public void onClickMain()
    {
        saveMealProportionPlayerPrefs();
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    
    public void onClickNext()
    {
        saveMealProportionPlayerPrefs();
        PlayerPrefs.Save();
        SceneManager.LoadScene("Setting_2");
    }

}
