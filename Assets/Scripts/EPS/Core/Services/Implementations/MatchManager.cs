using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;
using EPS.Api;

namespace EPS.Core.Services.Implementations
{
    public class MatchManager : IMatchManager, Foundation.IService
    {
        private IGameMode   CurrentGameMode { get; set; }
        private INetworking Network { get; set; }
        private IUIManager  UIManager { get; set; }
        private MatchType   CurrentMatchType { get; set; }

        //Match manager
        public void SetGameMode(IGameMode gameMode)
        {
            CurrentGameMode = gameMode;
        }

        public void EndGame()
        {
            CurrentGameMode.EndGame();
            CurrentGameMode = null;
        }

        public void OnEnGameForced()//add exception message
        {
            CurrentGameMode.ForceEnd();
            CurrentGameMode = null;
        }

        private void SpawnPlayers()
        {
            //netcode logic to  spawn player prefabs (server side)
            //Add netwkork player OnNetworkSpawn logic
            CurrentGameMode.OnStartGame();
        }

        private void KillPlayer()
        {

        }

        private void RespawnPlayer()
        {

        }

        public void SendPlayerInputs()//Player inputs: Vector3 movement, Vector3 rotation, Quaternion aimOrentation, Vector3 inputDirection
        {

        }

        public void SetMatchType(MatchType matchType)
        {
            CurrentMatchType = matchType;
        }

        public MatchType GetMatchType()
        {
            return MatchType.EMPTY;
        }

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