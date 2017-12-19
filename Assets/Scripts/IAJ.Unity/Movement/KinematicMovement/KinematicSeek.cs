using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicSeek : TargetedKinematicMovement
    {
        public override string Name
        {
            get { return "Seek"; }
        }

        public override MovementOutput GetMovement()
        {
            MovementOutput output = new MovementOutput();

            output.linear = this.Target.position - this.Character.position;

            if (output.linear.sqrMagnitude > 0)
            {
                output.linear.Normalize();
                output.linear *= this.MaxSpeed;
            }

            return output;
        }
    }
}
