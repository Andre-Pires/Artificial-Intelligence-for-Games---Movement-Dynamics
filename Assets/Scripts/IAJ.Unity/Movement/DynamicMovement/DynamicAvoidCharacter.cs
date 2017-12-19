using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidCharacter : DynamicMovement
	{
		public override string Name {
			get { return "AvoidCharacter"; }
		}

		public float MaxTimeLookAhead  { get; set; }
		public float CollisionRadius  { get; set; }
		public override KinematicData Target { get; set; }

		public DynamicAvoidCharacter (KinematicData target)
		{
			Target = target;
		}

		public override MovementOutput GetMovement ()
		{
			var output = new MovementOutput ();

			Vector3 deltaPos = Target.position - Character.position;
			Vector3 deltaVel = Target.velocity - Character.velocity;
			float deltaSpeed = deltaVel.magnitude;

			if (deltaSpeed == 0) {
				return output;
			}

			float timeToClosest = - Vector3.Dot (deltaPos, deltaVel) / (deltaSpeed * deltaSpeed);

			if (timeToClosest > MaxTimeLookAhead) {
				return output;
			}

			float distance = deltaPos.magnitude;
			float minSeparation = distance - deltaSpeed * timeToClosest;

			if (minSeparation > 2 * CollisionRadius) {
				return new MovementOutput ();
			}

			if (minSeparation <= 0 || distance < 2 * CollisionRadius) {
				output.linear = Character.position - Target.position;
			} else {
				output.linear = (deltaPos + deltaVel * timeToClosest) * -1;
			}

			output.linear = output.linear.normalized * MaxAcceleration;

			return output;
		}
	}
}
