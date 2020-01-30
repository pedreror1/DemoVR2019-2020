using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using NaughtyAttributes;
using OVR;

[ExecuteInEditMode]
public class RunTimeConsoleV2 : MonoBehaviour
{
    #region Vars
    public class Message
    {
       public   String MessageText;
       public LogType Type;
       public int Times;
       public String currentTimes;
       public Message(String MessageText, LogType Type, int Times, String currentTimes)
       {
           this.MessageText = MessageText;
           this.Type = Type;
           this.Times = Times;
           this.currentTimes = currentTimes;
       }       
    }
     struct Options
    {
       public bool showLogs ;
       public bool showErrors ;
       public bool showWarnings ;
       public bool isPaused;
    }
    private Options myOptions;
    private List<Message> messages = new List<Message>();
    [SerializeField] private RectTransform Panel;
    [SerializeField] private Text ErrorLog;
    [SerializeField] private Button ToggleButton;
    [SerializeField] private GameObject Container;
    private bool ready = false;
    private float scale = 0.51f;
    private IEnumerator animateFN;
    bool isready;
    private int textLength = 0;
    [SerializeField] bool isDebug;
    [SerializeField] private Button warningsButton;
    [SerializeField] private Button ErrorsButton;
    [SerializeField] private Button LogsButton;
    [SerializeField] private RectTransform pauseButton;
    [SerializeField] private RectTransform clearButton;
    [SerializeField] private RectTransform closeButton;
    [SerializeField] bool startOnAwake = false;
    bool OVRInputEnabled=true;
    enum ConsoleStatus
    {
        Hide,
        Minibar,
        FullConsole
    }
    private ConsoleStatus currentStatus = ConsoleStatus.Hide;
    string currentColor;

    public void Start()
    {
        closeConsole();
    }
    #endregion
    void OnEnable()
    {          
        if (Application.platform != RuntimePlatform.WindowsEditor && !Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }        
        StartCoroutine(delayAndStart());
        if (startOnAwake)
        {
            ToggleFullConsole();
            toggleLogs();
            toggleWarnings();
            toggleErrors();
        }
        ToggleButton.onClick.AddListener(ToggleFullConsole);
        #if UNITY_EDITOR
                Type.GetType("UnityEditor.LogEntries,UnityEditor.dll")
               .GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
               .Invoke(null, null);
        #endif
        
    }     
    IEnumerator delayAndStart()
    {
        yield return new WaitForSeconds(0.05f);
        Application.logMessageReceived += HandleLog;
    }
    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    [Button("Toggle Logs")]
    public void toggleLogs()
    {
        ToggleOptions(0);
    }

    [Button("Toggle Warnings")]

    public void toggleWarnings()
    {
        ToggleOptions(1);
    }

    [Button("toggle Errors")]

    public void toggleErrors()
    {
        ToggleOptions(2);
    }

    [Button("Pause Console")]

    public void Pauseconsole()
    {
        ToggleOptions(3);
    }
   

    public void ToggleOptions(int i)
    {
        switch(i)
        {
            case 0:
                myOptions.showLogs = !myOptions.showLogs;
                LogsButton.image.color = (myOptions.showLogs) ?  Color.white : Color.gray;
                break;
            case 1:
                myOptions.showWarnings= !myOptions.showWarnings;
                warningsButton.image.color = (myOptions.showWarnings) ? Color.white : Color.gray;
                break;
            case 2:
                myOptions.showErrors= !myOptions.showErrors;
                ErrorsButton.image.color = (myOptions.showErrors) ? Color.white : Color.gray;
                break;
            case 3:
                myOptions.isPaused = !myOptions.isPaused;
                pauseButton.GetComponent<Button>().image.color = (myOptions.isPaused) ? Color.white : Color.gray;
                break;
        }
        displayTexts();
    }

    public void setupConsole(float size)
    {
         
        Panel.sizeDelta = new Vector2(Panel.sizeDelta.x, size);
        
    }  

    [Button("close")]

    public void closeConsole()
    {
        if(myOptions.isPaused)
        ToggleOptions(3);
        if (!myOptions.showErrors)
            ToggleOptions(2);
        currentStatus = ConsoleStatus.Hide;
        pauseButton.sizeDelta = new Vector2(pauseButton.sizeDelta.x, 30);
        clearButton.sizeDelta = new Vector2(clearButton.sizeDelta.x, 30);
        closeButton.sizeDelta = new Vector2(closeButton.sizeDelta.x, 30);
        setupConsole(0);
    }
    [Button("display text")]

