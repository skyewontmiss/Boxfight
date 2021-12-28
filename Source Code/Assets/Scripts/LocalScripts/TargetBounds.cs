using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBounds : MonoBehaviour
{
    [SerializeField] BoxCollider boundary;
    public static TargetBounds instance;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 center = boundary.center + transform.position;

        float minX = center.x - boundary.size.x / 2;
        float maxX = center.x + boundary.size.x / 2;

        float minY = center.y - boundary.size.y / 2;
        float maxY = center.y + boundary.size.y / 2;

        float minZ = center.z - boundary.size.z / 2;
        float maxZ = center.z + boundary.size.z / 2;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

        return randomPosition;
    }
}
