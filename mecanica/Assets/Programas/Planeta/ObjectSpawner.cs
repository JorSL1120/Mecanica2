using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform sphere;
    public GameObject objectPrefab;
    public int numObjects;

    void Start()
    {
        float radius = sphere.localScale.x / 2f;

        for(int i = 0; i < numObjects; i++)
        {
            Vector3 position = sphere.position + radius * Random.onUnitSphere;
            Quaternion rotation = Quaternion.identity;
            GameObject obj = Instantiate(objectPrefab, position, rotation);
            obj.transform.up = (position - sphere.position).normalized;
        }
    }
}
