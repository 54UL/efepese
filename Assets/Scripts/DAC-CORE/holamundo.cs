using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holamundo : MonoBehaviour
{
    IEnumerable TurnNotification()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(30.0f);
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(30.0f);
        this.gameObject.SetActive(true);
    }

    public float speed;

    public GameObject caja;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TurnNotification");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject xd = GameObject.Find("NOMBRE");
        Rigidbody[] fisicas = GameObject.FindObjectsOfType<Rigidbody>();

        if(Input.GetKeyDown(KeyCode.W))
            caja.GetComponent<Transform>().position = new Vector3(0,0,Time.deltaTime*speed);
    }
}
