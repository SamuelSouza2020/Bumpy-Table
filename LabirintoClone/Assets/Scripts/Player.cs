using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody rig;
    float tiltX, tiltY, sensib = 10;
    public bool fora = true, mov = false;
    [SerializeField]
    Vector3 nPos;
    CamGame gameCam;

    GameObject canvasTP;
    Text tTempSt;

    AudioSource aud;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        gameCam = GameObject.Find("Main Camera").GetComponent<CamGame>();
        tTempSt = GameObject.Find("TempoStart").GetComponent<Text>();
        canvasTP = GameObject.Find("CanvasTp");

        aud = GetComponent<AudioSource>();
        mov = false;
    }
    void Update()
    {
        if(gameCam.ok)
        {
            if (fora)
            {
                if(rig.velocity.magnitude >= 0.2 && !mov)
                {
                    aud.Play();
                    mov = true;
                }
                if(rig.velocity.magnitude <= 0.2)
                {
                    aud.Stop();
                    mov = false;
                }
                tiltX = Input.acceleration.x * sensib;
                tiltY = Input.acceleration.y * sensib;
                rig.velocity = new Vector3(tiltX, rig.velocity.y, tiltY);
            }
            else
            {
                rig.velocity = new Vector3(0, 0, 0);
                transform.position = Vector3.Lerp(transform.position, new Vector3(nPos.x, transform.position.y, nPos.z), 3 * Time.deltaTime);
                gameObject.GetComponent<Collider>().enabled = false;
                rig.velocity = new Vector3(0, -1, 0);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Buraco"))
        {
            Debug.Log("EntrouA");
            nPos = new Vector3(other.transform.position.x, transform.position.y,
                other.transform.position.z);
            rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y, rig.velocity.z);
            fora = false;
        }
        if (other.gameObject.CompareTag("BuracoFim"))
        {
            nPos = new Vector3(other.transform.position.x, transform.position.y,
                other.transform.position.z);
            rig.velocity = new Vector3(rig.velocity.x, rig.velocity.y, rig.velocity.z);
            StartCoroutine(PerdeuGm());
            canvasTP.SetActive(true);
            tTempSt.text = "PERDEU";
            fora = false;
        }
    }
    IEnumerator PerdeuGm()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
