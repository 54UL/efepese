using UnityEngine;
using EPS;
public class TemplateInjector : MonoBehaviour 
{
    public EPS.Core.IUIManager uiService;

    void Start()
    {
        //this.gameModeService = (EPS.IGameMode) EPS.ServiceInjector.Inject("EPS.GameMode"); <- OLD WAY OF INJECTING THINGS
        this.uiService = ServiceInjector.getSingleton<EPS.Core.UIManager>();

        Debug.LogError(uiService.ToString() + " service is null:" + (this.uiService == null).ToString());
    }
}