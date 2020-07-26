using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    [SerializeField]Transform bottomLanding;
    [SerializeField]Transform topLanding;

    [SerializeField] GameObject BottomTrigger;
    [SerializeField] GameObject TopTrigger;


    public Transform MoveUp()
    {
        return topLanding;
    }

    public Transform MoveDown()
    {
        return bottomLanding;
    }



    public bool CheckUp(GameObject go)
    {
        if(go == BottomTrigger)
        {
            return true;
        }
        return false;
    }

    public bool CheckDown(GameObject go)
    {
        if(go == TopTrigger)
        {
            return true;
        }
        return false;
    }
}
