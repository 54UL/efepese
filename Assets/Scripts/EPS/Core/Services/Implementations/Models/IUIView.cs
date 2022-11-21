using UnityEngine;

namespace EPS
{
    public enum StackingLevel { NONE, GUI, CANVAS };
    
    public interface IUIView
    {
        void OnClose();
        void OnShow();
        GameObject ViewParent(); //view enabling status
    }
}