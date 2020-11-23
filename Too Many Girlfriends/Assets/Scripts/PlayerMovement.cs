using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    private float yValue;
    private Vector3 destinationPosition;
    private float destinationDistance;
    private bool isKeyBoardControl;
    // Start is called before the first frame update
    void Start()
    {
        yValue = this.transform.position.y;
        destinationPosition = this.transform.position;
        isKeyBoardControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Fixed the Y position
        this.transform.position = new Vector3(this.transform.position.x, yValue, this.transform.position.z);
        //Lets fix the rotation
        Quaternion rotation = Quaternion.Euler(Vector3.zero);
        this.transform.rotation = rotation;

        //keep track of distance
        destinationDistance = Vector3.Distance(destinationPosition, this.transform.position);

        if (destinationDistance < .5f)
        {
            destinationPosition = this.transform.position;
        }

        //Mouse Movement
        if (Input.GetMouseButtonDown(0))
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
                    break;
                }
            }
        }

       
        if (destinationDistance > .5f && !isKeyBoardControl)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, destinationPosition, Speed * Time.deltaTime);
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
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.tag == "Wall")
        //{
        //    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //}
    }
}
