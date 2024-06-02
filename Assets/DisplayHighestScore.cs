using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHighestScore : MonoBehaviour
{
    public TMP_Text highestScoreText;
    int highestScore;

    void Start()
    {
        highestScoreText = GetComponent<TMP_Text>();
        // Load the highest score from PlayerPrefs
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
    }
    private void Update()
    {
        highestScoreText.text = "Highest Score: " + highestScore.ToString();
    }
}
