using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GlassTowerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text winText;
    private void Start()
    {
        winText.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(GlassManager.Instance.GetGameOverCondition())
        {
            winText.gameObject.SetActive(true);
            if(GlassManager.Instance.GetWinCondition())
            {
                winText.text = "You Win!";
            }
            else
            {
                winText.text = "You Lose!";
            }
        }
    }
}
