using UnityEngine;

public class AnimalData : ScriptableObject
{
    public float playerDetectionRadius;

    public virtual float Velocity => 5;
    public virtual float AnimalStrength => 5;
    public virtual float FleeTimer => 1;
}
