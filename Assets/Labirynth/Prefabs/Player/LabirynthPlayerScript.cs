using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabirynthPlayerScript : MonoBehaviour
{
    public UnityEvent<string> HoleEnteredEvent;
    public float Speed = 10f;
    public GameObject RightNode;    
    
    bool _directionToHole = false;
    [SerializeField]
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    static bool IsObstacleOnWay(Transform player, Vector3 destination)
    {        
        float distance = Mathf.Abs((destination - player.transform.position).x);

        RaycastHit[] hits = Physics.RaycastAll(player.transform.position, player.transform.right, distance);
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

    public void TurnToDirection(Vector3 destination)
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
    public IEnumerator MoveSelf(Vector3 point, string nameOfDestinationGameObject = "")
    {
        Vector3 newPosition = new Vector3(point.x, transform.position.y, transform.position.z);

        while(transform.position.x != point.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Speed * Time.deltaTime);

            yield return null;
        }

        if (_directionToHole && nameOfDestinationGameObject != "")
        {
            HoleEnteredEvent.Invoke(nameOfDestinationGameObject);
        }

        if(RightNode != null)
        {
            TurnToDirection(RightNode.transform.position);
        }
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
                    if (!IsObstacleOnWay(transform, hit.point))
                    {
                        StopAllCoroutines();                        

                        StartCoroutine(MoveSelf(hit.point, hit.transform.gameObject.name));
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
