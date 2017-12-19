using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicWander : KinematicMovement
    {
        public override string Name
        {
            get { return "Wander"; }
        }

        public float MaxRotation { get; set; }

        public KinematicWander()  
        {
            this.MaxRotation = 8*MathConstants.MATH_PI;
        }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            // Move forward in the current direction
            output.linear = this.Character.GetOrientationAsVector();
            output.linear *= this.MaxSpeed;

            // Turn a little
            output.angular = RandomHelper.RandomBinomial() * this.MaxRotation;

            return output;
        }
    }
}
