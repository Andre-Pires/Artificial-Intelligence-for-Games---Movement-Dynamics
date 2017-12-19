using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement
{
	public class MovementOutput
	{
		public Vector3 linear;
		public float angular;
		public Vector3 wanderOffset;
		public float wanderRadius;

		public MovementOutput ()
		{
			this.Clear ();
		}

		public void Clear ()
		{
			this.linear = Vector3.zero;
			this.angular = 0;
		}

		public float SquareMagnitude ()
		{
			return this.linear.sqrMagnitude + this.angular * this.angular;
		}


		public float Magnitude ()
		{
			return (float)Math.Sqrt (this.SquareMagnitude ());
		}

		public static bool operator == (MovementOutput s1, MovementOutput s2)
		{
			return s1.linear == s2.linear && s1.angular == s2.angular;
		}

		public static bool operator != (MovementOutput s1, MovementOutput s2)
		{

			return s1.linear != s2.linear || s1.angular != s2.angular;
		}

	}
}

