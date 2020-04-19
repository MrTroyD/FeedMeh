using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    float _horizontal;
    float _vertical;

    bool _itemInHand;

    [SerializeField]int _health = 1;
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

    public float health
    {
        get {return this._health;}
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
        if (this._triggerBox == null || this._itemInHand || other.gameObject.layer == LayerMask.GetMask("Landscape")) return;
        // if (other.gameObject.layer == LayerMask.NameToLayer("Plant") || this.gameObject.layer == LayerMask.NameToLayer("Plant")) return;
       this._interactable = other.gameObject;
    }

    public void OnInteract()
    {
        if (this._itemInHand && this._interactable == null) this._itemInHand = false;
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
             Rigidbody rb;
            switch(pf.lifeStatus)
            {
                case PossibleFood.FoodStatus.Veggies:
                    objectParent.parent = this.transform;
                    this._itemInHand = true;
                   objectParent.transform.localPosition = this._triggerBox.transform.localPosition;
                    
                    rb = objectParent.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.detectCollisions = false;
                        rb.velocity = Vector3.zero;
                        rb.isKinematic = true;    
                        rb.useGravity = false;
                        
                    }
                    break;
                
                case PossibleFood.FoodStatus.Meat:
                    objectParent.parent = this.transform;
                    this._itemInHand = true;
                    objectParent.transform.localPosition = this._triggerBox.transform.localPosition;

                    rb = objectParent.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.detectCollisions = false;
                        rb.velocity = Vector3.zero;
                        rb.isKinematic = true;               rb.isKinematic = false;
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
           
            parentObject.parent = null;
            this._itemInHand = false;
            
            Rigidbody rb = parentObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.detectCollisions = true;
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
        character._health -= this._attackDamage;
        
        print ("Attacking!");
        if (character._health == 0)
        {
            print ("Dead");
            character.GetComponentInParent<PossibleFood>().lifeStatus = PossibleFood.FoodStatus.Meat;
            character.transform.localRotation = Quaternion.Euler(90, character.transform.localRotation.y, character.transform.localRotation.z);
            // character.transform.position = new Vector3(character.transform.position.x, -0.25f, character.transform.position.z);
        }
        else if (character._health < 0)
        {
            print ("More dead");
            character._health = 0;
        }

        if (this.gameObject.layer == LayerMask.NameToLayer("Plant"))
        {
            if (character._health == 0)
            {
                PlantBehaviour pb = this.GetComponent<PlantBehaviour>();
                if (pb) pb.OnEat(character.GetComponentInParent<PossibleFood>());
            }
        }
    }

    void OnCollisionEnter(Collision other) {
       if (other.gameObject.tag =="Ground") return;

       if (other.gameObject.layer == LayerMask.NameToLayer("Plant"))
       {
           Character plant = other.gameObject.GetComponentInChildren<Character>();
           plant.OnKillObject(this);
       }
    }

    void OnTriggerExit(Collider other) {
        if (this._triggerBox == null || this._itemInHand) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Plant") || this.gameObject.layer == LayerMask.NameToLayer("Plant")) return;

        if (this._interactable == other.gameObject) this._interactable = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._health <= 0 || MainGame.Instance.gamePaused)
        {
            return;
        }

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
