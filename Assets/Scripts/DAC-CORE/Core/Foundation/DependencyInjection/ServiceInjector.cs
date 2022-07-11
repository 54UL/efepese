using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DAC.Foundation;



public class ServiceInjector
{
    static private DependencyManager m_DependencyManager;

    public static void SetDependencyManager(DependencyManager ctx)
    {
        m_DependencyManager = ctx;
    }

    //IMPROVE WARNINGS AND ERROS MESSAGES (TRY CATCH?)
    public static IService Inject(string name)
    {
        if (m_DependencyManager != null )
            return m_DependencyManager.GetService(name);
        else 
        {
            Debug.LogError("DEPENDENCY MANAGER NOT FOUND");
            return null;
        }
    }

    public static T getSingleton<T>(T test)
    {
        Debug.Log(test.GetType().Name);
        return test;
    }
}
