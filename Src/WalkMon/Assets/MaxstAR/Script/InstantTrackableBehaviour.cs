/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.IO;
using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine.Rendering;

namespace maxstAR
{
    public class InstantTrackableBehaviour : AbstractInstantTrackableBehaviour
    {
		public override void OnTrackSuccess(string id, string name, Matrix4x4 poseMatrix)
        {
            //Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            //Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            List<GameObject> childObjects = new List<GameObject>();

            for(int i = 0; i < transform.childCount; i++)
            {
                childObjects.Add(transform.GetChild(i).gameObject);
            }

            // Enable renderers
            /*foreach (Renderer component in rendererComponents)
            {
                component.gameObject.SetActive(true);
            }

            // Enable colliders
            foreach (Collider component in colliderComponents)
            {
                component.gameObject.SetActive(true);
            }*/

            

            switch (GameManager.gameManager.status)
            {
                case "Egg":
                    switch(GameManager.gameManager.grade)
                    {
                        case "Low":
                            InitList(childObjects, childObjects[0]);
                            break;

                        case "Middle":
                            InitList(childObjects, childObjects[1]);
                            break;

                        case "High":
                            InitList(childObjects, childObjects[2]);
                            break;
                    }
                    break;

                case "First":
                    switch (GameManager.gameManager.grade)
                    {
                        case "Low":
                            InitList(childObjects, childObjects[3]);
                            break;

                        case "Middle":
                            InitList(childObjects, childObjects[4]);
                            break;

                        case "High":
                            InitList(childObjects, childObjects[5]);
                            break;
                    }
                    break;

                case "Second":
                    switch (GameManager.gameManager.grade)
                    {
                        case "Low":
                            InitList(childObjects, childObjects[6]);
                            break;

                        case "Middle":
                            InitList(childObjects, childObjects[7]);
                            break;

                        case "High":
                            InitList(childObjects, childObjects[8]);
                            break;
                    }
                    break;
            }

			transform.position = MatrixUtils.PositionFromMatrix(poseMatrix);
			transform.rotation = MatrixUtils.QuaternionFromMatrix(poseMatrix);
			transform.localScale = MatrixUtils.ScaleFromMatrix(poseMatrix);
        }

        public override void OnTrackFail()
        {
            //Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            //Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            List<GameObject> childObjects = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                childObjects.Add(transform.GetChild(i).gameObject);
            }

            // Disable renderer
            /*foreach (Renderer component in rendererComponents)
			{
                component.gameObject.SetActive(false);
			}

            // Disable collider
            foreach (Collider component in colliderComponents)
            {
                component.gameObject.SetActive(false);
            }*/

            foreach (GameObject gameObject in childObjects)
            {
                gameObject.SetActive(false);
            }
        }

        private void InitList(List<GameObject> objects, GameObject selected)
        {
            foreach(GameObject gameObject in objects)
            {
                if(gameObject == selected)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}