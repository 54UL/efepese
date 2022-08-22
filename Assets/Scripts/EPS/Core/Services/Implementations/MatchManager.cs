using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;

namespace EPS.Core.Services.Implementations
{
    public class MatchManager: EPS.Foundation.IService
    {   

        //Match manager
        public void SetGameMode(IGameMode gameMode)
        {

        }

       public void EndGame(IGameMode gameMode)
       {

       }

        public void OnEnGameForced()//add exception message
        {

        }

        private void SpawnPlayers()
        {
            //netcode logic to  spawn player prefabs (server side)
           //Add netwkork player OnNetworkSpawn logic
        }
        
        private void KillPlayer()
        {

        }

        private void RespawnPlayer()
        {

        }

        private void SendPlayerInputs()//Player inputs: Vector3 movement, Vector3 rotation, Quaternion aimOrentation, Vector3 inputDirection
        {

        }

        private void 

        //IService
        public string ReferencedName()
        {
            throw new NotImplementedException();
        }

        public void OnInit(DependencyManager manager)
        {
            throw new NotImplementedException();
        }

        public void Loop()
        {
           
        }

        public void OnDestroy()
        {
           
        }

        public void OnReset()
        {
           
        }

        public Func<IEnumerator> LoopCourrutine()
        {
            return null;
        }
    }
}