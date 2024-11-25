using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This script is for enabling the map when the player enters the map scene
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.24</version>
public class EnableMap : MonoBehaviour
{
    /// <summary>
    /// The map that will be enabled
    /// </summary>
    [SerializeField] private GameObject map;

    /// <summary>
    /// I think you can guess what this does
    /// </summary>
    private void OnEnable()
    {
        map.SetActive(true);
    }
}
