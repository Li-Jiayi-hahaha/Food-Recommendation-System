using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Handle_Setting_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("Setting_1");
    }

    public void Handle_UploadOREnquire_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("UploadOREnquireMenu");
    }

    public void Handle_EnquireNutrientContent_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("EnquireNutrient-1");
    }

    public void Handle_RecommendFood_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("RecommendMenu");
    }
}
