using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace LSRI.AuditoryGames.GameFramework
{
    public class ResourcePool<T> where T : BaseObject, new()
    {
        protected List<T> objects = new List<T>();

        public T UnusedObject
        {
            get
            {
                foreach (T baseObject in objects)
                {
                    if (!baseObject.InUse)
                        return baseObject;
                }

                T newObject = new T();
                objects.Add(newObject);
                return newObject;
            }
        }
        
        public ResourcePool()
        {
            
        }
    }
}
