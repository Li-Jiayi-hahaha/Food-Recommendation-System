using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UploadOREnquireMenu : MonoBehaviour
{
    public void Handle_Upload_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("UploadMealHistory");
    }

    public void Handle_Enquire_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("EnquireMealHistory");
    }

    public void Handle_Main_ButtonOnClickEvent()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
