using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public bool moveForward;

    public GameObject gLeft;
    public GameObject gRight;
    public float tCanBang = 0;

    public Vector3 mouseDown;
    public float xOffsetPlayer;

    public ParticleSystem effectWindCam;


    private void Update()
    {
        //print(Application.platform);

        switch (Manager.manager.stateGame) {
            case Manager.StateGame.PressToPlay:
            if (transform.eulerAngles != Vector3.zero) {
                transform.eulerAngles = Vector3.zero;
            }

            if (Input.touchCount == 1) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    Manager.manager.ToPlay();
                    mouseDown = Input.mousePosition;
                    xOffsetPlayer = transform.position.x;
                    moveForward = true;

                    Manager.manager.fillAmount.fillAmount = 0;
                    Manager.manager.fillAmount.DOFillAmount(1, 15);
                }
            }
            break;
            case Manager.StateGame.Playing:
            int zRotLeft = (int)gLeft.transform.eulerAngles.z;
            int zRotRight = (int)gRight.transform.eulerAngles.z;

            if (zRotLeft > 20 && zRotLeft < 340 || zRotRight > 20 && zRotRight < 340) {
                if (transform.position.z < Manager.manager.zPosEndgame) {
                    LosePlayer();
                    Manager.manager.LoseGame();
                } else {
                    if (gLeft.activeInHierarchy && gRight.activeInHierarchy) {

                        GameObject g = Instantiate(Resources.Load("Tmp Cay Ngang") as GameObject);
                        g.transform.parent = GameObject.Find("T Temp").transform;
                        g.transform.position = transform.position;
                        g.transform.eulerAngles = new Vector3(0, 0, 90);
                        g.transform.localScale = new Vector3(.25f, (gLeft.transform.localScale.x + gRight.transform.localScale.x) / 2, .25f);
                        g.GetComponent<Rigidbody>().AddForce(new Vector3(
                            Random.Range(-10f, 10f),
                            Random.Range(10f, 20f),
                            Random.Range(-10f, 10f)) * 10);

                        gLeft.SetActive(false);
                        gRight.SetActive(false);
                    }
                }
            }

            if (Input.touchCount == 1) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    mouseDown = Input.mousePosition;
                    xOffsetPlayer = transform.position.x;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary) {
                    float x = (Input.mousePosition - mouseDown).x;
                    float newPosX = x;

                    Ray r = new Ray(transform.position, -transform.up);
                    RaycastHit hit;
                    if (Physics.Raycast(r, out hit, 1f, 1 << 12)) {
                        newPosX = Mathf.Clamp(xOffsetPlayer + newPosX * .01f, -2, 2);
                    } else {
                        newPosX = Mathf.Clamp(xOffsetPlayer + newPosX * .01f, -4, 4);
                    }

                    transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
                }
            }

            if (Mathf.Abs(gLeft.transform.localScale.x - gRight.transform.localScale.x) >= .05f) {
                if (tCanBang > 0) {
                    tCanBang -= Time.deltaTime;
                }

                if (tCanBang <= 0) {
                    tCanBang = 1;
                    StartCoroutine(CanBangCo());
                }
            }

            if (moveForward) {

                Ray r = new Ray(transform.position, -transform.up);
                RaycastHit hit;


                if (Physics.Raycast(r, out hit, 1f, 1 << 12)) {
                    rb.constraints = (RigidbodyConstraints)112;

                    rb.velocity = new Vector3(0, rb.velocity.y, speed);
                    if (effectWindCam.isPlaying) {
                        effectWindCam.Stop();
                    }
                } else {
                    rb.constraints = (RigidbodyConstraints)48;

                    rb.velocity = new Vector3(0, rb.velocity.y, speed * 1.5f);
                    if (!effectWindCam.isPlaying) {
                        effectWindCam.Play();
                    }

                    if (transform.position.x <= -2.5f || transform.position.x >= 2.5f) {

                        LosePlayer();
                        Manager.manager.LoseGame();
                    }
                }
            }

            Check();
            break;
            case Manager.StateGame.PauseGame:
            break;
            case Manager.StateGame.WinLose:
            if (Input.touchCount == 1) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    ResetPlayer();
                    Manager.manager.SetStartLevel();
                }
            }
            break;
        }
    }

    void ResetPlayer()
    {
        if (effectWindCam.isPlaying) {
            effectWindCam.Stop();
        }

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, .5f, 8);
        transform.eulerAngles = Vector3.zero;

        if (!gLeft.activeInHierarchy) {
            gLeft.SetActive(true);
        }
        if (!gRight.activeInHierarchy) {
            gRight.SetActive(true);
        }

        gLeft.transform.localScale = new Vector3(1, .25f, .25f);
        gRight.transform.localScale = new Vector3(1, .25f, .25f);
    }

    void LosePlayer()
    {
        if (effectWindCam.isPlaying) {
            effectWindCam.Stop();
        }

        rb.velocity = Vector3.zero;
        GameObject g = Instantiate(Resources.Load("Tmp Cay Ngang") as GameObject);
        g.transform.parent = GameObject.Find("T Temp").transform;
        g.transform.position = transform.position;
        g.transform.eulerAngles = new Vector3(0, 0, 90);
        g.transform.localScale = new Vector3(.25f, (gLeft.transform.localScale.x + gRight.transform.localScale.x) / 2, .25f);
        g.GetComponent<Rigidbody>().AddForce(new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(10f, 20f),
            Random.Range(-10f, 10f)) * 10);

        gLeft.SetActive(false);
        gRight.SetActive(false);
    }

    public void AddScore()
    {
        Manager.manager.scoreDiamond += 1;
        Manager.manager.textScoreDiamond.text = Manager.manager.scoreDiamond.ToString();
    }

    public void Add(float i)
    {
        Manager.manager.scoreAdd++;
        Manager.manager.textScoreAdd.text = Manager.manager.scoreAdd.ToString();

        tCanBang = 1;

        float a = i / 5;

        GameObject t = Instantiate(Resources.Load("Text Add") as GameObject, GameObject.Find("Canvas").transform);
        t.GetComponent<RectTransform>().DOAnchorPos(t.GetComponent<RectTransform>().anchoredPosition + Vector2.up * 100, .45f);
        t.GetComponent<Text>().text = "+" + i;
        Destroy(t, .5f);

        Vector3 vL = gLeft.transform.localScale;
        vL.x += a;
        Vector3 vR = gRight.transform.localScale;
        vR.x += a;

        gLeft.transform.localScale = vL;
        gRight.transform.localScale = vR;
    }

    void Check()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(.25f, .5f, .25f));
        {
            foreach (var item in colliders) {
                if (item.gameObject.layer == 7) {// cham luoi cua => thua
                    LosePlayer();
                    Manager.manager.LoseGame();
                }
                if (item.gameObject.layer == 9) {
                    Add(item.transform.localScale.y == .25f ? 1 : 2);

                    Destroy(item.gameObject);
                }
                if (item.gameObject.layer == 11) { // cham cay doc => thua
                    LosePlayer();
                    Manager.manager.LoseGame();
                }

            }
        }

        Collider[] colliders2 = Physics.OverlapBox(transform.position, new Vector3(.35f, .55f, .35f));
        {
            foreach (var item in colliders2) {
                if (item.gameObject.layer == 10) {
                    if (Manager.manager.stateGame == Manager.StateGame.Playing) {

                        moveForward = false;
                        rb.velocity = Vector3.zero;

                        Manager.manager.textMultiScore.text = "X" + item.gameObject.name;
                        int total = (Manager.manager.scoreAdd * System.Convert.ToInt32(item.name));

                        Manager.manager.textToalScore.text = total.ToString();
                        Manager.manager.scoreDiamond += total;
                        Manager.manager.textScoreDiamond.text = Manager.manager.scoreDiamond.ToString();

                        Manager.manager.WinGame();
                    }
                }
            }
        }
    }

    IEnumerator CanBangCo()
    {
        while (Mathf.Abs(gLeft.transform.localScale.x - gRight.transform.localScale.x) >= .05f) {
            if (gLeft.transform.localScale.x > gRight.transform.localScale.x) {
                Vector3 vL = gLeft.transform.localScale;
                vL.x -= .05f;
                gLeft.transform.localScale = vL;

                Vector3 vR = gRight.transform.localScale;
                vR.x += .05f;
                gRight.transform.localScale = vR;
            } else {
                Vector3 vL = gLeft.transform.localScale;
                vL.x += .05f;
                gLeft.transform.localScale = vL;

                Vector3 vR = gRight.transform.localScale;
                vR.x -= .05f;
                gRight.transform.localScale = vR;
            }

            yield return new WaitForSeconds(.01f);
        }
    }
}
