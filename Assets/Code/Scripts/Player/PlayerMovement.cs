using UnityEngine;
using Spine.Unity;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust speed
    private Rigidbody2D rb;
    private Vector2 movement;
    private SkeletonAnimation skeletonAnimation;
    private Coroutine idleCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>(); // Get Spine Animation
    }

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        if (movement.magnitude > 0)
        {
            // Cancel any existing idle transition coroutine
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            skeletonAnimation.AnimationName = "walk"; // Play walk animation

            // Flip sprite based on direction
            if (movement.x < 0)
                skeletonAnimation.skeleton.ScaleX = -1; // Flip left
            else if (movement.x > 0)
                skeletonAnimation.skeleton.ScaleX = 1;  // Flip right
        }
        else
        {
            skeletonAnimation.AnimationName = "[face]idle"; // Switch to idle animation
            idleCoroutine = null; // Reset coroutine reference

        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

}   
