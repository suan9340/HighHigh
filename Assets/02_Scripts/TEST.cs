using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Transform sunrise; //������ ������ġ
    public Transform sunset; //������ ������ġ
    public float journeyTime = 1.0F; //������ġ���� ������ġ���� �����ϴ� �ð�, ���� �������� ������ ����.
    public float startTime;
    public float reduceHeight = 1f; //Center���� ���̱�, �ش� ���� �������� �������� ���̴� ��������.

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(MovePlayerPlablor());
        }
    }

    private IEnumerator MovePlayerPlablor()
    {
        startTime = Time.time;

        while (true)
        {
            if (transform.position == sunset.transform.position)
            {
                yield break;
            }

            Vector3 center = (sunrise.position + sunset.position) * 0.5F; //Center ����ŭ ���� �ö󰣴�.
            center -= new Vector3(0, 1f * reduceHeight, 0); //y���� ���̸� ���̰� ��������.

            Vector3 riseRelCenter = sunrise.position - center;
            Vector3 setRelCenter = sunset.position - center;

            float fracComplete = (Time.time - startTime) / journeyTime;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
            yield return null;
        }
    }
   
}