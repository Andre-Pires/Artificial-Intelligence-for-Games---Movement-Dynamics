using System;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement
{
    public class StaticData
    {
        public Vector3 position;
        public float orientation;

        public StaticData()
        {
            this.Clear();
        }

        public StaticData(Vector3 position)
        {
            this.position = position;
            this.orientation = 0;
        }

        public StaticData(Vector3 position, float orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }

        public virtual void Clear()
        {
            this.position = Vector3.zero;
            this.orientation = 0;
        }

        public virtual void Integrate(MovementOutput movement, float duration)
        {
            this.position.x +=  movement.linear.x*duration;
            this.position.y += movement.linear.y*duration;
            this.position.z += movement.linear.z*duration;
            this.orientation += movement.angular*duration;
            this.orientation = this.orientation%MathConstants.MATH_2PI;
        }

        public void ApplyWorldLimit(float xWorldSize, float zWorldSize)
        {
            if (this.position.x < -xWorldSize)
            {
                this.position.x = xWorldSize;
            }
            else if (this.position.x > xWorldSize)
            {
                this.position.x = -xWorldSize;
            }
            if (this.position.z < -zWorldSize)
            {
                this.position.z = zWorldSize;
            }
            else if (this.position.z > zWorldSize)
            {
                this.position.z = -zWorldSize;
            }
        }
       

        /**
         * Sets the orientation of this position so it points along
         * the given velocity vector.
         */

        public void SetOrientationFromVelocity(Vector3 velocity)
        {
            // If we haven't got any velocity, then we can do nothing.
            if (velocity.sqrMagnitude > 0)
            {
                this.orientation = MathHelper.ConvertVectorToOrientation(velocity);
            }
        }

        /**
         * Returns a unit vector in the direction of the current
         * orientation.
         */

        public Vector3 GetOrientationAsVector()
        {
            return MathHelper.ConvertOrientationToVector(this.orientation);
        }
    }
}
