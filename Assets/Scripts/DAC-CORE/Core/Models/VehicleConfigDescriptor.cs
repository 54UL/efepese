
using UnityEngine.UI;
using UnityEngine;

//DAC-BRIEF: CLASE QUE DESCRIBE UN VEHICULO EN EL JUEGO
namespace DAC
{
    [System.Serializable]
    public class VehicleConfigDescriptor : MonoBehaviour
    {
        //Base data model
        public string VehicleName;
        public int VehicleID;
        public int VehicleLevel;
        public Sprite previewInUI;
        public GameObject VehiclePrefab;

        //Minimal setup constructor
        public VehicleConfigDescriptor(GameObject prefab)
        {
            VehicleID = -2;
            VehicleLevel = -1;
            VehiclePrefab = prefab;
        }

        //Detailed setup constructor
        public VehicleConfigDescriptor(int vehicleID, int vehicleLevel, GameObject prefab)
        {
            VehicleID = vehicleID;
            VehicleLevel = vehicleLevel;
            VehiclePrefab = prefab;
        }
    }
}

