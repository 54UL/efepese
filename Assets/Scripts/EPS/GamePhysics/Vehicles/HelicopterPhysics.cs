using EPS.GamePhysics.Core;

namespace EPS.GamePhysics.Vehicles
{
    public class HelicopterPhysics : ControlledRbInputControllerBase
    {
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleCharacteristics();
        }

        private void HandleEngines()
        {
            throw new System.NotImplementedException();
        }
        
        protected virtual  void HandleCharacteristics()
        {
            throw new System.NotImplementedException();
        }

    }
}
