using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EPS;
using EPS.Core;

public class MainMenuUI : MonoBehaviour, IUIView
{
    //DEPENDENCIES
    private IUIManager UiService { get; set; }

    //MEMBERS
    public Button PlayGameBtn;
    public Button OptionsBtn;
    public Button CreditsBtn;
    public Button QuitBtn;

    void Start()
    {
        UiService = ServiceInjector.getSingleton<UIManager>();
        UiService.RegisterView(StackingLevel.GUI, this, true);

        //clicked actions listeners for the buttons of the current view
        PlayGameBtn.onClick.AddListener(() => { this.UiService.PushView("LobbyView"); });
        OptionsBtn.onClick.AddListener(() => { this.UiService.PushView("GameOptionsView"); });
        CreditsBtn.onClick.AddListener(() => { this.UiService.PushView("GameCreditsView"); });
        QuitBtn.onClick.AddListener(() => { Application.Quit(); });
    }

    //IUIView
    public void OnClose()
    {
        Debug.Log("[MainMenuUI]: se ha salido del menu principal");
    }

    public void OnShow()
    {
        //GameObject.Find("CINEMATIC_CAMERA").SetActive(true);
    }

    public GameObject ViewParent()
    {
        return this.gameObject;
    }
}