using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingP2Manager : MonoBehaviour
{
    [SerializeField] Dropdown[] dropdowns;
    [SerializeField] Text[] amountTexts;

    List<string> nameOfNutritions;
    List<int> addMinus;
    List<string> firstblank;
    List<string> optionList;

    int[] defaultPropor = new int[3] { 25, 50, 25 };

    // Start is called before the first frame update
    void Start()
    {
        nameOfNutritions = new List<string>();
        addMinus = new List<int>();

        setDropdownOptions();

        //read and display amount value
        for (int i=0;i<12;i++)
        {

            if ((i >= 1) && (i <= 3))
            {
                if(PlayerPrefs.HasKey("AddMinus-" + i.ToString()))
                {
                    addMinus.Add(PlayerPrefs.GetInt("AddMinus-" + i.ToString()));
                }
                else
                {
                    addMinus.Add(defaultPropor[i - 1]);
                }
                amountTexts[i].text = addMinus[i].ToString();
            }

            else
            {
                addMinus.Add(PlayerPrefs.GetInt("AddMinus-" + i.ToString()));
                updateAmountText(i, addMinus[i]);
            }
        }

        //set nameOfNutritions - part1
        for(int i=0;i<7;i++)
        {
            nameOfNutritions.Add(((NutritionNames)i).ToString());
            Console.WriteLine(nameOfNutritions[i]);
        }

        //set nameOfNutritions - part2
        for (int i = 0; i < 5; i++)
        {
            if(PlayerPrefs.HasKey("PersonalisedNutrition-" + i.ToString()))
            {
                string nu_name = PlayerPrefs.GetString("PersonalisedNutrition-" + i.ToString());
                nameOfNutritions.Add(nu_name);

                //set dropdown values
                if (nu_name.Equals("None"))
                {
                    dropdowns[i].value = 0;
                }
                else
                {
                    NutritionNames nuNameEnum = (NutritionNames)System.Enum.Parse(typeof(NutritionNames), nu_name);
                    dropdowns[i].value = (int)nuNameEnum - 6;
                }
                
            }

            else
            {
                nameOfNutritions.Add("None");
                dropdowns[i].value = 0;
            }

        }
        

    }

    void addPropor (int numID)
    {
        if (addMinus[1] + addMinus[2] + 5 > 100) return;

        updateAmountTextPropor(numID, addMinus[numID] + 5);
    }

    void minusPropor (int numID)
    {
        if (addMinus[numID]-5 <0) return;

        updateAmountTextPropor(numID, addMinus[numID] - 5);
    }

    void updateAmountTextPropor (int id, int num)
    {
        addMinus[id] = num;

        addMinus[3] = 100 - addMinus[1] - addMinus[2];

        amountTexts[id].text = addMinus[id].ToString();
        amountTexts[3].text = addMinus[3].ToString();
    }

    public void OnClickAdd(int numID)
    {
        if ((numID >= 1) && (numID <= 3))
        {
            addPropor(numID);
            return;
        }

        updateAmountText(numID, addMinus[numID]+5);
    }

    public void OnClickMinus(int numID)
    {
        if ((numID >= 1) && (numID <= 3))
        {
            minusPropor(numID);
            return;
        }

        updateAmountText(numID, addMinus[numID] - 5);
    }

    public void OnUserSelect(int otherID)
    {
        int choice = dropdowns[otherID].value;
        if (choice != 0)
        {
            choice += 6;
            nameOfNutritions[otherID + 7] = ((NutritionNames)choice).ToString();
        }

        else
        {
            nameOfNutritions[otherID + 7] = "None";
        }

        //updateAmountText(otherID + 7, 0);
    }

    void updateAmountText(int id, int num)
    {
      
        addMinus[id] = num;

        //Debug.Log("Edit AddMinus[" + id.ToString() + "] = " + addMinus[id].ToString());

        string num_str;
        if (num > 0) num_str = '+' + num.ToString();
        else num_str = num.ToString();

        amountTexts[id].text = num_str + "%";

        //Debug.Log("Display AddMinus-" + id.ToString() + " = " + num_str);
    }

    void setDropdownOptions()
    {
        //set dropdown options
        firstblank = new List<string>();
        firstblank.Add("-------");

        optionList = new List<string>();
        for (int i = 7; i < 24; i++)
        {
            optionList.Add(((NutritionNames)i).ToString());
        }

        for (int i = 0; i < 5; i++)
        {
            dropdowns[i].ClearOptions();
            dropdowns[i].AddOptions(firstblank);

            dropdowns[i].AddOptions(optionList);
        }
    }

    void savePlayerPrefs()
    {
        //save playprefs
        for (int i = 0; i < 12; i++)
        {
            PlayerPrefs.SetInt("AddMinus-" + i.ToString(), addMinus[i]);
            //Debug.Log("Set AddMinus-" + i.ToString() + " = " + addMinus[i].ToString());
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetString("PersonalisedNutrition-" + i.ToString(), nameOfNutritions[i + 7]);
        }

        PlayerPrefs.Save();

    }


    public void onClickLast()
    {
        savePlayerPrefs();
        SceneManager.LoadScene("Setting_1");
    }


    public void onClickMain()
    {
        savePlayerPrefs();
        //back to main scene
        SceneManager.LoadScene("MainMenu");
    }
}
