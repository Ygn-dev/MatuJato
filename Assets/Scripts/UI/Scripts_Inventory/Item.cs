using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Item : MonoBehaviour
{
    public int ID;
    public String type;
    public string description;
    public Sprite sprite;

    [HideInInspector]//Para que no se muestre en el inspector
    public bool pickedUp;


}
