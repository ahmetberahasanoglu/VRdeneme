using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int CalculateScore(Prescription prescription, PlayerSelections selection)
    {
        int score = 0;

        if (Mathf.Approximately(prescription.sphere, selection.sphere)) score++;
        if (Mathf.Approximately(prescription.cylinder, selection.cylinder)) score++;
        if (Mathf.Approximately(prescription.axis, selection.axis)) score++;

        if (prescription.lensType == selection.lensType) score++;
        if (prescription.frameType == selection.frameType) score++;
        if (prescription.coatingType == selection.coatingType) score++;
        if (prescription.nosePadType == selection.nosePadType) score++;
        if (prescription.screwType == selection.screwType) score++;

        return score;
    }
}
