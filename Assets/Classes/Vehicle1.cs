using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle1 : MonoBehaviour {

	private float horizontal;
	private float vertical;
	public float velocidad;

    private Text Score;
    private Text Winner;

    private int laps;
    private bool flag;

    private float uprightThreshold = 0.55f;
    private Quaternion initialrot;

	// Use this for initialization
	void Start ()
    {
        initialrot = transform.rotation;
        Score = GameObject.Find("Score1").GetComponent<Text>();
        Winner = GameObject.Find("Winner").GetComponent<Text>();
        laps = 1;
        flag = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 rotationInEuler = transform.rotation.eulerAngles;
        float tiltZ = Mathf.Abs(transform.rotation.z);
        float tiltX = Mathf.Abs(transform.rotation.x);
        print(tiltZ);
        print(tiltX);

        if(tiltX > uprightThreshold || tiltZ > uprightThreshold){
            StartCoroutine(Reposition());
        } 

        if(Input.GetKeyDown("r")){
           transform.rotation = initialrot;
        }

		horizontal = Input.GetAxis("Horizontal1");
		vertical = Input.GetAxis("Vertical1");

		//transform.Translate(0, 0, velocidad * vertical * Time.deltaTime, Space.World);
        transform.Translate(Vector3.left * velocidad * vertical * Time.deltaTime, Space.Self);
        transform.Rotate(0, horizontal * 1, 0, Space.Self);
        //transform.Translate(Vector3.left * Time.deltaTime, Space.World);

        if(vertical > 0)
        {
            velocidad = 50;
            GameObject[] wheels = GameObject.FindGameObjectsWithTag("Wheel1");
            foreach(GameObject wheel in wheels)
            {
                wheel.transform.Rotate(0, 0,  10 * velocidad * Time.deltaTime, Space.Self);
            }
        }
        else if(vertical < 0)
        {
            velocidad = 10;
            GameObject[] wheels = GameObject.FindGameObjectsWithTag("Wheel1");
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(0, 0, -10 * velocidad * Time.deltaTime, Space.Self);
            }
        }
	}

    void OnTriggerExit(Collider c)
    {
        if(c.gameObject.name == "CheckPoint")
        {
            flag = true;
        }
        if (c.gameObject.name == "Lap" && flag)
        {
            laps += 1;
            if (laps == 4)
            {
                Winner.text = "Winner: Player 1";
                Time.timeScale = 0;
            }
            else
            {
                Score.text = "Lap: " + laps + "/3";
            }
            flag = false;
        }
    }
    void OnTriggerStay(Collider other){
         if (other.gameObject.tag == "GrassFloor") {
            velocidad = 18;
            GameObject[] wheels = GameObject.FindGameObjectsWithTag("Wheel2");
            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(0, 0, -10 * velocidad * Time.deltaTime, Space.Self);
            }
         }
     }

    IEnumerator Reposition()
    {
        yield return new WaitForSeconds(3);
        transform.rotation = initialrot;
    }
}
