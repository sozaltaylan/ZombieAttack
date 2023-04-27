using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class EarnedMoneyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _earnedMoneyText;

    public void SetEarnedMoneyText(bool active, float zombieMoney)
    {
        if (active)
        {
            _earnedMoneyText.gameObject.SetActive(true);
            //_earnedMoneyText.gameObject.transform.LookAt(CameraManager.Instance.transform);
            _earnedMoneyText.gameObject.transform.DOLocalMoveY(1, 1, false);
            _earnedMoneyText.DOFade(0,1);
            _earnedMoneyText.text = "$" + zombieMoney.ToString();
        }
    }
}
