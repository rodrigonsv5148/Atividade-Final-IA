using TMPro;
using UnityEngine;

namespace Pathfinding
{
    public class NodeDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreDisplay;
        [SerializeField] TMP_Text cumulativeCostDisplay;
        [SerializeField] TMP_Text heuristicDisplay;

        public void Reset()
        {
            scoreDisplay.text = cumulativeCostDisplay.text = heuristicDisplay.text = string.Empty;
        }

        public void UpdateInformation(PathPoint<Node> point)
        {
            scoreDisplay.text = point.Score.ToString();
            cumulativeCostDisplay.text = point.CumulativeCost.ToString();
            heuristicDisplay.text = point.Heuristic.ToString();

            var textColor = point.State switch
            {
                Status.Frontier => Color.green,
                Status.Visited => Color.white,
                _ => Color.magenta
            };

            scoreDisplay.color = cumulativeCostDisplay.color = heuristicDisplay.color = textColor;
        }
    }
}