using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]Character _character;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
               
        //Character is pressing a key, turn in that direction
        this._character.horizontal = horizontal;
        this._character.vertical = vertical;

        if (Input.GetButtonUp("Pickup")) this._character.OnInteract();
        if (Input.GetButtonUp("Attack")) this._character.OnKillObject();
    }
}
