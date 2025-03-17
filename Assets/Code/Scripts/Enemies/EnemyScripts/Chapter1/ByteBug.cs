using UnityEngine;

public class ByteBug : Enemy
{
    [Header("Byte Bug Attributes")]
    [SerializeField] private float speedMultiplier = 1.5f; // Increases speed

    new void Start()
    {
        base.Start();
        moveSpeed *= speedMultiplier; // Make the Byte Bug faster
    }
}
