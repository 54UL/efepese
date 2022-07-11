using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAC.CORE;

namespace DAC
{
	namespace Foundation
	{
		public interface IEventManagerService 
		{ 
		    void TickNotificationQueue();
			void Subscribe(IEventHandler handler);
			void Unsubscribe(IEventHandler handler);
			void Publish(IEvent e);
		}
	}
}


