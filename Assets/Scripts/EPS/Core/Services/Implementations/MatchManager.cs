using System;
using System.Collections;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using InputSystem;

namespace EPS.Core.Services.Implementations
{
    public class MatchManager : EPS.Foundation.IService
    {   
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
    }
}