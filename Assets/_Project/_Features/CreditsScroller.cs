using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE
{
    public class CreditsScroller : MonoBehaviour
    {
        [SerializeField] protected TMPro.TMP_Text tMP;
        [SerializeField] protected TextAsset creditsTextAsset;
        [SerializeField] protected float scrollRate = 1;

        public void OnEnable()
        {
            tMP.rectTransform.anchoredPosition = Vector2.zero;
            tMP.text = creditsTextAsset.text;
            tMP.ForceMeshUpdate();
            Canvas.ForceUpdateCanvases();
            tMP.rectTransform.anchoredPosition = Vector2.zero;
        }

        public void Update()
        {
            if(tMP.rectTransform.rect.yMin < 0)
                tMP.rectTransform.position += Vector3.up * Time.deltaTime * scrollRate;
        }
    }
}