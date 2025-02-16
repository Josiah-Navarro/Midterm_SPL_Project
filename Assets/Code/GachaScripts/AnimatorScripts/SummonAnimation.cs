using UnityEngine;

public class SummonAnimation : MonoBehaviour
{
    public Animator portalAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        portalAnimator = GetComponent<Animator>();
    }
    
    public void StartSummon()
    {
        Debug.Log("EEEE");
        portalAnimator.SetTrigger("Open");
    }
}
