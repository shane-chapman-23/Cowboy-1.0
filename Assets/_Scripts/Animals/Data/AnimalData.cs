using UnityEngine;

public class AnimalData : ScriptableObject
{
    public LayerMask whatIsMapEdge;
    public LayerMask whatIsLasso;
    public float playerDetectionRadius;
    public virtual float Velocity => 5;
    public virtual float AnimalStrength => 5;
    public virtual float FleeTimer => 1;
}
