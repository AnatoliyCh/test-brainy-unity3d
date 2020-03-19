using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Business.ServiceMethods
{
    public static class ServiceMethods
    {
        public static float PercentNumber(float percent, float number)
        {
            return percent * number / 100;
        }

        /// <summary> Возвращает радиус объекта по самой большой стороне </summary>
        public static float GetObjectRadius(Vector2 scaleObject)
        {
            return scaleObject.x / 2 > scaleObject.y / 2 ? scaleObject.x / 2 : scaleObject.y / 2;
        }

        public static bool IsСollision2D(Transform objectA, Transform objectB)
        {
            return Vector2.Distance(objectA.position, objectB.position) < GetObjectRadius(objectA.lossyScale) + GetObjectRadius(objectB.lossyScale) ? true : false;
        }

        public static bool IsСollision2D(GameObject objectA, GameObject objectB)
        {
            return IsСollision2D(objectA.transform, objectB.transform);
        }

        public static Vector3 GetMouseWorldPosition()
        {
            var pozition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pozition.z = 0;
            return pozition;
        }
    }
}
