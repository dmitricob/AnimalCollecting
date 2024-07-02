using UnityEngine;

namespace DefaultNamespace.UI
{
    public class HudScoreCount : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
        
        public void SetScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}