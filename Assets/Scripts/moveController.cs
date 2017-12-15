using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveController : MonoBehaviour {
    // cameratransform
    public Transform cameraTransform;
    // the distance between camera and character
    public float cameraDistance;
    // game manager
    public GameManager gameManager;
    // speed of advance
    float moveVSpeed;
    // speed of moving
    public float moveHSpeed = 5.0f;
    // jump height
    public float jumpHeight = 5.0f;
    // animator
    Animator m_animator;
    // jump begin time
    double m_jumpBeginTime;
    // sign of jump
    int m_jumpState = 0;
    // max speed
    public float maxVSpeed = 8.0f;
    // min speed
    public float minVSpeed = 5.0f;

    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody>().freezeRotation = true;
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
            print("null");
        moveVSpeed = minVSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        // game over
        if (gameManager.isEnd)
        {
            return;
        }
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run"))
        {
            m_jumpState = 0;
            if (Input.GetButton("Jump"))
            {
                //start jump
                m_animator.SetBool("Jump", true);
                m_jumpBeginTime = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            else
            {
                // to the ground
            }
        }
        else
        {
            

            // drop
            m_jumpState = 1;
            m_animator.SetBool("Jump", false);
        }
        float h = Input.GetAxis("Horizontal");
        Vector3 vSpeed = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * moveVSpeed ;
        Vector3 hSpeed = new Vector3(this.transform.right.x, this.transform.right.y, this.transform.right.z) * moveHSpeed * h;
        Vector3 jumpSpeed = new Vector3(this.transform.up.x, this.transform.up.y, this.transform.up.z) * jumpHeight * m_jumpState;
        Vector3 vCameraSpeed = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * minVSpeed;
        this.transform.position += (vSpeed + hSpeed + jumpSpeed) * Time.deltaTime;
        cameraTransform.position += (vCameraSpeed) * Time.deltaTime;
        //When the distance of the person and the camera is less than cameraDistance, it accelerates 
        if (this.transform.position.x - cameraTransform.position.x > cameraDistance)
        {
            moveVSpeed = minVSpeed;
            cameraTransform.position = new Vector3(this.transform.position.x - cameraDistance, cameraTransform.position.y, cameraTransform.position.z);
        }
        // The camera exceeds the character
        if (cameraTransform.position.x - this.transform.position.x > 0.0001f)
        {
            Debug.Log("You Lose！！！！！！！！！！");
            gameManager.isEnd = true;
        }
        //cameraTransform.position = new Vector3(this.transform.position.x - cameraDistance, cameraTransform.position.y, cameraTransform.position.z);
    }

    void OnGUI()
    {
        if (gameManager.isEnd)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU LOSE!!!", style);

        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // if it arrives
        if (other.name.Equals("ArrivePos"))
        {
            gameManager.changeRoad(other.transform);
        }
        // if it is alphawall
        else if (other.tag.Equals("AlphaWall"))
        {
           
        }
        // if it is obstacle
        else if(other.tag.Equals("Obstacle"))
        {

        }
    }
}
