using Spine;
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
    private Spine.AnimationState animationstate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>(); // Get Spine Animation

        if (skeletonAnimation == null)
        {
            Debug.LogError("SkeletonAnimation component is missing on " + gameObject.name);
            return;
        }

        animationstate = skeletonAnimation.AnimationState;

        if (animationstate == null)
        {
            Debug.LogError("AnimationState is null on " + gameObject.name);
            return;
        }

        animationstate.SetAnimation(0, "[face]idle", true);
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

            //skeletonAnimation.AnimationName = "walk"; // Play walk animation
            animationstate.AddAnimation(0, "walk", true, 0f);
            animationstate.AddAnimation(1, "[face]attack", true, 0f);


            // Flip sprite based on direction
            if (movement.x < 0)
                skeletonAnimation.skeleton.ScaleX = -1; // Flip left
            else if (movement.x > 0)
                skeletonAnimation.skeleton.ScaleX = 1;  // Flip right
        }
        else
        {
            // Start idle transition coroutine if not already started
            animationstate.SetAnimation(0, "[face]idle", false);
            if (idleCoroutine == null)
            {
                idleCoroutine = StartCoroutine(SetIdleAfterDelay(0.5f));
            }
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    IEnumerator SetIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //skeletonAnimation.AnimationName = "reload"; // Switch to idle animation
        animationstate.SetAnimation(0, "reload", false);
        animationstate.AddAnimation(1, "[face]idle", true, 0f);
        idleCoroutine = null; // Reset coroutine reference
    }
}
