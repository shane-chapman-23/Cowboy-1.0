using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

    public GameObject sheep;
    public GameObject ram;

    private GameObject _currentAnimal;
    private int _animalsSpawned = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnAnimal();
    }

    public void SpawnAnimal()
    {
        Vector3 spawnPosition = new Vector3(Player.Instance.transform.position.x + 10f, -0.5f, -6 );
        Quaternion spawnRotation = Quaternion.identity;

        if (_animalsSpawned == 5)
        {
            return;
        }
        else if (_animalsSpawned == 4)
        {
            _currentAnimal = Instantiate(ram, spawnPosition, spawnRotation);
            _animalsSpawned++;
        }
        else
        {
            _currentAnimal = Instantiate(sheep, spawnPosition, spawnRotation);
            _animalsSpawned++;
        }
    }
}
