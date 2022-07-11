using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DAC
{
	namespace Foundation
	{
		public interface IEventHandler{
			void OnEventPublished(Object e);
			string HandlerName();
		}
	}
}
