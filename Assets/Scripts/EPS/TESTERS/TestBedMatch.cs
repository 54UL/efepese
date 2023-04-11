using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPS;
using EPS.Core;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Netcode.Transports.UNET;
using System;

//TODO: MOVE CODE TO PROPPER IMPLEMENTATIONS
//FOR EXAMPLE: BaseMatch class with methods and members

public class TestBedMatch : MonoBehaviour
{
    //Dependencies
    public INetworking   gameNetworking;
    public IGameSession  gameSession;

    private NetworkManager networkManager;

    [Space(10)]
    [Header("=====UI SYS=====")]
    //BaseMatch vars (refactor to a class)
    public GameObject LobbyCamera;
    public GameObject logPrefab;
    public bool ShellStatus;
    private InputSystem.PlayerActions _input;

    [Space(10)]
    [Header("=====TestBed=====")]
    public string currentAddress = "124.0.0.1";
    public int currentPort = 5454;
    public bool ConectionStatus = false;

    [Space(10)]

    //Ui members
    public Transform ServerShellRoot;
    public Transform GameLogsHolder;
   
    [Space(10)]
    public Button SpawnPlayerBtn;

    //EpsFundation methods start
    void LogInfo(string message)
    {
        TMPro.TextMeshProUGUI text =  GameObject.Instantiate(logPrefab, GameLogsHolder).GetComponent<TMPro.TextMeshProUGUI>();
        text.SetText("[INFO]  " + message);
    }

    void RenderShell(bool visible)
    {
        ServerShellRoot.gameObject.SetActive(visible);
        _input.cursorInputForLook = !visible;
        _input.cursorLocked = !visible;
        //Health.enabled = !visible;
    }
    
    private void OnTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
    {
        var warning = "OnTransportEvent: " + eventType;
        Debug.LogWarning(warning);
        LogInfo(warning);
    }

    void ConfigureNetworking()
    {
        networkManager.OnClientConnectedCallback += (ulong id) =>
        {
            if (networkManager.IsClient)
            {
                string welcomeMessage = string.Format("Hello : {0} :3 (player connected)", id.ToString());
                LogInfo(welcomeMessage);
                //RenderShell(false);
                EnableLobbyCamera(false);
                ConectionStatus = true;
            }
            else
            {
                LogInfo("Not a client");
                ConectionStatus = false;
            }
        };

        var transport = networkManager.transform.GetComponent<UNetTransport>();
        transport.OnTransportEvent += OnTransportEvent;
    }



    private void EnableLobbyCamera(bool enable)
    {
        LobbyCamera.SetActive(enable);
    }

    //EpsFundation methods end

    private void SpawnTestBedPlayer() 
    {
    
    }

    private void KillTestBedPlayer() 
    {
    
    }

    private void BootsrapMatch() 
    {
       // matchManager.StartMatch();
        
        //Create empty game mode
        //Spawn match
        //Handle everything overther    
    }

    /// <summary>
    /// Some of the procedures here will be constructed in the base clase to reduce code volume
    /// </summary>
    void Start()
    {
        networkManager = NetworkManager.Singleton;
        gameNetworking = ServiceInjector.getSingleton<Networking>();
        gameSession = ServiceInjector.getSingleton<EPS.Core.GameSession>();

        //gameMode = ServiceInjector.getSingleton<GameMode>();
        var inputService = EPS.ServiceInjector.getSingleton<EPS.Core.InputService>();
        _input = inputService.GetInputActions();

        SpawnPlayerBtn.onClick.AddListener(() => {
            gameSession.Host("TEST_BED");
        });

        ConfigureNetworking();
        LogInfo("Testing match ready... connections can join to 124.0.0.1:5454");
    }

    void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            RenderShell(ShellStatus);
            ShellStatus = !ShellStatus;
        }
    }
}
