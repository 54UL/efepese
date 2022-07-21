using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TEST DESCRIPTION : SLICEMENT OF THE SCREEN AND PLAYER SPAWNING (WITHOUT BUSSINES LOGIC)
public class NetworkMatch : MonoBehaviour
{
    //Clasic vars
    public Transform VehicleSpawnPoint;
    public int MaxVehiclesToSpawn;
    public bool ConectionStatus = false;
    
    //Ui members
    public Transform ServerShellRoot;
    public Transform MatchesHolder;
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
    public Text PingText;

    //DEPENDENCIES
    // public DAC.IInputManager InputManagerService;
    // public DAC.IPlayerManager PlayerManager;
    // public DAC.IGameMode GameModeService;
    // public DAC.Foundation.INetClient NetClient;
    // public DAC.Foundation.ClientInfoArgs clientInfo;

    void joinMatch(int matchId)
    {
        Debug.Log("match id :" + matchId);
    }

    void ConfigServerFields()
    {
        // this.NetClient.SetClientPort(int.Parse(this.ClientPortInputField.text));
        // this.NetClient.SetPort(int.Parse(this.ServerPortInputField.text));
        // this.NetClient.SetServerIP(this.ServerIpInputField.text.Trim());
    }

    void GetPoolList()
    {
        // this.NetClient.GetActivePools((DAC.Foundation.PoolInfo poolInfo) =>
        // {
        //     foreach (var netMatch in poolInfo.pools)
        //     {
        //         var uiElement = GameObject.Instantiate(matchUIdelegate);
        //         uiElement.transform.parent = MatchesHolder;
        //         uiElement.transform.GetChild(0).GetComponent<Text>().text = netMatch.PoolName;
        //         uiElement.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { joinMatch(netMatch.PoolId); });
        //     }
        // });
    }


    void RenderShell(bool visible)
    {
        ServerShellRoot.gameObject.SetActive(visible);
    }

    void ToggleConnectAction()
    {
        if (!ConectionStatus)
        {
            ConfigServerFields();
        //     this.NetClient.Connect((DAC.Foundation.ClientInfoArgs clientInfoData) =>
        //    {
        //        clientInfo = clientInfoData;
        //        this.ToggleConnect.GetComponentInChildren<Text>().text = "Disconnect";
        //        ConectionStatus = true;
        //        GetPoolList();
        //        RenderShell(false);
        //    });
        }
        else
        {
            this.ToggleConnect.GetComponentInChildren<Text>().text = "Connect";
            // this.NetClient.Disconnect();
            ConectionStatus = false;
        }
    }

    void RefreshListAction()
    {

    }

    void CreateMatchAction()
    {

    }

    //PLAYER SPAWNING TEST
    void Start()
    {
        //Injections
        // InputManagerService = (DAC.IInputManager)ServiceInjector.Inject("DAC.InputManager");
        // PlayerManager = (DAC.IPlayerManager)ServiceInjector.Inject("DAC.PlayerManager");
        // GameModeService = (DAC.IGameMode)ServiceInjector.Inject("DAC.GameMode");
        // NetClient = (DAC.Foundation.INetClient)ServiceInjector.Inject("DAC.Foundation.BinaryNetClient");
        SpawnPlayerBtn.onClick.AddListener(() => { this.SpawnLocalPlayer(); });
        // PlayerManager.SetSpawnPoint(VehicleSpawnPoint);
        // Debug.Log("MATCH SETTED TO FREE MODE");
        // this.GameModeService.SetMatchType(DAC.MatchType.FREE);
        this.ToggleConnect.onClick.AddListener(() => { ToggleConnectAction(); });

        // this.NetClient.OnSpawn((DAC.Foundation.SpawnArgs spawnArgs) =>
        // {
        //     SpawnNetworkPlayer(spawnArgs.PlayerId);
        // });
    }


    private void SpawnNetworkPlayer(int playerId)
    {
        // NetworkPlayer instacedControl = Instantiate(new GameObject("PLAYER:network")).AddComponent<NetworkPlayer>();
        // instacedControl.NetData.Id = playerId;
        // PlayerManager.InstancePlayer(instacedControl, DAC.PlayerType.NETWORK);
    }

    public void SpawnLocalPlayer()
    {
        // KeyboardController instacedControl = Instantiate(new GameObject("LOCAL_PLAYER")).AddComponent<KeyboardController>();
        // PlayerManager.InstancePlayer(instacedControl, DAC.PlayerType.LOCAL);
        // instacedControl.LocalPlayerId = clientInfo.ClientId;
        // instacedControl.NetClientData.Id = clientInfo.ClientId;
        // var spawnArgs = new DAC.Foundation.SpawnArgs()
        // {
        //     PrefabName = "VEHICLE",
        //     PlayerId = clientInfo.ClientId
        // };
        // NetClient.SpawnObject(spawnArgs);
    }

    // NETWORK MATCH TEST
    bool ShellStatus;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RenderShell(ShellStatus);
            ShellStatus = !ShellStatus;
        }
        // this.PingText.text = "PING:" + (NetClient.GetUpssertLatency()*1000).ToString("f2")+"s";
    }
}
