using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.Arbitration
{
    public class MovementWithWeight
    {
        public DynamicMovement.DynamicMovement Movement { get; set; }
        public float Weight { get; set; }

        public MovementWithWeight(DynamicMovement.DynamicMovement movement)
        {
            this.Movement = movement;
            this.Weight = 1.0f;
        }

        public MovementWithWeight(DynamicMovement.DynamicMovement movement, float weight)
        {
            this.Movement = movement;
            this.Weight = weight;
        }
    }

    public class BlendedMovement : DynamicMovement.DynamicMovement
    {
        
        public override string Name
        {
            get { return "Blended"; }
        }

        public override KinematicData Target { get; set; }

        public List<MovementWithWeight> Movements { get; private set; }

        public BlendedMovement()
        {
            this.Movements = new List<MovementWithWeight>();
        }

        public override MovementOutput GetMovement()
        {
            MovementOutput tempOutput;
            var finalOutput = new MovementOutput();

            var totalWeight = 0.0f;

            foreach (MovementWithWeight movementWithWeight in this.Movements)
            {
                movementWithWeight.Movement.Character = this.Character;
                
                tempOutput = movementWithWeight.Movement.GetMovement();
                if (tempOutput.SquareMagnitude() > 0)
                {
                    finalOutput.linear += tempOutput.linear * movementWithWeight.Weight;
                    finalOutput.angular += tempOutput.angular * movementWithWeight.Weight;
                    totalWeight += movementWithWeight.Weight;    
                }
            }

            if (totalWeight > 0)
            {
                //in case the totalWeight is not 1, we need to normalize
                float normalizationFactor = 1.0f/totalWeight;
                finalOutput.linear *= normalizationFactor;
                finalOutput.angular *= normalizationFactor;
            }

            return finalOutput;
        }
    }
}
