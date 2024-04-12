using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabirynthPlayerScript : MonoBehaviour
{
    public UnityEvent<string> HoleEnteredEvent; 
    public float Speed = 10f;
    bool _directionToHole = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    IEnumerator MoveSelfToPoint(RaycastHit hit)
    {
        float counter = 0;
        Vector3 newPosition = new Vector3(hit.point.x, transform.position.y, transform.position.z);

        while (counter < Speed)
        {
            counter += Time.deltaTime;
            Vector3 currentPos = transform.position;

            float time = Vector3.Distance(currentPos, hit.point) / (Speed - counter) * Time.deltaTime;

            transform.position = Vector3.MoveTowards(currentPos, newPosition, time);

            yield return null;
        }
        if (_directionToHole)
        {
            HoleEnteredEvent.Invoke(hit.transform.name);
        }
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
                    StartCoroutine(MoveSelfToPoint(hit));
                    _directionToHole = false;
                }
                if (hittedObject.tag == "Hole")
                {
                    _directionToHole = true;
                }
            }
        }
    }
}
