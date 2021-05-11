using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CayNgang : MonoBehaviour
{
    public Player p;

    public ParticleSystem effect;

    public enum Dir
    {
        Left,
        Right
    }

    public Dir dir;

    private void Update()
    {
        Vector3 halfExtents = new Vector3(transform.localScale.x / 2, .25f, .25f);
        Collider[] colliders = Physics.OverlapBox(transform.GetChild(0).position, halfExtents);

        bool bStick = false;
        Vector3 vEffect = Vector3.zero;

        foreach (var item in colliders) {

            if (item.gameObject.layer == 6) {
                GameObject e = Instantiate(Resources.Load("Effect Touch Diamond") as GameObject, item.transform.position, Quaternion.identity);
                Destroy(e, .5f);

                p.AddScore();
                Destroy(item.gameObject);

                GameObject uiDiamond = Instantiate(Resources.Load("Temp Diamond Collect") as GameObject, GameObject.Find("Canvas").transform);
                uiDiamond.GetComponent<RectTransform>().DOAnchorPos(new Vector3(280, 1100), 1f);
                Destroy(uiDiamond, 1.1f);

            }
            if (item.gameObject.layer == 7) {
                Vector3 v = item.ClosestPointOnBounds(transform.position);


                float n = Mathf.Abs(v.x - transform.position.x);

                if (n / 2 < transform.localScale.x) {
                    float xCuoiCay = transform.position.x;
                    switch (dir) {
                        case Dir.Left:
                        xCuoiCay += -transform.localScale.x;
                        break;
                        case Dir.Right:
                        xCuoiCay += transform.localScale.x;
                        break;
                    }

                    transform.localScale = new Vector3(n - .01f, .25f, .25f);

                    GameObject g = Instantiate(Resources.Load("Tmp Cay Ngang") as GameObject,
                        new Vector3((xCuoiCay + v.x) / 2, transform.position.y, transform.position.z), Quaternion.identity, GameObject.Find("T Temp").transform);
                    g.transform.localScale = new Vector3(.25f, Mathf.Abs(xCuoiCay - v.x) / 2, .25f);
                    g.transform.eulerAngles = new Vector3(0, 0, 90);
                }

                p.tCanBang = 1;
            }

            if (item.gameObject.layer == 11) {

                bStick = true;
                vEffect = item.ClosestPointOnBounds(transform.position);
            }
        }

        if (bStick) {
            p.tCanBang = 1;

            Vector3 pEffet = effect.transform.position;
            pEffet.x = vEffect.x;

            effect.transform.position = pEffet;

            if (!effect.isPlaying) {
                effect.Play();
            }

            //if (name == "Left") {
            //    p.stickLeft = true;
            //} else if (name == "Right") {
            //    p.stickRight = true;
            //}
        } else {
            if (effect.isPlaying) {
                effect.Stop();
            }

            //if (name == "Left") {
            //    p.stickLeft = false;
            //} else if (name == "Right") {
            //    p.stickRight = false;
            //}
        }
    }
}
