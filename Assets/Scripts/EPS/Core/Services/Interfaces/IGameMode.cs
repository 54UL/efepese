using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;

namespace EPS
{
    public interface IGameMode
    {
       void SetGameMode(GameType gameMode); 
       GameType GetGameMode();
       void SetMatchType(MatchType matchType);
       MatchType GetMatchType();
    }
}


