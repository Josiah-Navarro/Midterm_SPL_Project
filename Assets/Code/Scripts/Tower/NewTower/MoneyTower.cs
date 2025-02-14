using UnityEngine;
public class MoneyTower : BaseSupport
{
    public int moneyPerRound = 250;

    protected override void ApplySupportEffect()
    {
        RoundManager.Instance.OnRoundEnd += GenerateIncome;
    }

    protected override void RemoveSupportEffect()
    {
        RoundManager.Instance.OnRoundEnd -= GenerateIncome;
    }

    private void GenerateIncome()
    {
        LevelManager.main.IncreaseCurrency(moneyPerRound);
        Debug.Log($"Generate Income {moneyPerRound}");
    }
}
