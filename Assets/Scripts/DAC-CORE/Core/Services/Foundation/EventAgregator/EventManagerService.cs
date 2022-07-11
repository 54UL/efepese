using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAC.Foundation;
namespace DAC
{
    namespace CORE
    {
        public class EventManagerService : IEventManagerService, IService
        {
            public List<IEventHandler> handlers = new List<IEventHandler>();
            public Queue<IEvent> eventQueue = new Queue<IEvent>();

            public List<GameObject> target_listeners = new List<GameObject>();




            public EventManagerService()
            {
             
            }

            //IService
            public string ReferencedName()
            {
                return this.GetType().ToString();
            }

            public System.Func<IEnumerator> LoopCourrutine()
            {
                return null;
            }

            public void OnInit(DependencyManager manager)
            {
                //target_listeners.Add(GameObject.Instantiate())
            }

            public void Loop()
            {
                TickNotificationQueue();
            }
            public void OnDestroy()
            {

            }
            public void OnReset()
            {

            }

            public void TickNotificationQueue()
            {
                if (eventQueue.Count == 0)
                    return;

                IEvent activeEvent = eventQueue.Dequeue();
             
                //if an event matches in the event register, call that handler to pass the notification
                List<IEventHandler> currentHandlers = handlers.FindAll((IEventHandler e) => { return e.HandlerName() == activeEvent.HandlerName(); });
                foreach (IEventHandler activeHandler in currentHandlers)
                {
                    //Multi trhead dispatching or corutine callback ;TO DO : DECIDE...
                    activeHandler.OnEventPublished((Object)activeEvent);
                }
            }

            //IEventManagerService
            public void Subscribe(IEventHandler handler)
            {
                handlers.Add(handler);
            }

            public void Unsubscribe(IEventHandler handler)
            {
                handlers.Remove(handler);
            }

            public void Publish(IEvent e)
            {
                eventQueue.Enqueue(e);
            }
        }
    }
}

