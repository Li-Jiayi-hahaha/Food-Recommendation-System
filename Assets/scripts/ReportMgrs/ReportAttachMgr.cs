using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportAttachMgr : MonoBehaviour
{
    ReportMgr theMgr;

    [SerializeField] Sprite AssessPlanBG;
    [SerializeField] Sprite LongTermBG;
    [SerializeField] Image BG_Image;

    [SerializeField] Text[] TableHeadTexts;
    [SerializeField] Text Bottomtext;

    const string assessPlanTableHeader_str = "Plan    Std      Rate";
    const string longTermTableHeader_str = "Avg     Std      Rate";

    const string assessPlanBottomtext_str = "* std is the goal nutrition intake of this meal, which can be calculated only after fullfilling Setting page 1.";
    const string longTermBottomtext_str = "* std is the goal nutrition intake of one day, which can be calculated only after fullfilling Setting page 1.";

    //Dictionary<string, Sprite> sprite_dic;
    Dictionary<string, string> tableHeader_dic;

    void Start()
    {
        theMgr = Camera.main.GetComponent<ReportMgr>();

        tableHeader_dic = new Dictionary<string, string>();
        tableHeader_dic.Add("AssessPlan", assessPlanTableHeader_str);
        tableHeader_dic.Add("LongTermAdvice", longTermTableHeader_str);

    }

    //------set backgrounds-------
    public void setBackground(string scene_name)
    {
        SetSprite(scene_name);
        SetTableHeader(scene_name);
        SetBottomText(scene_name);
    }


    void SetSprite(string scene_name)
    {
        if (scene_name.Equals("AssessPlan"))
        {
            BG_Image.sprite = AssessPlanBG;
        }

        if (scene_name.Equals("LongTermAdvice"))
        {
            BG_Image.sprite = LongTermBG;
        }

    }

    void SetTableHeader(string scene_name)
    {
        string table_header = "ABC";
        if (scene_name.Equals("AssessPlan"))
        {
            table_header = assessPlanTableHeader_str;
        }

        if (scene_name.Equals("LongTermAdvice"))
        {
            table_header = longTermTableHeader_str;
        }

        for (int i = 0; i < 2; i++)
            TableHeadTexts[i].text = table_header;
    }

    void SetBottomText(string scene_name)
    {
        string text_str = "* None";

        if (scene_name.Equals("AssessPlan"))
        {
            text_str = assessPlanBottomtext_str;
        }

        if (scene_name.Equals("LongTermAdvice"))
        {
            text_str = longTermBottomtext_str;
        }


        Bottomtext.text = text_str;
    }

    //------button handlings-----

    public void OnClickMain()
    {
        theMgr.backToMainFromReport();
    }

    public void OnClickEdit()
    {
        theMgr.backFromReport();
    }

    public void OnClickSwap()
    {
        theMgr.swapReportPage();
    }



}
