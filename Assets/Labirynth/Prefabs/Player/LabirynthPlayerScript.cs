using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabirynthPlayerScript : MonoBehaviour
{
    public UnityEvent<string> HoleEnteredEvent;
    public float Speed = 10f;
    public GameObject RightNode;
    // public List<Node> SubSection;
    // public Node EasiestNode;

    Vector3 _destination;
    bool _directionToHole = false;
    [SerializeField]
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    bool IsObstacleOnWay(Vector3 destination)
    {
        // RaycastHit hit;

        float distance = Mathf.Abs((destination - transform.position).x);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.right, distance);
        foreach(var hit in hits)
        {
            Debug.Log("hitted: " + hit.transform.gameObject.name);
            if(hit.transform.tag == "Wall")
            {
                return true;
            }
        }            

        return false;
    }

    private void TurnToDirection(Vector3 destination)
    {        
        if (transform.position.x < destination.x)
        {
            //direction = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //direction = -1;
        }
    }

    // Update is called once per frame
    IEnumerator MoveSelfToPoint(Vector3 point, string nameOfDestinationGameObject = "")
    {
        float counter = 0;
        Vector3 newPosition = new Vector3(point.x, transform.position.y, transform.position.z);

        while (counter < Speed)
        {
            counter += Time.deltaTime;
            Vector3 currentPos = transform.position;

            float time = Vector3.Distance(currentPos, point) / (Speed - counter) * Time.deltaTime;

            transform.position = Vector3.MoveTowards(currentPos, newPosition, time);

            yield return null;
        }

        if (_directionToHole && nameOfDestinationGameObject != "")
        {
            HoleEnteredEvent.Invoke(nameOfDestinationGameObject);
        }

        TurnToDirection(RightNode.transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Transform hittedObject = hit.transform;
                if (hittedObject.tag == "Section" || hittedObject.tag == "Hole")
                {
                    TurnToDirection(hit.point);
                    if (!IsObstacleOnWay(hit.point))
                    {
                        StopAllCoroutines();                        

                        StartCoroutine(MoveSelfToPoint(hit.point, hit.transform.gameObject.name));
                    }
                    

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
