using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReportMgr : MonoBehaviour
{
    public GameObject Report1;
    GameObject report_1_object;

    public GameObject Report2;
    GameObject report_2_object;

    public NutritionContent nc_a;
    public NutritionContent nc_b;

    public void instantiateObject(NutritionContent a, NutritionContent b)
    {
        nc_a = a;
        nc_b = b;
        report_1_object = createReportObejct(1);
    }

    GameObject createReportObejct(int id)
    {
        GameObject gameObject;
        if (id == 1)
        {
            gameObject = Instantiate(Report1);
        }
        else
        {
            gameObject = Instantiate(Report2);
        }

        ReportAttachMgr objMgr = gameObject.GetComponent<ReportAttachMgr>();

        string scene_name = SceneManager.GetActiveScene().name;
        Debug.Log("Active Scene Name: " + scene_name);
        objMgr.setBackground(scene_name);

        return gameObject;
    }

    public void backFromReport()
    {
        if (report_1_object != null) Destroy(report_1_object);
        if (report_2_object != null) Destroy(report_2_object);
    }

    public void backToMainFromReport()
    {
        backFromReport();
        SceneManager.LoadScene("MainMenu");
    }

    public void swapReportPage()
    {
        if (report_1_object != null)
        {
            Destroy(report_1_object);
            report_2_object = createReportObejct(2);
        }
        else
        {
            Destroy(report_2_object);
            report_1_object = createReportObejct(1);
        }
    }
}
