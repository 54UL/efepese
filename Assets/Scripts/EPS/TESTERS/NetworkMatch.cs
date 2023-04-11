using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Netcode.Transports.UNET;
using System;


using EPS;
using EPS.Core;


public class NetworkMatch : MonoBehaviour
{
    //Clasic vars
    public GameObject LobbyCamera;
    
    public bool ConectionStatus = false;
    public string currentAddress;
    public int currentPort;

    //Ui members
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
    public Text Health;

    private InputSystem.PlayerActions _input;


    private NetworkManager networkManager;
  
    //DEPENDENCIES
    public INetworking gameNetworking;

    public void SetUIHealth(double value)
    {
        Health.text = "Health:" + value + "%";
    }

    void LogInfo(string message)
    {
        TMPro.TextMeshProUGUI text =  GameObject.Instantiate(consoleLogPrefab, gameLogsHolder).GetComponent<TMPro.TextMeshProUGUI>();
        text.SetText("[INFO]  " + message);
    }

    void RenderShell(bool visible)
    {
        ServerShellRoot.gameObject.SetActive(visible);
        _input.cursorInputForLook = !visible;
        _input.cursorLocked = !visible;
        Health.enabled = !visible;
    }

    IEnumerator NetworkStatus(NetworkSytemType netType,  bool connect = false)
    {
        if (connect)
        {
            gameNetworking.Start(netType, currentAddress, currentPort);
        }
        else
        {
            gameNetworking.Stop();
        }
        
        yield return new WaitForFixedUpdate();
    }

    void ToggleConnectAction()
    {
        currentAddress = this.ServerIpInputField.text.Trim();
        currentPort = int.Parse(this.ServerPortInputField.text);

        if (!networkManager.IsConnectedClient)
        {
            LogInfo("Joning match...");
            this.ToggleConnect.GetComponentInChildren<Text>().text = "Disconnect";
            StartCoroutine(NetworkStatus(NetworkSytemType.CLIENT, true));
        }
        else
        {
            LogInfo("Player disconnected");
            this.ToggleConnect.GetComponentInChildren<Text>().text = "Connect";
            ConectionStatus = false;
            StartCoroutine(NetworkStatus(NetworkSytemType.CLIENT, false));
            EnableLobbyCamera(true);
            RenderShell(true);
        }
    }

    private void EnableLobbyCamera(bool enable)
    {
        LobbyCamera.SetActive(enable);
    }

    void Start()
    {
        networkManager = NetworkManager.Singleton;
        currentAddress = this.ServerIpInputField.text.Trim();
        currentPort = int.Parse(this.ServerPortInputField.text);
        gameNetworking = ServiceInjector.getSingleton<Networking>();
        var inputService = EPS.ServiceInjector.getSingleton<InputService>();
        _input = inputService.GetInputActions();

        SpawnPlayerBtn.onClick.AddListener(() => {
           
        });

        ToggleConnect.onClick.AddListener(() => {
            ToggleConnectAction();
        });

        CreateMatch.onClick.AddListener(() => {
            string message = string.Format("Match created invite friends via direct ip [{0}] port [{1}]: ", currentAddress, currentPort);
            RenderShell(false);
            EnableLobbyCamera(false);
            StartCoroutine(NetworkStatus(NetworkSytemType.HOST, true));
        });

        LogInfo("efepese game ready...");
    }

    private void OnTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
    {
        var warning = "OnTransportEvent: " + eventType;
        Debug.LogWarning(warning);
        LogInfo(warning);
    }

    // NETWORK MATCH TEST
    bool ShellStatus;

    void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            RenderShell(ShellStatus);
            ShellStatus = !ShellStatus;
        }

        // this.PingText.text = "PING:" + (NetClient.GetUpssertLatency()*1000).ToString("f2")+"s";
    }
}
