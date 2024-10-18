using Constellation.Module_1.Data;
using UnityEngine;

namespace Constellation.Module_1.Utils
{
    public static class EquatorialMath
    {
        private const float DefaultRadius = 1f;
        private const float OneHourDegree = 15f;
        private const float HoursInDay = 24f;


        public static Vector3 ToVector3(EquatorialCoords coords, float radius = DefaultRadius)
        {
            return ToVector3(coords.Ra, coords.Dec, radius);
        }
        public static Vector3 ToVector3(float ra, float dec, float radius = DefaultRadius)
        {
            var raRadians = ra * OneHourDegree * Mathf.Deg2Rad;
            var decRadians = dec * Mathf.Deg2Rad;

            return new Vector3(
                radius * Mathf.Cos(decRadians) * Mathf.Cos(raRadians),
                radius * Mathf.Sin(decRadians),
                radius * Mathf.Cos(decRadians) * Mathf.Sin(raRadians));
        }

        public static EquatorialCoords ToEquatorialCoords(Vector3 position)
        {
            var mag = position.magnitude;

            float decRadians = Mathf.Asin(position.y / mag);

            float raRadians = Mathf.Atan2(position.z, position.x);

            float decDegree = decRadians * Mathf.Rad2Deg;
            float raDegree = raRadians * Mathf.Rad2Deg;
            raDegree /= OneHourDegree;

            //check is in day time hours within 0-24hrs
            if (raDegree < 0)
            {
                raDegree += HoursInDay;
            }

            return new EquatorialCoords(raDegree, decDegree);
        }
    }
}