using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    bool isFiring;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void pointerClick()
    {
        Invoke("makeFireVaribaleTrue", 0.5f);
    }

    void makeFireVariableTrue()
    {
        isFiring = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {

        }
    }
}
