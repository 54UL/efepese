using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EPS;
using UnityEngine.InputSystem;
using EPS.Core;

public class LobbyView : MonoBehaviour, IUIView
{
    //DEPENDENCIES
    private IUIManager UiService { get; set; }
    private IMatchManager MatchManagerService { get; set; }
    private INetworking NetworkingServices { get; set; }
    private InputSystem.PlayerActions _input { get; set; }

    //MEMBERS
    public GameObject LobbyCamera;
    [Space(10)]
    public bool ConectionStatus = false;
    public string currentAddress;
    public int currentPort;
    [Space(10)]
    public Transform ServerShellRoot;
    public Transform MatchesHolder;
    public Transform gameLogsHolder;
    public GameObject matchUIdelegate;
    [Space(10)]
    public Button CreateMatch;
    public Button SpawnPlayerBtn;
    public Button RefreshList;
    public Button ToggleConnect;
    [Space(10)]
    public InputField ServerIpInputField;
    public InputField ServerPortInputField;
    public InputField ClientPortInputField;
    public InputField MatchName;
    public GameObject consoleLogPrefab;
    [Space(10)]
    [Header("Networl match test")]
    bool ShellStatus;

    void Start()
    {
        var inputService = EPS.ServiceInjector.getSingleton<InputService>();
        _input = inputService.GetInputActions();

        UiService = ServiceInjector.getSingleton<UIManager>();
        UiService.RegisterView(StackingLevel.GUI, this, true);

        currentAddress = ServerIpInputField.text.Trim();
        currentPort = int.Parse(this.ServerPortInputField.text);

        ToggleConnect.onClick.AddListener(() => {
            ToggleConnectAction();
        });

        CreateMatch.onClick.AddListener(() => {
            string message = string.Format("Match created invite friends via direct ip [{0}] port [{1}]: ", currentAddress, currentPort);
            RenderShell(false);
            EnableLobbyCamera(false);
            StartCoroutine(NetworkStatus(NetworkSytemType.HOST, true));
        });
    }

    void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            RenderShell(ShellStatus);
            ShellStatus = !ShellStatus;
        }
    }

    void RenderShell(bool visible)
    {
        ServerShellRoot.gameObject.SetActive(visible);
        _input.cursorInputForLook = !visible;
        _input.cursorLocked = !visible;
        //Health.enabled = !visible;
    }

    void LogInfo(string message)
    {
        TMPro.TextMeshProUGUI text = GameObject.Instantiate(consoleLogPrefab, gameLogsHolder).GetComponent<TMPro.TextMeshProUGUI>();
        text.SetText("[INFO]  " + message);
    }

    IEnumerator NetworkStatus(NetworkSytemType netType, bool connect = false)
    {
        if (connect)
        {
            //CHANGE BY MATCH START
            //gameNetworking.Start(netType, currentAddress, currentPort);
        }
        else
        {
            //MATCH END...
            //gameNetworking.Stop();
        }

        yield return new WaitForFixedUpdate();
    }

    private void EnableLobbyCamera(bool enable)
    {
        LobbyCamera.SetActive(enable);
    }

    void ToggleConnectAction()
    {
        currentAddress = this.ServerIpInputField.text.Trim();
        currentPort = int.Parse(this.ServerPortInputField.text);

        //if (!networkManager.IsConnectedClient)
        //{
        //    LogInfo("Joning match...");
        //    this.ToggleConnect.GetComponentInChildren<Text>().text = "Disconnect";
        //    StartCoroutine(NetworkStatus(NetworkSytemType.CLIENT, true));
        //}
        //else
        //{
        //    LogInfo("Player disconnected");
        //    this.ToggleConnect.GetComponentInChildren<Text>().text = "Connect";
        //    ConectionStatus = false;
        //    StartCoroutine(NetworkStatus(NetworkSytemType.CLIENT, false));
        //    EnableLobbyCamera(true);
        //    RenderShell(true);
        //}
    }

    //IUIView
    public void OnClose()
    {
        Debug.Log("[MainMenuUI]: se ha salido del menu principal");
    }

    public void OnShow()
    {
        GameObject.Find("CINEMATIC_CAMERA").SetActive(true);
    }

    public GameObject ViewParent()
    {
        return this.gameObject;
    }
}