using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using DG.Tweening;
using UnityEngine;

namespace _Cars_Merge._Scripts.ElementRelated
{
    public class FloorEffectElement : MonoBehaviour
    {
        public static FloorEffectElement instance;
        
        public List<Transform> tiles;  //no of tiles
        public List<Transform> arrows;
        public List<Transform> walls;

        public List<Transform> edgeTile;//list of edge tiles
 
        private void Awake()
        {
            instance = this;
            MainController.GameStateChanged += CheckEdgeTilesOccupied;
        }
       

        private void Start()
        {
            StartCoroutine(TileEffect());
        }

        
        IEnumerator TileEffect()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                Vector3 origAngle = walls[i].transform.eulerAngles;
                walls[i].DORotate(new Vector3(60, origAngle.y, origAngle.z), 0.25f).From();
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].transform.DORotate(Vector3.right * 45, 0.25f).From();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.4f);
            for (int i = 0; i < arrows.Count; i++)
            {
                Vector3 origAngle = arrows[i].transform.eulerAngles;
                arrows[i].transform.DORotate(new Vector3(origAngle.x, origAngle.y+ 180, origAngle.z), 0.25f).From();
            }
        }

        void CheckEdgeTilesOccupied(GameState newState, GameState oldState)
        {
            StartCoroutine(Delay(newState,oldState));
        }
        IEnumerator Delay(GameState newState, GameState oldState)
        {
            yield return new WaitForSeconds(.5f);
            if (newState == GameState.Input)
            {
                int possibleArrow = 0;
                for (int i = 0; i < edgeTile.Count; i++)
                {
                    if (Physics.Raycast(edgeTile[i].position, edgeTile[i].up, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.transform.CompareTag("car"))
                            possibleArrow++;
                    }
                }
                if (possibleArrow == edgeTile.Count)
                {
                    //loose panel;
                    yield return new WaitForSeconds(1);
                    MainController.instance.SetActionType(GameState.Levelfail);
                }
            }
        }

        private void OnDisable()
        {
            MainController.GameStateChanged -= CheckEdgeTilesOccupied;
        }
    }   
}
