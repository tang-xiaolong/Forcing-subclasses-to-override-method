#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class MustOverrideChecker
{
    private static Dictionary<Type, List<string>> markedClasses;

    static MustOverrideChecker()
    {
        EditorApplication.update += CheckOverrides;
    }

    private static void CheckOverrides()
    {
        EditorApplication.update -= CheckOverrides;
        markedClasses = new Dictionary<Type, List<string>>();
        var startTime = DateTime.Now;

        var types = Assembly.GetExecutingAssembly().GetTypes();
        
        // Step 1: Find all marked classes and the methods they need to override
        foreach (var type in types)
        {
            var attributes = type.GetCustomAttributes<MustOverrideAttribute>();
            if (attributes.Any())
            {
                if (!markedClasses.ContainsKey(type))
                {
                    markedClasses[type] = new List<string>();
                }
                foreach (var attribute in attributes)
                {
                    markedClasses[type].Add(attribute.MethodName);
                }
            }
        }

        // Step 2: Find all subclasses and their descendants
        foreach (var markedClass in markedClasses.Keys)
        {
            foreach (var type in types)
            {
                if (IsSubclassOf(type, markedClass))
                {
                    // Step 3: Check if the subclass overrides the required methods
                    foreach (var methodName in markedClasses[markedClass])
                    {
                        var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (method == null || method.DeclaringType != type)
                        {
                            Debug.LogError(type.Name + " must override " + methodName + " method from " + markedClass.Name + ".");
                        }
                    }
                }
            }
        }
        var endTime = DateTime.Now;
        Debug.Log("MustOverrideChecker completed in " + (endTime - startTime).TotalMilliseconds + " ms");
    }

    private static bool IsSubclassOf(Type type, Type baseType)
    {
        while (type != null && type != typeof(object))
        {
            if (type == baseType)
            {
                return true;
            }
            type = type.BaseType;
        }
        return false;
    }
}
#endif