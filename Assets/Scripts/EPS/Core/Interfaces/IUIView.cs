using UnityEngine;

namespace EPS.UI
{
    public interface IUIView
    {
        void OnClose();
        void OnShow();
        GameObject ViewParent(); 
    }
}
