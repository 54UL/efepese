using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Intersection : MonoBehaviour
{
    public bool isSelected;
    public Transform[] StreetBlocks;
    public Intersection[] NearbyPoints;
    public IrregularPath IrregularPathPoint; // OPCIONAL Y DEBERIA DE SER PLURAL(ARREGLO), pero la ciudad mantiene una sencilles de un tramo irregular entre dos puntos.
    public GameObject RutePointIndicator; //Visual representation of an rute point in the editor
}
