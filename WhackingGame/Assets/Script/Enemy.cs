using System.Collections;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Golden,
    Bomb
}

public class Enemy : MonoBehaviour
{
    public float moveDistance = 0.5f; 
    public float disappearTime = 0.5f;

    public EnemyType enemyType = EnemyType.Normal; 

    private int touchCount = 0;

    private void Start()
    {
        StartCoroutine(CheckClickDelay());
    }

    private void Update()
    {
        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Bomb)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                CheckTouch(Input.touches[0]);
            }
        }
        else if (enemyType == EnemyType.Golden)
        {
            if (touchCount < 3 && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                CheckTouch(Input.touches[0]);
            }
        }
    }

    private void CheckTouch(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                touchCount++; 

                if (enemyType == EnemyType.Normal)
                {
                    if (touchCount == 1)
                    {
                        ScoreManager.instance.AddPoint(1);
                        AudioManager.instance.PlaySFX("Pop");
                        StartCoroutine(MoveAndDisappear());
                    }
                }
                else if (enemyType == EnemyType.Golden)
                {
                    if (touchCount == 1 || touchCount == 2)
                    {
                        Debug.Log("No point");
                        StartCoroutine(MoveUp());
                    }
                    else if (touchCount == 3)
                    {
                        Debug.Log("5 point");
                        ScoreManager.instance.AddPoint(5);
                        AudioManager.instance.PlaySFX("Pop");
                        StartCoroutine(MoveAndDisappear());
                    }
                    
                }
                else if (enemyType == EnemyType.Bomb)
                {
                    if (touchCount == 1)
                    {
                        Debug.Log("Bomb clicked!");
                        ScoreManager.instance.DeductPoint(3);
                        AudioManager.instance.PlaySFX("Pop");
                        StartCoroutine(MoveAndDisappear());
                    }
                }

                
            }
        }
    }
    private IEnumerator CheckClickDelay()
    {
        yield return new WaitForSeconds(2f); 

        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Bomb)
        {
            if (touchCount == 0)
            {
                StartCoroutine(Disappear());
            }
        }
        else if (enemyType == EnemyType.Golden)
        {
            if (touchCount <= 2)
            {
                StartCoroutine(Disappear());
            }
        }
            
    }
    private IEnumerator MoveUp()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 0.1f; 
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f) 
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / 0.2f);
            yield return null;
        }
    }

    private IEnumerator MoveAndDisappear()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < disappearTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / disappearTime);
            yield return null;
        }
        
        Destroy(gameObject);
    }
    
    private IEnumerator Disappear()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - Vector3.up * moveDistance * 2.5f; 
        float elapsedTime = 0f;

        while (elapsedTime < disappearTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / disappearTime);
            yield return null;
        }

        Destroy(gameObject);
    }
}
