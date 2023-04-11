using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPS.Api;

namespace EPS.Core
{
    public interface IUIManager
    {
        void HideTop(StackingLevel level);
        void DiscardView(string viewName);
        void PushView(string viewName);
        void RegisterView(StackingLevel level, IUIView view, bool isMain = false); 
    }
}


