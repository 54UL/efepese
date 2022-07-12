using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS;

public class HolaMundo : MonoBehaviour
{
    [Range(0,100)]
    public float XD;
    public IGameMode gamemodeService;

    // Start is called before the first frame update
    void Start()
    {
        this.gameModeService = ServiceInjector.getSingleton<EPS.GameMode>();
        Debug.LogError("game mode servicie is null?" + (this.gameModeService == null).ToString());
    }
       
    // Update is called once per frame
    void Update()
    {
        
    }
}
