using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    enum MyBehaviour
    {
        Idle,
        Ponder,
        Turning,
        Walking
    }

    float _actionTimer;
    float _idleTimerStarts = 1f;
    float _ponderTimer = 1f;
    float _turningTimer = .25f;
    float _walkingTimer = 2f;
    

    MyBehaviour _currentBehaviour = MyBehaviour.Idle;

    Vector3 _destination;

    [SerializeField]Character _character;
    

    // Start is called before the first frame update
    void Start()
    {
        this._actionTimer = this._idleTimerStarts + Random.Range(0, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this._character.health < 1) return;
        this._actionTimer -= Time.deltaTime;

        CheckTimer();

        switch (this._currentBehaviour)
        {
            case MyBehaviour.Turning:
                this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.y, 0);
                var lookDestination = Quaternion.LookRotation(this._destination, transform.up);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, lookDestination, 15f);
                break;

            case MyBehaviour.Walking:
                // float angle = Mathf.Atan2(_destination.z, _destination.x);
                this._character.horizontal = this.transform.forward.x;//Mathf.Cos(this.transform.rotation.y * Mathf.Deg2Rad);//Mathf.Clamp(this._destination.x - this.transform.position.x , -1, 1);
                this._character.vertical = this.transform.forward.z; Mathf.Sin(this.transform.rotation.y * Mathf.Deg2Rad);
                break;
            
            default:
                this._character.horizontal = 0;
                this._character.vertical = 0;
                break;
        }
        
    }

    void CheckTimer()
    {
        if (this._actionTimer < 0)
        {
            switch (this._currentBehaviour)
            {
                case MyBehaviour.Idle:
                    this._currentBehaviour = MyBehaviour.Ponder;
                    this._actionTimer = this._ponderTimer;
                    
                    float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
                    this._destination = new Vector3(Mathf.Cos(angle) * this._walkingTimer, 0, Mathf.Sin(angle) * this._walkingTimer);
                    break;

                case MyBehaviour.Ponder:
                    this._currentBehaviour = MyBehaviour.Turning;
                    this._actionTimer = this._turningTimer;

                    break;

                case MyBehaviour.Turning:
                    this._currentBehaviour = MyBehaviour.Walking;
                    this._actionTimer = this._walkingTimer;
                    break;

                case MyBehaviour.Walking:
                    this._currentBehaviour = MyBehaviour.Idle;
                    this._actionTimer = this._idleTimerStarts + Random.Range(0, 1.5f);
                    break;
            }
        }
    }
}
