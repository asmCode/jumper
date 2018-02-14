using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsPanel : MonoBehaviour {
	public static int randomVariable;

	public GameObject arrow;
	public GameObject hint;
	public Vector2 positionUL;
	public Vector2 positionUR;
	public Vector2 positionDL;
	public Vector2 positionDR;
	public Vector2 hintPositionL;
	public Vector2 hintPositionR;

	public void TargetSpotRNG()
	{
		randomVariable = Random.Range(1, 4);
		Debug.Log ("Hint random generated is " + randomVariable);
	}

	public void TargetHintActivate()
	{
		Debug.Log ("Hint activated");
		switch (randomVariable)
		{
		case 1:
			//reposition arrow canvas image up left
			arrow.GetComponent<RectTransform>().anchoredPosition = positionUL;
			//reposition description canvas text left
			hint.GetComponent<RectTransform>().anchoredPosition = hintPositionL;
			break;
		case 2:
			//reposition arrow canvas image up right
			arrow.GetComponent<RectTransform>().anchoredPosition = positionUR;
			//reposition description canvas text right
			hint.GetComponent<RectTransform>().anchoredPosition = hintPositionR;
			break;
		case 3:
			//reposition arrow canvas image down left
			arrow.GetComponent<RectTransform> ().anchoredPosition = positionDL;
			arrow.GetComponent<RectTransform> ().Rotate( new Vector3 (0, 0, 180) );
			//reposition description canvas text left
			hint.GetComponent<RectTransform>().anchoredPosition = hintPositionL;
			break;
		case 4:
			//reposition arrow canvas image down right
			arrow.GetComponent<RectTransform>().anchoredPosition = positionDR;
			arrow.GetComponent<RectTransform> ().Rotate( new Vector3 (0, 0, 180) );
			//reposition description canvas text right
			hint.GetComponent<RectTransform>().anchoredPosition = hintPositionR;
			break;
		}
		//turn on arrow canvas image
		arrow.SetActive(true);
		//turn on description canvas tex
		hint.SetActive(true);
	}

	public void TargetHintDeactivate()
	{
		arrow.SetActive(false);
		hint.SetActive(false);
	}
}
