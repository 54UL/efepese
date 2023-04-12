using EPS.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public EPS.DependencyManager DependencySystem = new EPS.DependencyManager();
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
        //Dependecy system initialization
        EPS.ServiceInjector.SetDependencyManager(DependencySystem);
        //EPS.DependencyManager.RegisterService<EPS.GameMode>();
        
        //EFPS BETA 1.0
        EPS.DependencyManager.RegisterService<InputService>();
        EPS.DependencyManager.RegisterService<Networking>();
        EPS.DependencyManager.RegisterService<MatchManager>();
        EPS.DependencyManager.RegisterService<GameSession>();
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

    //ALL GAME CODE IS INITIALIZED BELOW (THIS IS THE ENTRY POINT OF ALL SERVICES)
    void Awake()
    {
        InitializeSystems();
    }

    void Start()
    {
        // XRSettings.enabled = false;
        DontDestroyOnLoad(this);
        DependencySystem.ServicesOnInit();
        DispatchCourrutines();
    }

    // Update is called once per frame
    void Update()
    {
        DependencySystem.ServiceLoop();
    }

    // Notify on application quit
    void OnApplicationQuit()
    {
        DependencySystem.ShutDown();
    }
}
