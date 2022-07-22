using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;

namespace EPS
{
    public class GameMode : EPS.IGameMode, EPS.Foundation.IService
    {
        //Members
        private GameType CurrentGameType;
        private MatchType CurrentMatchType;

        public GameMode()
        {

        }

        //IGameMode
        public GameType GetGameMode()
        {
            return this.CurrentGameType;
        }

        public MatchType GetMatchType()
        {
            return this.CurrentMatchType;
        }
        public void SetGameMode(GameType gameMode)
        {
            this.CurrentGameType = gameMode;
        }

        public void SetMatchType(MatchType matchType)
        {
            this.CurrentMatchType = matchType;
        }

        //IService
        public System.Func<IEnumerator> LoopCourrutine()
        {
            return null;
        }

        public void Loop()
        {
            // throw new System.NotImplementedException();
        }


        public void OnDestroy()
        {
            //throw new System.NotImplementedException();
        }

        public void OnInit(DependencyManager manager)
        {
            // throw new System.NotImplementedException(); 
            Debug.Log("GAME MODE WORKS....");
        }

        public void OnReset()
        {
            //throw new System.NotImplementedException();
        }

        public string ReferencedName()
        {
            return this.GetType().ToString();
        }
    }
}

