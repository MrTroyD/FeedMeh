using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    float _horizontal;
    float _vertical;

    bool _itemInHand;

    [SerializeField]int health = 1;
    [SerializeField]float _velocity;
    [SerializeField]float _maxSpeed = 5f;
    [SerializeField]int _attackDamage = 1;

    [SerializeField]Rigidbody _rb;

    float _turnSpeed = 35f;

    GameObject _interactable;

    [SerializeField]BoxCollider _triggerBox;

    public float horizontal
    {
        get {return this._horizontal;}
        set {this._horizontal = value;}
    }

    public float vertical
    {
        get {return this._vertical;}
        set {this._vertical = value;}
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (this._triggerBox) this._triggerBox.isTrigger = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (this._triggerBox) this._triggerBox.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (this._triggerBox == null || this._itemInHand) return;
        // if (other.gameObject.layer == LayerMask.NameToLayer("Plant") || this.gameObject.layer == LayerMask.NameToLayer("Plant")) return;

        print ("Show interactible options for :: " +other.transform.parent.gameObject.name);
        this._interactable = other.gameObject;
    }

    public void OnInteract()
    {
        if (!this._interactable) return;

        if (!this._itemInHand)
        {
            //Check to see how to interact with it
            PossibleFood pf = this._interactable.GetComponentInParent<PossibleFood>();
            if (!pf)
            {
                //Say I won't touch it
                return;
            }

            Transform objectParent = this._interactable.transform.parent;
            print ("Pickup!"+pf.lifeStatus);
            switch(pf.lifeStatus)
            {
                case PossibleFood.FoodStatus.Veggies:
                    objectParent.parent = this.transform;
                    this._itemInHand = true;
                    objectParent.transform.position = new Vector3(objectParent.transform.position.x, .125f, objectParent.transform.position.z);
                    break;
                
                case PossibleFood.FoodStatus.Meat:
                    objectParent.parent = this.transform;
                    this._itemInHand = true;
                    objectParent.transform.position += Vector3.up * .25f;

                    Rigidbody rb = objectParent.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.useGravity = false;
                    }
                    break;

                case PossibleFood.FoodStatus.Alive:
                    print ("Not while it's still moving");
                    break;
            }
        }
        else if (this._itemInHand)
        {
            
            Transform parentObject = this._interactable.transform.parent;
            //Drop said item
            print ("Drop it.");
            parentObject.parent = null;
            this._itemInHand = false;
            
            Rigidbody rb = parentObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true;
            }
            else
            {
                parentObject.transform.position = new Vector3(parentObject.transform.position.x, 0, parentObject.transform.position.z);            
            }
        }
    }



    public void OnKillObject()
    {
        if (!this._interactable) return;

        Character character = this._interactable.GetComponentInParent<Character>();
        if (character == null) return;

        OnKillObject(character);
    }

    public void OnKillObject(Character character)
    {
        character.health -= this._attackDamage;
        
        print ("Attacking!");
        if (character.health == 0)
        {
            print ("Dead");
            character.GetComponentInParent<PossibleFood>().lifeStatus = PossibleFood.FoodStatus.Meat;
            character.transform.localRotation = Quaternion.Euler(90, character.transform.localRotation.y, character.transform.localRotation.z);
            // character.transform.position = new Vector3(character.transform.position.x, -0.25f, character.transform.position.z);
        }
        else if (character.health < 0)
        {
            print ("More dead");
            character.health = 0;
        }

        if (this.gameObject.layer == LayerMask.NameToLayer("Plant"))
        {
            if (character.health == 0)
            {
                PlantBehaviour pb = this.GetComponent<PlantBehaviour>();
                pb.OnEat(character.GetComponentInParent<PossibleFood>());
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag =="Ground") return;

       print ("Collision "+this.name+"::"+other.gameObject.name +" //" +(other.gameObject.layer)+","+(LayerMask.NameToLayer("Plant")));    
       if (other.gameObject.layer == LayerMask.NameToLayer("Plant"))
       {
           Character plant = other.gameObject.GetComponentInChildren<Character>();
           plant.OnKillObject(this);
       }
    }

    void OnTriggerExit(Collider other) {
        if (this._triggerBox == null || this._itemInHand) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Plant") || this.gameObject.layer == LayerMask.NameToLayer("Plant")) return;

        print ("Trigger Exit!"+this.gameObject.name +" :: " +other.transform.parent.gameObject.name);
        if (this._interactable == other.gameObject) this._interactable = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (this._horizontal != 0 || this._vertical != 0)
        {
            this._velocity = Mathf.Clamp( Mathf.Abs(this._horizontal) + Mathf.Abs(this._vertical), 0, 1);
            var lookDestination = Quaternion.LookRotation(new Vector3(this._horizontal, 0, this._vertical), transform.up);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, lookDestination,this._turnSpeed);
        }
    }


    void FixedUpdate() {
        this.transform.position += this.transform.forward * this._velocity * this._maxSpeed * Time.deltaTime; 
        if (this._velocity > .001f) 
        {
            this._velocity *= .94f;
        } else 
        {
            this._velocity = 0;
        }
    }
}
