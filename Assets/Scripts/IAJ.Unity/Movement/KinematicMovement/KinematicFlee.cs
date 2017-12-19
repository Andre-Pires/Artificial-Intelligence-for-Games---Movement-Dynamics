using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicFlee : TargetedKinematicMovement
    {
        public override string Name
        {
            get { return "Flee"; }
        }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            output.linear = this.Character.position - this.Target.position;

            if (output.linear.sqrMagnitude > 0)
            {
                output.linear.Normalize();
                output.linear *= this.MaxSpeed;
            }

            return output;
        }
    }
}
