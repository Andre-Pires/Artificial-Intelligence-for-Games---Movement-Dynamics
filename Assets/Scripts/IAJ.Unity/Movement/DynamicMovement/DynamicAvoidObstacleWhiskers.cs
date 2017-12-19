using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidObstacleWhiskers : DynamicSeek
	{
		public override string Name {
			get { return "AvoidObstacleWhiskers"; }
		}

		public float MaxLookAhead  { get; set; }
		public float AvoidMargin  { get; set; }
		public Collider collisionDetector { get; set; }
		public override KinematicData Target { get; set; }

		public DynamicAvoidObstacleWhiskers (GameObject collider)
		{
			collisionDetector = collider.GetComponent<Collider> ();
			this.Target = new KinematicData ();
		}

		public override MovementOutput GetMovement ()
		{
			//right whisker
			{
				Vector3 adjustedVel = Quaternion.AngleAxis (30, Vector3.up) * this.Character.velocity.normalized;

				if (adjustedVel.x != 0 && adjustedVel.z != 0) {

					Ray rayVector = new Ray (this.Character.position, adjustedVel.normalized);
					RaycastHit hitInfo = new RaycastHit ();
					Debug.DrawRay (this.Character.position, adjustedVel.normalized * (this.MaxLookAhead / 2), Color.blue);
					if (collisionDetector.Raycast (rayVector, out hitInfo, this.MaxLookAhead / 2)) {
						Target.position = hitInfo.point + hitInfo.normal * (AvoidMargin * 1.5F);
						return base.GetMovement ();
					}
				}
			}

			//left whisker
			{
				Vector3 adjustedVel = Quaternion.AngleAxis (-30, Vector3.up) * this.Character.velocity.normalized;

				if (adjustedVel.x != 0 && adjustedVel.z != 0) {

					Ray rayVector = new Ray (this.Character.position, adjustedVel.normalized);
					RaycastHit hitInfo = new RaycastHit ();
					Debug.DrawRay (this.Character.position, adjustedVel.normalized * (this.MaxLookAhead / 2), Color.green);
					if (collisionDetector.Raycast (rayVector, out hitInfo, this.MaxLookAhead / 2)) {
						Target.position = hitInfo.point + hitInfo.normal * (AvoidMargin * 1.5F);
						return base.GetMovement ();
					}
				}
			}

			//forward ray
			{
				if (this.Character.velocity.x != 0 && this.Character.velocity.z != 0) {

					Ray rayVector = new Ray (this.Character.position, this.Character.velocity.normalized);
					RaycastHit hitInfo = new RaycastHit ();
					Debug.DrawRay (this.Character.position, this.Character.velocity.normalized * this.MaxLookAhead, Color.red);
					if (collisionDetector.Raycast (rayVector, out hitInfo, this.MaxLookAhead)) {
						Target.position = hitInfo.point + hitInfo.normal * AvoidMargin;
						return base.GetMovement ();
					}
				}
			}

			return new MovementOutput ();

		}
	}
}
