using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecommendMenuManager : MonoBehaviour
{
    public void OnClickPlanAssess()
    {
        SceneManager.LoadScene("AssessPlan");
    }

    public void OnClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickLongTerm()
    {
        SceneManager.LoadScene("LongTermAdvice");
    }
}
