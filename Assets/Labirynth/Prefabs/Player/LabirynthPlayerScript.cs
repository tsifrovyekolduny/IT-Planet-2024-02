using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirynthPlayerScript : MonoBehaviour
{
    public float Speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    IEnumerator MoveSelfToPoint(Vector3 point)
    {   
        Vector3 newPosition = new Vector3(point.x, transform.position.y, transform.position.z);

        //Substract the time of arrival with the current time to know how long the object has to be in movement
        float duration = Speed - Time.time;
        //Calculate the distance
        float distance = Vector3.Distance(transform.position, newPosition);
        //And the speed
        float speed = distance / duration;


        while(transform.position.x != newPosition.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    void MakeTransition()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100f))
            {
                Transform hittedObject = hit.transform;
                if(hittedObject.tag == "Section" || hittedObject.tag == "Hole")
                {
                    StopAllCoroutines();
                    StartCoroutine("MoveSelfToPoint", hit.point);
                }
                if(hittedObject.tag == "Hole")
                {
                    MakeTransition();
                }
            }
        }
    }
}
