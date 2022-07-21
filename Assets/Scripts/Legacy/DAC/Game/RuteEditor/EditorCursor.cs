using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class EditorCursor : MonoBehaviour
{
    //DEPENDENCIES
    //private DAC.IRaceEditor RaceEditor;

    // Start is called before the first frame update
    public Intersection getSelectedObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit))
        {
            Intersection selectedIntersection = null;
            if (hit.collider.transform.parent.TryGetComponent<Intersection>(out selectedIntersection))
            {
                Debug.Log("Selected game object name:" + hit.collider.gameObject.name);
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.green);
                return selectedIntersection;
            }
            else
                return null;
        }
        return null;
    }

   void Start()
   {
       //this.RaceEditor = (DAC.IRaceEditor) ServiceInjector.Inject("DAC.RaceEditor");
       //this.RaceEditor.AttachCursor(this);
   }

    void Update()
    {
      
    }
}
