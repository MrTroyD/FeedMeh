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

        if (this._character.health == 0 || MainGame.Instance.gamePaused || !MainGame.Instance.gameActive) return;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
               
        //Character is pressing a key, turn in that direction
        this._character.horizontal = horizontal;
        this._character.vertical = vertical;

        if (Input.GetButtonUp("Pickup")) this._character.OnInteract();
        if (Input.GetButtonUp("Attack")) this._character.OnKillObject();

        //Pressing Control should attack as well. Too lazy to fix
        if (Input.GetKeyUp(KeyCode.LeftControl)) this._character.OnKillObject();
        if (Input.GetKeyUp(KeyCode.RightControl)) this._character.OnKillObject();
        
    }
}
