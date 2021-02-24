using UnityEngine;
using Zenject;

namespace ZigZag
{
    public class NewRecordLabel : MonoBehaviour
    {
        [Inject]
        private void Construct(ScoreService scoreService)
        {
            scoreService.NewRecord += Show;
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
    }
}