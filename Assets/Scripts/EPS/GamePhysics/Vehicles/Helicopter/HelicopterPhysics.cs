using System;
using System.Collections.Generic;
using EPS.GamePhysics.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace EPS.GamePhysics.Vehicles.Helicopter
{
    public class HelicopterPhysics : ControlledRbInputControllerBase
    {
        [FormerlySerializedAs("Engines")] [Header("Helicopter Properties")] public List<EnginePhysics> engines = new List<EnginePhysics>();
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleCharacteristics();
        }

        private void HandleEngines()
        {
            float totalPower = 0.0f;
            foreach (EnginePhysics engine in engines)
            {
                engine.UpdateEngine(input.jump);
                totalPower += engine.CurrentHorsePower;
                Debug.Log(totalPower);
            }
        }
        
        protected virtual  void HandleCharacteristics()
        {
            
        }

    }
}
