using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<LuaObjectComponent>().Load("Init");
    }
}
