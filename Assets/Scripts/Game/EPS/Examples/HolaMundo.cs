using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS;

public class HolaMundo : MonoBehaviour
{
    [Range(0,100)]
    public float XD;
    public EPS.IGameMode gamemodeService;

    // Start is called before the first frame update
    void Start()
    {
        this.gamemodeService = ServiceInjector.getSingleton<EPS.GameMode>();
        Debug.LogError("game mode servicie is null?" + (this.gamemodeService == null).ToString());
    }
       
    // Update is called once per frame
    void Update()
    {
        
    }
}
