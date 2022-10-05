using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private Camera cam;
    public Transform buttonsParent;
    private ButtonObject[] buttons;

    private string message = "";

    public Text textFirst;

    private Text[] texts;
    private Text[] textTimers;
    public GameObject vertBlocks;
    private GameObject[] monitorBlocks;
    private int indexLine = -1;

    public GameObject wrongButtonObject;
    public Image wrongButtonImage;
    public GameObject gameOverObject;
    public Image gameOverImage;

    public ScreenFader screenFader;

    private bool isDisaster = true;
    private int errorsCount = 0;
    public int errorsCountMax = 2;

    public GameObject tooltip;
    private Text tooltipText;

    private ButtonObject currentButton;

    public ButtonObject bigRedButton;
    public GameObject light;

    public Image target;

    private bool lastChance = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        int n = buttonsParent.childCount;
        buttons = new ButtonObject[n];
        for (int i = 0; i < n; i++)
        {
            buttons[i] = buttonsParent.GetChild(i).GetComponent<ButtonObject>();
        }
        n = vertBlocks.transform.childCount;
        monitorBlocks = new GameObject[n];
        texts = new Text[monitorBlocks.Length];
        textTimers = new Text[monitorBlocks.Length];
        for (int i = 0; i < n; i++)
        {
            monitorBlocks[i] = vertBlocks.transform.GetChild(i).gameObject;
            texts[i] = monitorBlocks[i].transform.GetChild(0).GetComponent<Text>();
            textTimers[i] = monitorBlocks[i].transform.GetChild(1).GetComponent<Text>();
        }
        tooltipText = tooltip.transform.GetChild(0).GetComponent<Text>();
        tooltip.SetActive(false);
        StartCoroutine("TimerFirst");
    }

    void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
        HandleTooltip();
    }

    public void HandleSelection()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            ButtonObject hitted = hit.collider.gameObject.GetComponent<ButtonObject>();
            if (hitted != null)
            {
                if (hitted == currentButton && !hitted.wasPressed && hitted.needToPress)
                {
                    hitted.wasPressed = true;
                    hitted.needToPress = false;
                    isDisaster = false;
                    vertBlocks.transform.GetChild(indexLine).GetChild(0).GetComponent<Text>().color = Color.green;
                    target.color = Color.green;
                    return;
                }

                ErrorButton();
            }
        }
    }

    public void HandleTooltip()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            ButtonObject hitted = hit.collider.gameObject.GetComponent<ButtonObject>();
            if (hitted != null)
            {
                tooltip.SetActive(true);
                if (DataManager.Instance.eng)
                {
                    tooltipText.text = hitted.tooltipText;
                }
                else
                {
                    tooltipText.text = hitted.tooltipTextRu;
                }
                
                return;
            }
            tooltip.SetActive(false);
        }
    }

    private IEnumerator TimerFirst()
    {
        if (DataManager.Instance.eng)
        {
            textFirst.text = "New entry detected!";
        }
        else
        {
            textFirst.text = "Обнаружено новое подключение!";
        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 3; i++)
        {
            if (DataManager.Instance.eng)
            {
                textFirst.text = "Loading";
            }
            else
            {
                textFirst.text = "Загрузка";
            }
            for (int j = 0; j <= 3; j++)
            {
                yield return new WaitForSeconds(0.5f);
                textFirst.text += ".";
            }
        }
        textFirst.gameObject.SetActive(false);
        NewDisaster();
    }

    private IEnumerator TimerToLose()
    {
        target.color = Color.red;
        Transform currentLine = vertBlocks.transform.GetChild(indexLine);
        Text currentMessage = currentLine.GetChild(0).GetComponent<Text>();
        Text currentTimer = currentLine.GetChild(1).GetComponent<Text>();
        currentMessage.color = Color.red;
        currentMessage.text = message;
        if (DataManager.Instance.eng)
        {
            currentTimer.text = "10 sec";
        }
        else
        {
            currentTimer.text = "10 сек";
        }
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(1);
            if (DataManager.Instance.eng)
            {
                currentTimer.text = (9 - j).ToString() + " sec";
            }
            else
            {
                currentTimer.text = (9 - j).ToString() + " сек";
            }
        }
        if (isDisaster)
        {
            if (DataManager.Instance.eng)
            {
                currentMessage.text = currentButton.wasntPressedMessage;
                currentTimer.text = "Err sec";
            }
            else
            {
                currentMessage.text = currentButton.wasntPressedMessageRu;
                currentTimer.text = "Err сек";
            }
                
            ErrorButton();
        }
        NewDisaster();
    }

    private void ErrorButton()
    {
        errorsCount++;
        CheckErrors();
    }

    private void CheckErrors()
    {
        if (errorsCount > errorsCountMax && !lastChance)
        {
            StopCoroutine("TimerToLose");
            StartCoroutine("TimerToGameOver");
        }
    }

    private IEnumerator GameOver()
    {
        //StopCoroutine("TimerToGameOver");
        screenFader.fadeState = ScreenFader.FadeState.In;
        yield return new WaitForSeconds(2f);
        gameOverObject.SetActive(true);
        screenFader.fadeState = ScreenFader.FadeState.OutEnd;
        SceneManager.LoadScene(0);
    }

    private IEnumerator TimerToGameOver()
    {
        lastChance = true;
        light.SetActive(false);
        isDisaster = true;
        NextLine();
        currentButton = bigRedButton;
        currentButton.needToPress = true;
        currentButton.wasPressed = false;
        message = currentButton.message;
        Transform currentLine = vertBlocks.transform.GetChild(indexLine);
        Text currentMessage = currentLine.GetChild(0).GetComponent<Text>();
        Text currentTimer = currentLine.GetChild(1).GetComponent<Text>();
        currentMessage.text = message;
        currentTimer.text = "10 сек";
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(1);
            currentTimer.text = (9 - j).ToString() + " сек";
        }
        if (isDisaster)
        {
            currentMessage.text = currentButton.wasntPressedMessage;
            currentTimer.text = "Err сек";
            StartCoroutine("GameOver");
            yield return null;
        }
        lastChance = false;
        light.SetActive(true);
        NewDisaster();
    }

    public void NewDisaster()
    {
        //StopCoroutine("TimerToGameOver");
        if (lastChance)
        {
            return;
        }
        isDisaster = true;
        NextLine();
        int n = Random.Range(0, buttons.Length);
        currentButton = buttons[n];
        currentButton.needToPress = true;
        currentButton.wasPressed = false;
        if (DataManager.Instance.eng)
        {

            message = currentButton.message;
        }
        else
        {

            message = currentButton.messageRu;
        }
        StartCoroutine("TimerToLose");
    }

    public void NextLine()
    {
        indexLine++;
        if (indexLine > monitorBlocks.Length - 1)
        {
            indexLine = monitorBlocks.Length - 1;
            vertBlocks.transform.GetChild(0).SetAsLastSibling();
        }
    }
}
