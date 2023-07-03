using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Transform sunrise; //포물선 시작위치
    public Transform sunset; //포물선 종료위치
    public float journeyTime = 1.0F; //시작위치에서 종료위치까지 도달하는 시간, 값이 높을수록 느리게 간다.
    public float startTime;
    public float reduceHeight = 1f; //Center값을 줄이기, 해당 값이 높을수록 포물선의 높이는 낮아진다.

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

            Vector3 center = (sunrise.position + sunset.position) * 0.5F; //Center 값만큼 위로 올라간다.
            center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.

            Vector3 riseRelCenter = sunrise.position - center;
            Vector3 setRelCenter = sunset.position - center;

            float fracComplete = (Time.time - startTime) / journeyTime;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
            yield return null;
        }
    }
   
}