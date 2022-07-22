using UnityEngine;
using EPS;

public class TemplateInjector : MonoBehaviour 
{
    public EPS.IGameMode gameModeService;

    void Start()
    {
        //this.gameModeService = (EPS.IGameMode) EPS.ServiceInjector.Inject("EPS.GameMode"); <- OLD WAY OF INJECTING THINGS
        this.gameModeService = ServiceInjector.getSingleton<EPS.GameMode>();

        Debug.LogError("game mode servicie is null?" + (this.gameModeService == null).ToString());
    }
}