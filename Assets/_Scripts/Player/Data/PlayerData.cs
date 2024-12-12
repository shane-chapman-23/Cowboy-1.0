using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/ Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Trot State")]
    public float trotVelocity = 1f;

    [Header("Gallop State")]
    public float gallopVelocity = 3f;
}
