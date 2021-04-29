using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
	private SpriteRenderer dead;
	private SpriteRenderer seed;

	public void StartFlashRed()
    {
		dead = transform.GetChild(0).GetComponent<SpriteRenderer>();
		seed = transform.GetChild(1).GetComponent<SpriteRenderer>();
		StartCoroutine(FlashRed());
	}

	IEnumerator FlashRed() 
	{ 
		Color red = new Color32(255, 208, 208, 255);
		dead.color = red;
		seed.color = red;

		for (int i = 209; i <= 255; i += 4)
		{
			dead.color = new Color32(255, (byte)i, (byte)i, 255);
			seed.color = new Color32(255, (byte) i, (byte) i, 255);
			yield return new WaitForSeconds(0.0001f);
		}
		dead.color = Color.white;
		seed.color = Color.white;
	}
}
