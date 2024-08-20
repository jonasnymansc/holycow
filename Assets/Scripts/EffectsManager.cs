using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject destroyBlockEffect;

    void OnEnable()
    {
        Block.OnDestroyMe += OnDestroyMe;
    }
    
    void OnDisable()
    {
        Block.OnDestroyMe -= OnDestroyMe;
    }
    

    void OnDestroyMe(Vector3 position)
    {
        Instantiate(destroyBlockEffect, position, Quaternion.identity);    
    }
}
