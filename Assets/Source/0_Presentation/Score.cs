using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private Text score;

        private int scorePlayer;
        private int scoreBot;

        private void Awake()
        {
            if (score == null) score = GetComponent<Text>();
        }

        private void Start() => GameController.Instance.EventScoreChange += ChangeScore;

        private void ChangeScore(string loserTag)
        {
            if (loserTag == "Player") scoreBot++;
            else if (loserTag == "Bot") scorePlayer++;
            score.text = scorePlayer + ":" + scoreBot;
        }
    }
}