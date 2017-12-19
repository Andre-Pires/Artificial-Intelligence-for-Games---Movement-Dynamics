using System;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement
{
    public class KinematicData : StaticData
    {
        public Vector3 velocity;
        public float rotation;

        public KinematicData()
        {
            this.velocity = Vector3.zero;
            this.rotation = 0;
        }

        public KinematicData(StaticData loc) : base(loc.position,loc.orientation)
        {
            this.velocity = Vector3.zero;
            this.rotation = 0;
        }

        public KinematicData(StaticData loc, Vector3 velocity) : base(loc.position,loc.orientation)
        {
            this.velocity = velocity;
            this.rotation = 0;
        }

        public KinematicData(Vector3 position, Vector3 velocity, float orientation, float angularVelocity) : base(position, orientation)
        {
            this.velocity = velocity;
            this.rotation = angularVelocity;
        }

        public override void Clear()
        {
            base.Clear();
            this.velocity = Vector3.zero;
            this.rotation = 0;
        }

        public void Integrate(float duration)
        {
            this.position.x += this.velocity.x * duration;
            this.position.y += this.velocity.y * duration;
            this.position.z += this.velocity.z * duration;
            this.orientation += this.rotation * duration;
            this.orientation = this.orientation % MathConstants.MATH_2PI;
        }

        public override void Integrate(MovementOutput movement, float duration)
        {
            this.Integrate(duration);
            this.velocity.x += movement.linear.x*duration;
            this.velocity.y += movement.linear.y*duration;
            this.velocity.z += movement.linear.z*duration;
            this.rotation += movement.angular*duration;
        }

        public void Integrate(MovementOutput steering, float drag, float duration)
        {
            this.Integrate(duration);

            var totalDrag = (float)Math.Pow(drag, duration);

            this.velocity *= totalDrag;
            this.rotation *= drag*drag;

            this.velocity.x += steering.linear.x * duration;
            this.velocity.y += steering.linear.y * duration;
            this.velocity.z += steering.linear.z * duration;
            this.rotation += steering.angular * duration;
        }

        public void Integrate(MovementOutput steering, MovementOutput drag, float duration)
        {
            this.Integrate(duration);

            this.velocity.x *= (float)Math.Pow(drag.linear.x, duration);
            this.velocity.y *= (float) Math.Pow(drag.linear.y, duration);
            this.velocity.z *= (float) Math.Pow(drag.linear.z, duration);
            this.rotation *= (float)Math.Pow(drag.angular, duration);

            this.velocity.x += steering.linear.x * duration;
            this.velocity.y += steering.linear.y * duration;
            this.velocity.z += steering.linear.z * duration;
            this.rotation += steering.angular * duration;
        }

        public void TrimMaxSpeed(float maxSpeed)
        {
            if (this.velocity.sqrMagnitude > maxSpeed*maxSpeed) {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
        }

        public void SetOrientationFromVelocity()
        {
            base.SetOrientationFromVelocity(this.velocity);
        }
    }
}
