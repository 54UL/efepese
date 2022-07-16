using UnityEngine;

namespace EPS.GamePhysics.Core
{
    public class RbPhysicsBase : MonoBehaviour
    {
        protected Rigidbody Body;
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            Body = GetComponent<Rigidbody>();
        }

        // Update guarantees synchronization with the physics engine
        void FixedUpdate()
        {
            if (Body)
            {
                HandlePhysics();
            }
        }
        
        // Child classes should implement _rigidbody physics here to be executed on FixedUpdate
        protected virtual void HandlePhysics() {}
    }
}
