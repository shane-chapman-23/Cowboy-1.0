using UnityEngine;

[CreateAssetMenu(fileName = "newSheepData", menuName = "Data/Animal Data/Sheep Data")]
public class SheepData : AnimalData
{
    public float velocity;
    public override float Velocity => velocity;

    public float strength;
    public override float AnimalStrength => strength;
}

