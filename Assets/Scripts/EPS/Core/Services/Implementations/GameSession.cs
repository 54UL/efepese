using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;

namespace EPS.Core
{
    public class GameSession: IGameSession, EPS.Foundation.IService
    {
        private Networking gameNetworking;
        private IMatchManager MatchManager { get; set; }

        //GameSession


        void InitializeTestBedHost() 
        {
            gameNetworking.Start(NetworkSytemType.HOST, "124.0.0.1", 5454);
            var emptyMode = new EmptyMode();
            
            //MatchManager.StartMatch(emptyMode); NOT NEEDED UNTIL MANUAL SPAWN
            // matchManager.StartMatch();

            //Use an empty mode
            //Spawn match
            //Handle everything overther  
        }

        void InitializeHost() 
        { 

        }

        public bool Host(string GameName)
        {
            if (GameName == "TEST_BED")
            {
                InitializeTestBedHost();
            }
            else
            {
                InitializeHost();
            }
            //Start networking systems
            //Start Match systems fro creating a match
            //Show spawn screen UI

            return false;
        }

      


        public bool Join(uint id)
        {
            //Start networking systems
            //Start Match systems for joining a match
            //Show spawn screen
            return false;
        }
        
        
        //IService
        public void OnInit(DependencyManager manager)
        {
            gameNetworking = ServiceInjector.getSingleton<Networking>();
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