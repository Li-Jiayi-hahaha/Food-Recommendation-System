using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnquireNutrientContentManager : MonoBehaviour
{
    [SerializeField]
    InputField foodInputField;

    [SerializeField]
    InputField amountInputField;

    [SerializeField]
    Dropdown dropdown;

    [SerializeField]
    Text[] nutritionAmouontTexts;

    List<FoodPair> relatedFoods;

    SearchFoodName searchFoodName;
    ReadFoodNutrientDAO readFoodNutrientDAO;

    List<string> firstblank;

    static int food_ID = -1;
    static string food_Name = null;
    static int amount = -1;

    int page;

    void Start()
    {
        //food_ID = -1;
        //amount = -1;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Equals("EnquireNutrient-1"))
        {
            page = 0;
            //food_ID = -1;
            //amount = -1;
            Debug.Log("page = 0");
        }
        else
        {
            page = 1;
            Debug.Log("page = 1");
        }

        searchFoodName = new SearchFoodName();
        readFoodNutrientDAO = new ReadFoodNutrientDAO();

        firstblank = new List<string>();
        firstblank.Add("-------");

        refreshPage();
    }

    public void onUserTypeFood()
    {
        string user_type = foodInputField.text;

        if (user_type.Length < 3) return;

        print(user_type);

        relatedFoods = searchFoodName.search(user_type);

        print(relatedFoods.Count);

        List<string> relatedFoodNames = new List<string>();

        for (int i = 0; i < relatedFoods.Count; i++)
        {
            relatedFoodNames.Add(relatedFoods[i].Name);
        }

        print(relatedFoodNames.Count);

        dropdown.ClearOptions();
        dropdown.AddOptions(firstblank);

        dropdown.AddOptions(relatedFoodNames);

        dropdown.Show();

    }

    public void onUserTypeAmount()
    {
        string amount_str = amountInputField.text;
        Debug.Log("Amount = " + amount_str);

        if (amount_str.Length == 0) return;

        //save amount value

        amount = int.Parse(amount_str);
    }

    public void onUserSelect()
    {
        int selection_id = dropdown.value;

        if (selection_id == 0) return;

        //change currentFoods
        selection_id -= 1;

        food_ID = relatedFoods[selection_id].ID;
        food_Name = relatedFoods[selection_id].Name;

        Debug.Log("set text: " + food_Name);
        //foodTexts[list_id].text = currentFoods[list_id].Name;

        foodInputField.text = food_Name;
    }


    void refreshPage()
    {
        if ((food_ID == -1) || (amount == -1)) return;

        foodInputField.text = food_Name;
        amountInputField.text = amount.ToString();
        onUserClickSearch();
    }

    public void onUserClickSearch()
    {
        if ((food_ID == -1) || (amount == -1)) return;

        List<float> nutritionAmount = readFoodNutrientDAO.readNutrition(food_ID, page, 12);

        for(int i=0; i<12; i++)
        {
            if(Mathf.Approximately(nutritionAmount[i], Mathf.RoundToInt(nutritionAmount[i])))
            {
                float n_Amount = nutritionAmount[i];
                n_Amount *= amount / 100.0f;
                
                nutritionAmouontTexts[i].text = Mathf.RoundToInt(n_Amount).ToString();
            }
            else
            {
                float n_Amount = nutritionAmount[i];
                n_Amount *= amount / 100.0f;
                nutritionAmouontTexts[i].text = n_Amount.ToString("0.00");
            }
            
        }
    }

    public void onClickLastORNext()
    {
        if(page==0) SceneManager.LoadScene("EnquireNutrient-2");
        else SceneManager.LoadScene("EnquireNutrient-1");
    }

    public void onClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
