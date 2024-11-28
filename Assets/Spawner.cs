using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Animal AnimalPrefab;
    private float _timer;
    public float Interval = 20;
    
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > Interval )
        {
            Animal AnimalInstantiate = Instantiate( AnimalPrefab, transform.position, Quaternion.identity );
            _timer = 0;
        }
    }
}
