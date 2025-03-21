using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Round Settings")]
    public float timeBetweenRounds = 10f;
    public int currentRound = 1;

    [Header("Events")]
    public UnityEvent OnRoundStart = new UnityEvent();
    public UnityEvent OnRoundEnd = new UnityEvent();
    public DialogueLoader dl;

    private bool roundActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(RoundLoop());
    }

    private IEnumerator RoundLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenRounds);
            Debug.Log($"Round {currentRound} starting!");

            StartRound();
            dl.StartConversation("Echo");

            // Wait for either 30 seconds to pass or all enemies to be defeated
            float roundTimer = 30f;
            while (roundTimer > 0)
            {
                roundTimer -= Time.deltaTime;
                yield return null;
            }
            dl.StartConversation("Turing");
            
            Debug.Log($"Round {currentRound} ending!");
            EndRound();
        }
    }



    private void StartRound()
    {
        roundActive = true;
        currentRound++;
        OnRoundStart.Invoke();
        EnemySpawner.Instance.StartRound();
    }

    private void EndRound()
    {
        roundActive = false;
        OnRoundEnd.Invoke();
        EnemySpawner.Instance.StopSpawning();
    }
}
