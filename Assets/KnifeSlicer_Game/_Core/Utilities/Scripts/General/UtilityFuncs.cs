using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace KnifeSlicer.Utilities
{
    public static class UtilityFuncs
    {
        public static T[] FindObjectsInScene<T>(bool findNotActive = false) where T : class
        {
            var searchType = findNotActive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            return Object
                .FindObjectsByType(typeof(T), searchType, FindObjectsSortMode.None)
                .FindObjectsOfInterface<T>();
        }

        public static T FindInterfaceInScene<T>(bool findNotActive = false) where T : class
        {
            var searchType = findNotActive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            return Object
                .FindObjectsByType(typeof(T), searchType, FindObjectsSortMode.None)
                .FindInterfaceInScene<T>();
        }
        public static List<T> FindInterfacesInScene<T>(bool includeInactive = false) where T : class
        {
            var searchType = includeInactive
                ? FindObjectsInactive.Include
                : FindObjectsInactive.Exclude;

            // grab every MonoBehaviour and filter by interface
            return Object
                .FindObjectsByType<MonoBehaviour>(searchType, FindObjectsSortMode.None)
                .OfType<T>()
                .ToList();
        }
    }

    public static class ObjectExtensions
    {
        public static T[] FindObjectsOfInterface<T>(this Object[] objects) where T : class
        {
            List<T> interfaceObjects = new List<T>();
            foreach (Object obj in objects)
            {
                if (obj is T)
                    interfaceObjects.Add(obj as T);
            }

            return interfaceObjects.ToArray();
        }

        public static T FindInterfaceInScene<T>(this Object[] objects) where T : class
        {
            foreach (Object obj in objects)
            {
                if (obj is T)
                    return obj as T;
            }

            return null;
        }

        public static T FindInterfaceInScene<T>() where T : class
        {
            return Object.FindObjectsByType(typeof(T), FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FindInterfaceInScene<T>();
        }
    }
}