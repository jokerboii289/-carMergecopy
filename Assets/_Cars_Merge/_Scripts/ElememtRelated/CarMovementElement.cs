using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.Ambulance;
using _Cars_Merge._Scripts.ControllerRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class CarMovementElement : MonoBehaviour
    {
        private Vector3 _moveDir;
        private Rigidbody _rb;
        public float moveSpeed;

        private CarElement _carElement;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _carElement = GetComponent<CarElement>();
            _moveDir = CarsController.instance.dir;
        }

        void FixedUpdate()
        {
            _rb.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
        }

        private void Update()
        {
            //front raycast
            /*RaycastHit frontHit;
            Physics.Raycast(transform.position, transform.forward, out frontHit, 3.5f);

            Debug.DrawRay(transform.position, transform.forward * 3.5f, Color.magenta);

            RaycastHit rightHit;
            Physics.Raycast(transform.position, transform.right, out rightHit, 3.5f);

            Debug.DrawRay(transform.position, transform.right * 3.5f, Color.magenta);

            RaycastHit lefttHit;
            Physics.Raycast(transform.position, -transform.right, out rightHit, 3.5f);

            Debug.DrawRay(transform.position, -transform.right * 3.5f, Color.magenta);*/
        }

        private void OnTriggerEnter(Collider other)
        {
            _carElement.engineSmoke.SetActive(false);
            CarElement otherCarElement = other.gameObject.GetComponent<CarElement>();

            if (otherCarElement)
            {
                if (gameObject.layer != 3)
                {
                    if (otherCarElement.num == _carElement.num && other.gameObject.layer == 3)
                    {
                        Merge(other.gameObject);
                        Vibration.Vibrate(27);
                    }
                    else if (otherCarElement.num != _carElement.num && other.gameObject.layer == 3)
                    {
                        moveSpeed = 0;
                        _rb.isKinematic = true;
                        MainController.instance.SetActionType(GameState.Input);
                        gameObject.layer = 3;
                        PlayCrashEffect();
                    }
                }
            }
            else if(other.gameObject.CompareTag("wall"))
            {
                moveSpeed = 0;
                _rb.isKinematic = true;
                MainController.instance.SetActionType(GameState.Input);
                gameObject.layer = 3;
                PlayCrashEffect();
                Vector3 origWallRot = other.transform.eulerAngles;
                other.transform.DOLocalRotate(new Vector3(15, origWallRot.y, origWallRot.z), 0.25f).OnComplete(() =>
                {
                    other.transform.DOLocalRotate(new Vector3(0, origWallRot.y, origWallRot.z), 0.25f);
                });
                Vibration.Vibrate(17);
            }
            else if (other.gameObject.CompareTag("Finish"))
            {
                MainController.instance.SetActionType(GameState.Input);
            }

            /*if (_carElement.num != other.gameObject.GetComponent<CarElement>().num && gameObject.layer != 3)
            {
                moveSpeed = 0;
                _rb.isKinematic = true;
                MainController.instance.SetActionType(GameState.Input);
                gameObject.layer = 3;
                PlayCrashEffect();
            }
            else if (_carElement.num == other.gameObject.GetComponent<CarElement>().num && other.gameObject.layer == 3)
                Merge(other.gameObject);

            if (other.gameObject.CompareTag("wall") && gameObject.layer != 3)
            {
                moveSpeed = 0;
                _rb.isKinematic = true;
                MainController.instance.SetActionType(GameState.Input);
                gameObject.layer = 3;
                PlayCrashEffect();
                _carElement.engineSmoke.SetActive(false);

                Vector3 origWallRot = other.transform.eulerAngles;
                other.transform.DOLocalRotate(new Vector3(15, origWallRot.y, origWallRot.z), 0.25f).OnComplete(() =>
                {
                    other.transform.DOLocalRotate(new Vector3(0, origWallRot.y, origWallRot.z), 0.25f);
                });
            }*/
        }

        void Merge(GameObject otherCar)
        {
            GetComponent<Collider>().enabled = false;
            otherCar.GetComponent<Collider>().enabled = false;
            if(!GameController.instance.ambulanceLevel) 
                CarsController.instance.SetupMergedCar(transform, otherCar.transform, _carElement.num * 2);
            else
                AmbulanceController.instance.SetupMergedCar(transform, otherCar.transform, _carElement.num * 2);
            /*GameObject mergeFx = otherCar.GetComponent<CarElement>().mergeFx;
            mergeFx.transform.parent = null;
            mergeFx.SetActive(true);*/
            gameObject.SetActive(false);
        }

        void PlayCrashEffect()
        {
            //_carElement.canRaycast = true;
            Vector3 origPos = new Vector3();
            if (!GetComponent<CarElement>().tileOccupied) return;
            origPos = GetComponent<CarElement>().tileOccupied.position;
            //Vector3 origPos = transform.position;
            if (transform.rotation.y < .5)
            {
                transform.DOMoveZ(origPos.z - 0.5f, 0.25f).OnComplete(() => { transform.DOMoveZ(origPos.z, 0.25f); });
            }
            else if (transform.rotation.y > .5)
            {
                transform.DOLocalMoveX(origPos.x - 0.5f, 0.25f).OnComplete(() =>
                {
                    transform.DOLocalMoveX(origPos.x, 0.25f);
                });
            }

            SoundsController.instance.PlaySound(SoundsController.instance.crash);
        }
    }
}