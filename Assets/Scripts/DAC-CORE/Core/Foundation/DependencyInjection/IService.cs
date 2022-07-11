using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAC
{
	namespace Foundation
	{
		public interface IService
		{
			string ReferencedName();
		    void OnInit(DependencyManager manager);
			void Loop();
			void OnDestroy();
			void OnReset();
			System.Func<IEnumerator> LoopCourrutine();
		}
	}
}


