using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int finalScore { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int CalculateScore(PrescriptionData prescription, PlayerSelections playerSelection)
    {
        int score = 0;

        if (prescription.lensType == playerSelection.lensType) score++;
        if (prescription.frameType == playerSelection.frameType) score++;
        if (prescription.coatingType == playerSelection.coatingType) score++;
        if (prescription.nosePadType == playerSelection.nosePadType) score++;
        if (prescription.screwType == playerSelection.screwType) score++;

        finalScore = score;
        Debug.Log("Final Score: " + finalScore);
        return score;
    }
}