    public void displayTexts()
    {
        ErrorLog.text = "";
        foreach (Message s in messages)
        {
            if(isRelevant(s.Type))
            {
                if (messages.Count > 250)
                {
                    ClearMessages();
                    break;
                }
                else
                    ErrorLog.text += currentColor + s.currentTimes + "  " + s.MessageText + ((s.Times > 1) ? s.Times + "\n" : "\n");

            }
                     
        }
    }

    [Button("toggle full Console")]

    public void ToggleFullConsole()
    {        
        if (currentStatus == ConsoleStatus.Minibar || currentStatus ==ConsoleStatus.Hide )
        {
            displayTexts();
            isready = true;
            currentStatus = ConsoleStatus.FullConsole;
            setupConsole(240);
            pauseButton.sizeDelta = new Vector2(pauseButton.sizeDelta.x, 40);
            clearButton.sizeDelta = new Vector2(clearButton.sizeDelta.x, 40);
            closeButton.sizeDelta = new Vector2(closeButton.sizeDelta.x, 40);

        }
        else
        {
            closeConsole();
        }
    }
    [Button("clear")]

    public void ClearMessages()
    {
      //  ErrorLog.rectTransform.sizeDelta = new Vector2(ErrorLog.rectTransform.sizeDelta.x, 240);
        //E/rrorLog.rectTransform.position = new Vector3(ErrorLog.rectTransform.position.x, 0, ErrorLog.rectTransform.position.z);
        messages.Clear();
        textLength = 0;
         ErrorLog.text = "";
    }
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (!myOptions.isPaused)
        {
            if (textLength > 14000)
            {
                return;
            }
            if (!isready)
            {
                Container.SetActive(false);
                Container.SetActive(true);
            }
            if (logString.IndexOf("In order to call GetTransformInfoExpectUpToDate, RendererUpdateManager.UpdateAll must be called first.") >= 0)
            {
                return;
            }
            if (logString.IndexOf("Internal: JobTempAlloc has allocations that are more than 4 frames old - this is not allowed and likely a leak") >= 0)
            {
                return;
            }
            if (logString.IndexOf("To Debug, enable the define: TLA_DEBUG_STACK_LEAK in ThreadsafeLinearAllocator.cpp. This will output the callstacks of the leaked allocations") >= 0)
            {
                return;
            }

            Message newMessage = new Message(logString + "</color>   ",type,1,"[" + DateTime.Now.ToString("HH:mm:ss") + "]");
#if false
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].MessageText == newMessage.MessageText)
                {
                    newMessage.Times = messages[i].Times + 1;
                    messages.RemoveAt(i);
                    break;
                }
            }
#endif

            textLength += newMessage.MessageText.Length;
            messages.Insert(0, newMessage);
            ErrorLog.rectTransform.sizeDelta += Vector2.up * 60;

            if (currentStatus == ConsoleStatus.Hide && isRelevant(newMessage.Type))
            {
                currentStatus = ConsoleStatus.Minibar;
               setupConsole(30);
                
            }

            displayTexts();
        }
    }    
    public bool isRelevant(LogType s)
    {        
        if(myOptions.showErrors &&(s== LogType.Assert|| s == LogType.Error|| s == LogType.Exception))
        {
            currentColor = "<color=red>";
            return true;
        }
        else if(myOptions.showLogs && s== LogType.Log)
        {
            currentColor = "<color=white>";
            return true;
        }
        else if(myOptions.showWarnings&& s== LogType.Warning)
        {
            currentColor = "<color=yellow>";
            return true;
        }
        currentColor = "";
        return false;
      
    }     
    void Update()
    { 
        if(OVRInputEnabled && OVRInput.Get(OVRInput.Button.One))
        {

            ToggleFullConsole();
            OVRInputEnabled = false;
        }
        else if(!OVRInputEnabled && !OVRInput.Get(OVRInput.Button.One))
        {
            OVRInputEnabled = true;
        }


        if (OVRInput.Get(OVRInput.Button.Two))
        {
            ClearMessages() ;
        }

    }
    [Button("Test Log")]

    public void testLog()
    {
        Debug.Log("test log");
    }

    [Button("test Warning")]

    public void testWarning()
    {
        Debug.LogWarning("Warning");
    }
    [Button("test Error")]

    public void testError()
    {
        Debug.LogError("test Error");
    }
}
