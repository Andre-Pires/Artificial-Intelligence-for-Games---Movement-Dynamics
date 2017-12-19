using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicArrive : TargetedKinematicMovement
    {
        public override string Name
        {
            get { return "Arrive"; }
        }
        public float TimeToTarget { get; set; }
        public float Radius { get; set; }

        public KinematicArrive() 
        {
            this.TimeToTarget = 2.0f;
            this.Radius = 1.0f;
        }
        
        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            output.linear = this.Target.position - this.Character.position;

            if (output.linear.sqrMagnitude < this.Radius*this.Radius)
            {
                output.linear = Vector3.zero;
            }
            else
            {
                // We'd like to arrive in timeToTarget seconds
                output.linear *= (1.0f/this.TimeToTarget);

                // If that is too fast, then clip the speed
                if (output.linear.sqrMagnitude > this.MaxSpeed*this.MaxSpeed)
                {
                    output.linear.Normalize();
                    output.linear *= this.MaxSpeed;
                }
            }

            return output;
        }
    }
}
