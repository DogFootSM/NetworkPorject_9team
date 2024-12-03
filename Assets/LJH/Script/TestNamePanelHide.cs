using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TestNamePanelHide : MonoBehaviour
{
    [SerializeField] float Detectradius = 10;
    [SerializeField] GameObject[] namePanels;



    private void Start()
    {
        StartCoroutine(findNameobj());
      
    }
    IEnumerator findNameobj() 
    {
        yield return 3f.GetDelay();
        namePanels = GameObject.FindGameObjectsWithTag("NamePanel");
        StartCoroutine(DelayFindNamePanel());
    }
    IEnumerator DelayFindNamePanel() 
    {
        while (true)
        {
            yield return 0.3f.GetDelay();

            FindNamePanel();

        }
    }

    private void FindNamePanel() 
    {
        for (int i = 0; i < namePanels.Length; i++)
        {
            if (Detectradius > Vector2.Distance(transform.position, namePanels[i].transform.position))
            {
               
                namePanels[i].layer = 16;
                Vector2 dir = namePanels[i].transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, Detectradius);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == namePanels[i])
                    {
                        GameObject obj = hit.collider.gameObject;
                        obj.layer = 16;


                    }
                }
            }
            else
            {   
             
                namePanels[i].layer = 15;
              
            }
        }
    }
}
