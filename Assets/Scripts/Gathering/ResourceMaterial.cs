using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMaterial : MonoBehaviour
{
    [SerializeField] int _quantity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetQuantity()
    {
        return _quantity;
    }
    public void SetQuantity(int num)
    {
        _quantity = num;
    }
    public void DecreaseQuantity()
    {
        _quantity--;
    }
}
