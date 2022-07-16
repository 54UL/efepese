using UnityEngine;

namespace EPS.GamePhysics.Core
{
    public class ControlledRbInputControllerBase : InputControllerBase
    {
        protected Rigidbody Body;
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Body = GetComponent<Rigidbody>();
        }

        // Update guarantees synchronization with the physics engine
        void FixedUpdate()
        {
            if (Body  && input)
            {
                HandlePhysics();
            }
        }
        
        // Child classes should implement _rigidbody physics here to be executed on FixedUpdate
        protected virtual void HandlePhysics() {}
    }
}
