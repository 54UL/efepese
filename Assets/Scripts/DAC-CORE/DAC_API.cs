using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* DEVELOMENT DOCUMENTATION AND DRAFTING

    UI Algorithms and configuration Stages:
    //Session configuration
    //Player Configuration
    //Rute Editor
*///

//AQUI VA TODO LO QUE SE CONECTE CON EL BACK Y FRONT Y SE USE EN AMBOS LADOS

namespace DAC
{
    public enum PlayerType { UNDEFINED, LOCAL, NPC, NETWORK };
    public enum GameType { UNDEFINED, SINGLE, MULTI };
    public enum MatchType { UNDEFINED, RUTE, FREE };
    public enum StackingLevel { NONE, GUI, CANVAS };
    public interface IUIView
    {
        void OnClose();
        void OnShow();
        GameObject ViewParent(); //view enabling status
    }

    public interface IPlayerHUD
    {
        void GetOwnerPlayerID();
        void SetValue(string selector, int value);
    }
}