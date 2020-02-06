using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMaterial : MonoBehaviour
{
    enum MaterialType
    {
        Crystal, Metal
    }
    [SerializeField] MaterialType _materialType;
    [SerializeField] int _quantity;
    // Start is called before the first frame update
    void Start()
    {
        if(_quantity == 0)
        {
            _quantity = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //deactivate the object if there's no material left
        if(GetQuantity() <= 0)
        {
            gameObject.SetActive(false);
        }
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
