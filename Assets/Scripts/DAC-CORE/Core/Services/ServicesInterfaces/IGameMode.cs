using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAC.CORE;
//using Rewired;





namespace DAC
{
    public interface IGameMode
    {
       void SetGameMode(GameType gameMode); 
       GameType GetGameMode();
       void SetMatchType(MatchType matchType);
       MatchType GetMatchType();
    }
}


