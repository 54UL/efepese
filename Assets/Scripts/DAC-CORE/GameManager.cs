using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAC.CORE;
using UnityEngine.XR;



//DAC-BRIEF: CORE ENTRY POINT A.K.A: main(int argc,char** argv)

public class GameManager : MonoBehaviour
{
    //CORE SYSTEMS
    public DependencyManager DependencySystem = new DependencyManager();
    [Header("GAME-RESOURCES")]
    public GameObject[] PrefabResources;
    public List<System.Func<IEnumerator>> ServiceCourrutines;

    private void StoreGameResources()
    {
        DependencySystem.StoreResources(PrefabResources);
        Debug.Log("RESOURCES LOADED");
    }

    private void RegisterServices()
    {
        //DEPENDENCIES INITIALIZATIONS
        ServiceInjector.SetDependencyManager(DependencySystem);
        DependencyManager.RegisterService<DAC.CORE.EventManagerService>();
        //DependencyManager.RegisterService<DAC.GameMode>();
        //DependencyManager.RegisterService<DAC.InputManager>();
        //DependencyManager.RegisterService<DAC.GameSession>();
        //DependencyManager.RegisterService<DAC.PlayerManager>();
        //DependencyManager.RegisterService<DAC.UIManager>();
        //DependencyManager.RegisterService<DAC.ProgressManager>();
        //DependencyManager.RegisterService<DAC.VehicleCatalog>();
        //DependencyManager.RegisterService<DAC.PlayersVehicleConfiguration>();
        //DependencyManager.RegisterService<DAC.RaceManager>();
        //DependencyManager.RegisterService<DAC.RaceEditor>();
        //DependencyManager.RegisterService<DAC.NpcManager>();
        //DependencyManager.RegisterService<DAC.RacePlace>(); 
        //DependencyManager.RegisterService<DAC.Foundation.BinaryNetClient>();

        //EXPERIMENTAL FOUNDATION SERVICES.... 
        // DependencyManager.RegisterService<DAC.Foundation.JsonNetClient>();
        //DependencyManager.RegisterService<DAC.Foundation.BinaryNetClient>();

    }

    private void InitializeSystems()
    {
        StoreGameResources();
        RegisterServices();
        DependencySystem.InitServices();
        ServiceCourrutines = DependencySystem.GetCourrutines();
    }

    private void DispatchCourrutines()
    {
        foreach (var courrutine in ServiceCourrutines)
        {
            if (courrutine != null)
                StartCoroutine(courrutine());
        }
    }

    // Use this for initialization
    void Awake()
    {
        InitializeSystems();
    }


    void Start()
    {
        XRSettings.enabled = false;
        DontDestroyOnLoad(this);
        DependencySystem.ServicesOnInit();
        DispatchCourrutines();
    }


    // Update is called once per frame
    void Update()
    {
        DependencySystem.ServiceLoop();
    }
    void OnApplicationQuit()
    {
        DependencySystem.ShutDown();
    }
}
