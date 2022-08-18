using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;

namespace EPS.Core.Services.Implementations
{
    public class GameSession : EPS.Foundation.IService
    {   
        //IService
        public void OnInit(DependencyManager manager)
        {
            // _inputActions = Object.FindObjectOfType<PlayerActions>();

        }

        public void Loop()
        {

        }

        public void OnDestroy()
        {
            //throw new NotImplementedException();
        }

        public void OnReset()
        {
            //throw new NotImplementedException();
        }

        public Func<IEnumerator> LoopCourrutine()
        {
            return null;
        }

        //GameSession
        
        //Get players info from somewhere
        public void StartGame();//GAME LOGIC ENTRY POINT

        public void HostSession();
        public void JoinSession();

        public void StartMatch();
        public void EndMatch();
        public void SoftRestart();//Returns info about the ended session
        public void HardRestart();

        //Match manager
        public void SetGameMode();
        public void MatchStarted();
        public void SpawLocalPlayer();//Use some player settings like weapon attachements and weapon settings
        public void SpawnPlayer();
        public void KillPlayer();
        public void SpawnPlayers();//Called once.. (network manager..?)
        public double GetMatchDuration();
        public void TickMatchClock();
        public void OnStartMatch(system.action callback);// Add this as event
    }
}