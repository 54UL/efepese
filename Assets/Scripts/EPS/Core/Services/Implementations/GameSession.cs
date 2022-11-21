using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;

namespace EPS.Core.Services.Implementations
{
    public class GameSession: EPS.Foundation.IService
    {
        private EPS.Networking m_networking;
        private EPS.IMatchManager MatchManager { get; set; }

        //GameSession
        public bool HostSession(string GameName)
        {
            //Start networking systems
            //Start Match systems fro creating a match
            //Show spawn screen UI

            return false;
        }

        public bool JoinSession(uint id)
        {
            //Start networking systems
            //Start Match systems for joining a match
            //Show spawn screen
            return false;
        }
        
        public void SoftRestart()
        {

        }

        public void HardRestart()
        {

        }
        
        //IService
        public void OnInit(DependencyManager manager)
        {
            m_networking = ServiceInjector.getSingleton<Networking>();
            MatchManager = ServiceInjector.getSingleton<MatchManager>();
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

        public string ReferencedName()
        {
            return this.GetType().ToString();
        }
    }
}