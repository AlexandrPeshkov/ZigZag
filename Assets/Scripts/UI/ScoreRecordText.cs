using System.Collections;
using UnityEngine;
using Zenject;

namespace ZigZag
{
	public class ScoreRecordText : MonoBehaviour
	{
		[SerializeField]
		private GameObject _content;

		[Inject]
		private void Construct(ScoreService scoreService)
		{
			scoreService.NewRecord += OnNewRecord;
		}

		private void OnNewRecord()
		{
			StartCoroutine(ShowNewRecordText());
		}

		private IEnumerator ShowNewRecordText()
		{
			_content.gameObject.SetActive(true);
			yield return new WaitForSecondsRealtime(2f);
			_content.gameObject.SetActive(false);
		}
	}
}