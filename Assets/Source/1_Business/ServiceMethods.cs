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
            return scaleObject.x > scaleObject.y ? scaleObject.x : scaleObject.y;
        }
    }
}
