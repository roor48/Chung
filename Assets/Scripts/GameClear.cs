using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class GameClear : MonoBehaviour
{
    public TMP_Text infoText;

    private void Awake()
    {
        if (GameManager.Instance == null)
            return;
        infoText.text = $"걸린 총 시간 : {GameManager.Instance.sumTime/60:D2}분 {GameManager.Instance.sumTime%60:D2}\n\n" +
                        $"받은 총 데미지 : {GameManager.Instance.sumTakenDmg}\n\n" +
                        $"가한 총 데미지 : {GameManager.Instance.sumGivenDmg}\n\n" +
                        $"총 스코어 : {GameManager.Instance.sumScore}";
    }
}
