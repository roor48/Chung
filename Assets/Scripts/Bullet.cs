using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int throughCnt;
    public int curThroughCnt;

    public int dmg;

    private void OnEnable()
    {
        curThroughCnt = throughCnt;
    }

    private void Update()
    {
        if (GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x > 1.5f ||
            GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}
