using System;
using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    
    public event Action OnRoundStart;
    public event Action OnRoundEnd;

    public int currentRound = 0;
    private bool isRoundActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public IEnumerator EndRound()
    {
        isRoundActive = false;
        Debug.Log($"Round {currentRound} ended!");
        OnRoundEnd?.Invoke(); 

        yield return null;
    }
}
