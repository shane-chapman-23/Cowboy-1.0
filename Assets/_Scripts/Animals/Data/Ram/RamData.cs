using UnityEngine;

[CreateAssetMenu(fileName = "newRamData", menuName = "Data/Animal Data/Ram Data")]
public class RamData : AnimalData
{
    public float velocity;
    public override float Velocity => velocity;

    public float strength;
    public override float AnimalStrength => strength;
}
