using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunStatUI : MonoBehaviour
{
	[SerializeField] private TMP_Text statLabelText;
	[SerializeField] private TMP_Text statValueText;
	[SerializeField] private TMP_Text statUpValueText;

	public void UpdateStat(string label, string value, string upValue)
	{
		if (statLabelText != null)
			statLabelText.text = label;
		if (statValueText != null)
			statValueText.text = value;
		if (statUpValueText != null)
			statUpValueText.text = upValue;
	}
}
