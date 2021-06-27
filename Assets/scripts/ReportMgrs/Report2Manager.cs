using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Report2Manager : MonoBehaviour
{
    [SerializeField] Text[] planDataTexts;
    [SerializeField] Text[] stdDataTexts;
    [SerializeField] Text[] rateTexts;

    NutritionContent nc_plan;
    NutritionContent nc_std;
    ReportMgr theMgr;

    // Start is called before the first frame update
    void Start()
    {
        theMgr = Camera.main.GetComponent<ReportMgr>();
        nc_plan = theMgr.nc_a;
        nc_std = theMgr.nc_b;

        //display plan data
        displayNut(planDataTexts, nc_plan);
        //display std data
        displayNut(stdDataTexts, nc_std);
        //display rate data
        displayRate();

    }

    void displayNut(Text[] dataTexts, NutritionContent nc)
    {
        for (int i = 0; i < 12; i++)
        {
            float ft = nc.nameValuePairs[(NutritionNames)(i+12)];
            if (ft < 10)
            {
                dataTexts[i].text = ft.ToString("0.00");
            }

            else
            {
                dataTexts[i].text = Mathf.RoundToInt(ft).ToString();
            }
        }
    }

    void displayRate()
    {
        for (int i = 0; i < 12; i++)
        {
            float ft_a = nc_plan.nameValuePairs[(NutritionNames)(i+12)];
            float ft_b = nc_std.nameValuePairs[(NutritionNames)(i+12)];

            int pct = Mathf.RoundToInt(ft_a * 100f / ft_b);

            rateTexts[i].text = pct.ToString() + "%";
        }
    }

    

}
