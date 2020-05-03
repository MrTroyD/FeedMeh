using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingScript : MonoBehaviour
{
    [SerializeField]Character _character;
    [SerializeField]Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this._animator.SetFloat("velocity", this._character.velocity);
        this._animator.SetBool("holding", this._character.itemInHand);
    }
}
