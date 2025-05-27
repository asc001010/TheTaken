using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
