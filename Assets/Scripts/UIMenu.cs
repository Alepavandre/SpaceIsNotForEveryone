using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public Text startButtonText;

    public string startEn = "Start!";
    public string startRu = "Поехали!";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Ru()
    {
        DataManager.Instance.eng = false;
        startButtonText.text = startRu;
    }

    public void En()
    {
        DataManager.Instance.eng = true;
        startButtonText.text = startEn;
    }
}
