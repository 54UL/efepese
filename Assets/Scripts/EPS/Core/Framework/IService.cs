using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPS
{
	namespace Foundation
	{
		public interface IService
		{
			string ReferencedName();
		    void OnInit(EPS.DependencyManager manager);
			void Loop();
			void OnDestroy();
			void OnReset();
			System.Func<IEnumerator> LoopCourrutine();
		}
	}
}


