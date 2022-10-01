using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Camera cam;
    private Vector3 pos;
    public ButtonObject[] buttons;

    public Text text;
    private string message = "Press the big red button!";
    public Text textTimer;

    public GameObject wrongButtonObject;
    public Image wrongButtonImage;
    public GameObject gameOverObject;
    public Image gameOverImage;

    public ScreenFader screenFader;

    void Start()
    {
        cam = GetComponent<Camera>();
        pos = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        StartCoroutine("TimerToLose");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }

    public void HandleSelection()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            ButtonObject hitted = hit.collider.gameObject.GetComponent<ButtonObject>();
            if (hitted != null)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (hitted == buttons[i] && !hitted.wasPressed && hitted.needToPress)
                    {
                        hitted.wasPressed = true;
                        int n = Random.Range(0, buttons.Length);
                        buttons[n].needToPress = true;
                        buttons[n].wasPressed = false;
                        message = buttons[n].message;
                        StopCoroutine("TimerToLose");
                        StartCoroutine("TimerToLose");
                        return;
                    }
                }

                StartCoroutine("WrongButton");
            }
        }
    }

    private IEnumerator TimerToNext()
    {
        text.text = "To next disaster remains...";
        textTimer.text = "10 sec";
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            textTimer.text = (9 - i).ToString() + " sec";
        }
        text.text = message;
        StartCoroutine("TimerToLose");
    }

    private IEnumerator TimerToLose()
    {
        text.text = message;
        textTimer.text = "10 sec";
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            textTimer.text = (9 - i).ToString() + " sec";
        }
        StartCoroutine("GameOver");
    }

    private IEnumerator WrongButton()
    {
        screenFader.fadeState = ScreenFader.FadeState.In;
        yield return new WaitForSeconds(2f);
        wrongButtonObject.SetActive(true);
        screenFader.fadeState = ScreenFader.FadeState.OutEnd;
        yield return new WaitForSeconds(1f);
        StartCoroutine("GameOver");
    }

    private IEnumerator GameOver()
    {
        screenFader.fadeState = ScreenFader.FadeState.In;
        yield return new WaitForSeconds(2f);
        gameOverObject.SetActive(true);
        screenFader.fadeState = ScreenFader.FadeState.OutEnd;
    }
}
