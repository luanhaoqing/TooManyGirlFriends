using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public GameObject Cursor;
    public GameObject CurrentFollowingGirlFriend;
    public bool HasMoveOutOfShop;
    public bool IsInRestroom;
    private GameObject currentShop;
    private float yValue;
    private Vector3 destinationPosition;
    private float destinationDistance;
    private bool isKeyBoardControl;
    private bool isStop;
    private NavMeshAgent man;
    // Start is called before the first frame update
    void Start()
    {
        yValue = this.transform.position.y;
        destinationPosition = this.transform.position;
        isKeyBoardControl = false;
        Cursor.SetActive(false);
        isStop = true;
        IsInRestroom = false;
        man = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Movement
        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100);
            for(int i = 0;i<hits.Length;i++)
            {
                if(hits[i].collider.gameObject.tag == "Wall")
                {
                    break;
                }
                else if(hits[i].collider.gameObject.tag == "Ground")
                {
                    isKeyBoardControl = false;
                    destinationPosition = new Vector3(hits[i].point.x, yValue, hits[i].point.z);
                    Cursor.SetActive(true);
                    Cursor.transform.position = new Vector3(destinationPosition.x,0, destinationPosition.z);
                    break;
                }
            }
        }
        //keep track of distance

        destinationDistance = Vector3.Distance(destinationPosition, this.transform.position);

        if (destinationDistance < .5f)
        {
            destinationPosition = this.transform.position;
        }

        if (destinationDistance > .5f && !isKeyBoardControl)
        {
            man.SetDestination(destinationPosition);
            man.speed = Speed;
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", true);
            isStop = false;
        }
        else if(destinationDistance <= .5f && !isKeyBoardControl && !isStop)
        {
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", false);
            Cursor.SetActive(false);
            isStop = true;
        }

    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        this.GetComponent<Rigidbody>().velocity = new Vector3((h * Speed),
           0, (v * Speed));
  
        if(h != 0 || v != 0)
        {
            isKeyBoardControl = true;
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", true);
            Cursor.SetActive(false);
            isStop = false;
                 this.transform.LookAt(new Vector3((this.transform.position.x + h * Speed),
                    yValue, (this.transform.position.z + v * Speed)));
        }
        else if(isKeyBoardControl)
        {
            this.GetComponentInChildren<Animator>().SetBool("IsWalk", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(CurrentFollowingGirlFriend!=null)
        {
            if(CurrentFollowingGirlFriend.GetComponent<AIBehaviour>().HandlePlayerTriggerEnter(other))
            {
                HasMoveOutOfShop = false;
                currentShop = other.gameObject;
            }
        }
        if(other.tag == "Restroom")
        {
            IsInRestroom = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentShop)
        {
            HasMoveOutOfShop = true;
        }
        if (other.tag == "Restroom")
        {
            IsInRestroom = false;
        }
    }
}
