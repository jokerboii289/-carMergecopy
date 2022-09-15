using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class ArrowElement : MonoBehaviour
    {
        public Transform AdjacentTile;//tile attached to the arrow
        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }
        private void Start()
        {
            AdjacentTile = null;
            Invoke("GetTile",3f);
        }
        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }

        private void Update()
        {
            GetTile();
        }
        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.Input)
            {
                _canInstantiate = true;
            }
            if(newState==GameState.Movement)
            {
                _canInstantiate = false;
            }
            if(newState==GameState.Levelwin)
            {
                _canInstantiate = false;
            }
            if(newState==GameState.Levelfail)
            {
                _canInstantiate = false;
            }
        
        }

        private bool _canInstantiate;
        private void OnMouseDown()
        {
            //check if the tile attaced to arrow is empty or not


            if(!_canInstantiate || MainController.instance.GameState == GameState.Levelwin) return;
            CarsController.instance.SpawnCar(transform);
            MainController.instance.SetActionType(GameState.Movement);
            SoundsController.instance.PlaySound(SoundsController.instance.arrowTap);
            transform.DOScale(0.2f, 0.25f).OnComplete(() =>
            {
                transform.DOScale(0.5f, 0.25f);
            });
            Vibration.Vibrate(40);
        }

        void GetTile() //to get tile attached to arrow
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position - new Vector3(0, .5f, 0), transform.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("tile"))
                {
                    AdjacentTile = hit.transform;
                    var edgeTileList = FloorEffectElement.instance.edgeTile;
                    if(!edgeTileList.Contains(hit.transform))
                        edgeTileList.Add(hit.transform);

                    if(Physics.Raycast(AdjacentTile.position,AdjacentTile.up,out RaycastHit hitTwo,Mathf.Infinity))
                    {
                        //print(hitTwo.transform);
                        if (hitTwo.transform.CompareTag("car"))
                            _canInstantiate = false;
                        else
                            _canInstantiate = true;
                    }
                }
            }
        }

    }   
}
