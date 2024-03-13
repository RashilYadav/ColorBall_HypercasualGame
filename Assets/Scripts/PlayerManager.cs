using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    [Range(0f, 1f)] public float maxSpeed;
    [Range(0f, 1f)] public float camSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    [Range(0f, 1000f)] public float ballRotateSpeed;
    private float velocity, camVelocity_x, camVelocity_y;
    private Camera mainCam;
    public Transform path;
    private Rigidbody rb;
    private Collider collider;
    private Renderer BallRenderer;
    public ParticleSystem collideParticle;
    public ParticleSystem airEffect;
    public ParticleSystem dust;
    public ParticleSystem BallTrail;
    public Material[] BallMats = new Material[2];
    public GameObject retryBtn;

    void Start()
    {
        ball = transform;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        BallRenderer = ball.GetChild(1).GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManager.MenuManagerInstance.GameState)
        {
            moveTheBall = true;
            BallTrail.Play();

            // So, overall, this line of code creates a new plane object oriented along the upward direction with its position set at the origin
            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray, out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }

        if (moveTheBall)
        {
            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray, out var distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 MouseNewPos = mouseNewPos - startMousePos;
                Vector3 DesireBallPos = MouseNewPos + startBallPos;

                // we are limiting the position of the ball on the x-axis such that it stays on the surface and does not go beyond that
                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x, -1.5f, 1.5f);

                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesireBallPos.x, ref velocity, maxSpeed), ball.position.y, ball.position.z);
            }
        }

        if (MenuManager.MenuManagerInstance.GameState)
        {
            var pathNewPos = path.position;

            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));

            ball.GetChild(1).Rotate(Vector3.right * ballRotateSpeed * Time.deltaTime);
        }

    }

    private void LateUpdate()
    {
        var CameraNewPos = mainCam.transform.position;
        if (rb.isKinematic)
        {
            mainCam.transform.position = new Vector3(Mathf.SmoothDamp(CameraNewPos.x, ball.transform.position.x, ref camVelocity_x, camSpeed), Mathf.SmoothDamp(CameraNewPos.y, ball.transform.position.y + 3f, ref camVelocity_y, camSpeed), CameraNewPos.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            gameObject.SetActive(false);
            MenuManager.MenuManagerInstance.GameState = false;
            MenuManager.MenuManagerInstance.menuElement[2].SetActive(true);
            MenuManager.MenuManagerInstance.menuElement[2].transform.GetChild(0).GetComponent<Text>().text = "You Lose";
        }

        switch(other.tag)
        {
            case "red":
                other.gameObject.SetActive(false);
                BallMats[1] = other.GetComponent<Renderer>().material;
                BallRenderer.materials = BallMats;
                var NewParticle = Instantiate(collideParticle, transform.position, Quaternion.identity);
                NewParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrailColor = BallTrail.trails;
                BallTrailColor.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

            case "green":
                other.gameObject.SetActive(false);
                BallMats[1] = other.GetComponent<Renderer>().material;
                BallRenderer.materials = BallMats;
                var NewParticle1 = Instantiate(collideParticle, transform.position, Quaternion.identity);
                NewParticle1.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail_1 = BallTrail.trails;
                BallTrail_1.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

            case "yellow":
                other.gameObject.SetActive(false);
                BallMats[1] = other.GetComponent<Renderer>().material;
                BallRenderer.materials = BallMats;
                var NewParticle2 = Instantiate(collideParticle, transform.position, Quaternion.identity);
                NewParticle2.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail_2 = BallTrail.trails;
                BallTrail_2.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;

            case "blue":
                other.gameObject.SetActive(false);
                BallMats[1] = other.GetComponent<Renderer>().material;
                BallRenderer.materials = BallMats;
                var NewParticle3 = Instantiate(collideParticle, transform.position, Quaternion.identity);
                NewParticle3.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                var BallTrail_3 = BallTrail.trails;
                BallTrail_3.colorOverLifetime = other.GetComponent<Renderer>().material.color;
                break;
        }

        if(other.gameObject.name.Contains("ColorBall"))
        {
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 1);
            MenuManager.MenuManagerInstance.menuElement[1].GetComponent<Text>().text = PlayerPrefs.GetInt("score").ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("path"))
        {
            if(other.gameObject.name.Contains("Path (2)"))
            {
                gameObject.SetActive(false);
                retryBtn.SetActive(true);

                MenuManager.MenuManagerInstance.GameState = false;
                MenuManager.MenuManagerInstance.menuElement[2].SetActive(true);
                MenuManager.MenuManagerInstance.menuElement[2].transform.GetChild(0).GetComponent<Text>().text = "You Won!";
            }

            rb.isKinematic = false;
            collider.isTrigger = false;
            rb.velocity = new Vector3(0f, 8f, 0f);
            pathSpeed = pathSpeed * 2;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 10f;
            BallTrail.Stop();
            ballRotateSpeed = 1000f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("path"))
        {
            rb.isKinematic = collider.isTrigger = true;
            pathSpeed = 30f;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 4f;

            dust.transform.position = collision.contacts[0].point + new Vector3(0f, 0.3f, 0f);
            dust.GetComponent<Renderer>().material = BallRenderer.material;
            dust.Play();
            BallTrail.Play();
            ballRotateSpeed = 500f;
        }
    }
}
