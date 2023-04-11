using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EPS.Api;
using System;
using EPS.Foundation;


//FUTURE TODOS:QUITAR TODO LO RELACIONADO CON EL VIEW STACK Y DEJARLO EN UN STACK PARA SIEMPRE(TODA LA APP)
namespace EPS.Core
{
    //REFACTOR MODELS
    public class UIManager : IUIManager, Foundation.IService
    {
        //IUIManager
        Dictionary<StackingLevel, List<IUIView>> registeredViews;
        Dictionary<StackingLevel, Stack<IUIView>> viewsStack;
        bool isTopHidden = false; //WTFf???? no se usa considerar descartar

        public UIManager()
        {

        }

        public void HideTop(StackingLevel level)
        {
            //HIDE THE TOP AND CALL CLOSE
            var peek = this.viewsStack[level].Peek();
            peek.ViewParent().SetActive(false);
            peek.OnClose();
            isTopHidden = true;
        }

        // a b c e d CARRERA
        // A B C NIVEL

        public void DiscardView(string viewName)
        {
            throw new System.NotImplementedException();
        }

        public void PopView(StackingLevel level)
        {

            Stack<IUIView> viewStack;
            if (viewsStack.TryGetValue(level, out viewStack))
            {
                var view = viewStack.Pop();
                if (view != null)
                {
                    view.ViewParent().SetActive(false);
                    try
                    {
                        Debug.Log("[EPS::UIManager] poping view with name :" + view.ViewParent().name);

                        view.OnClose();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("[UI MANAGER]:Not Implemented: " + e.Message);
                    }
                }
                viewStack.Peek().ViewParent().SetActive(true);
            }

        }

        private void setStackingPosAndShow(GameObject uiElement, int stackPos)
        {
            if (stackPos > -1)
            {
                var uiTransform = uiElement.GetComponent<RectTransform>();
                uiTransform.position = new Vector3(uiTransform.position.x, uiElement.transform.position.y, stackPos);
                uiElement.SetActive(true);
            }
        }

        public void PushView(string viewName)
        {
            StackingLevel currentViewStackLevel = StackingLevel.NONE;
            Stack<IUIView> currentStackLevelList;
          
            Debug.Log("[DAC::UIManager] pushing view with name :" + viewName);
            IUIView viewToBeingPushed = registeredViews.Select((viewPair) =>
                 {
                     var viewFinded = viewPair.Value.Find((e) =>
                         {
                             currentViewStackLevel = viewPair.Key;
                             return e.ViewParent().name == viewName;
                         });
                     return viewFinded;
                 }).FirstOrDefault();

            if (viewsStack.TryGetValue(currentViewStackLevel, out currentStackLevelList))
            {
                var currentPeek  =  currentStackLevelList.Peek();
               currentPeek.ViewParent().SetActive(false);
               currentPeek.OnClose();
            }
            
            if (viewToBeingPushed != null)
            {
                //Active view, and asign them a stacking pos.
                if (viewsStack.TryGetValue(currentViewStackLevel, out currentStackLevelList))
                {
                    currentStackLevelList.Push(viewToBeingPushed);
                }
                else
                {
                    currentStackLevelList = new Stack<IUIView>();
                    currentStackLevelList.Push(viewToBeingPushed);
                    viewsStack.Add(currentViewStackLevel, currentStackLevelList);
                }
                setStackingPosAndShow(viewToBeingPushed.ViewParent(), currentStackLevelList.Count());
                try
                {
                    viewToBeingPushed.OnShow();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[UI MANAGER]:Not Implemented:" + e.Message);
                }
            }
            else
                Debug.LogError("[UI MANAGER]:VIEW NOT FOUND NAME:" + viewName);
        }

        public void RegisterView(StackingLevel level, IUIView view, bool isMain = false)
        {
            List<IUIView> views;

            if (registeredViews.TryGetValue(level, out views))
            {
                views.Add(view);
            }
            else
            {
                views = new List<IUIView>();
                views.Add(view);
                registeredViews.Add(level, views);
            }
            if (isMain)
            {
                this.PushView("MainMenuUI");
            }
            view.ViewParent().SetActive(isMain);
        }

        //IService
        public string ReferencedName()
        {
            return this.GetType().ToString();
        }

        public void Loop()
        {
            // throw new System.NotImplementedException();
        }

        public void OnDestroy()
        {
            //throw new System.NotImplementedException();
        }

        public void OnInit(DependencyManager manager)
        {
            registeredViews = new Dictionary<StackingLevel, List<IUIView>>();
            viewsStack = new Dictionary<StackingLevel, Stack<IUIView>>();
            // throw new System.NotImplementedException();
        }

        public void OnReset()
        {
            //throw new System.NotImplementedException();
        }

        Func<IEnumerator> IService.LoopCourrutine()
        {
            return null;
        }
    }
}