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
            Body.drag = 0.05f;
            throw new System.NotImplementedException();
        }
        
        protected virtual  void HandleCharacteristics()
        {
            throw new System.NotImplementedException();
        }

    }
}
