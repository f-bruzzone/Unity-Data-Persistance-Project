using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public TMP_InputField Name;

    public void StartGame()
    {
        UpdateName();
        SceneManager.LoadScene(1);
    }

    private void UpdateName()
    {
        ScreenManager.Instance.Name = Name.text;
    }
}
