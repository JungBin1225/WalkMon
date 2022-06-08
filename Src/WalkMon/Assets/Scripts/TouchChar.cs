using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchChar : MonoBehaviour
{
    public GameObject SmileBody;
    public Texture touchTex;
    public Texture idleTex;

    private Vector3 touchPosition;

    public Material faceMaterial;
    public Animator ani;

    void Start()
    {
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];
        ani = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Monster")
                {
                    int ran = Random.Range(0, 3);
                    switch(ran)
                    {
                        case 0:
                            ani.SetTrigger("Jump");
                            break;

                        case 1:
                            ani.SetTrigger("Damage");
                            break;

                        case 2:
                            ani.SetTrigger("Damage2");
                            break;
                    }
                    StartCoroutine(SetFace());
                }
            }
        }
    }

    IEnumerator SetFace()
    {
        faceMaterial.SetTexture("_MainTex", touchTex);
        yield return new WaitForSeconds(0.7f);
        faceMaterial.SetTexture("_MainTex", idleTex);
    }
}
