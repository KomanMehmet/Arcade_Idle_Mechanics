using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ObjectControll;

public class Player : MonoBehaviour
{
    public Oven oven;

    Touch touch;
    NavMeshAgent _agent;
    Animator _playerAnim;
    Camera _cam;

    [SerializeField] private Transform _parentStack;
    public List<Transform> collectablePizzasPool = new List<Transform>();

    private bool _isMovable = true;
    private int collectLoopCounter;

    private void Awake()
    {
        _playerAnim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _cam = Camera.main;
    }


    private void Update()
    {
        //Touch_Movement();
        Mouse_Movement();

    }

    private void Touch_Movement()
    {
        if (Input.touchCount > 0)
        {
            Vector3 fingerPosition = Input.GetTouch(0).position;

            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                //playerRB.velocity = new Vector3(
                //    touch.deltaPosition.x * 20f * Time.deltaTime,
                //    transform.position.y,
                //    touch.deltaPosition.y * 20f * Time.deltaTime);

                Plane plane = new Plane(Vector3.up, transform.position);
                Ray ray = _cam.ScreenPointToRay(touch.position);

                if (plane.Raycast(ray, out var distance))
                {
                    _agent.SetDestination(ray.GetPoint(distance));
                    _playerAnim.SetBool("run", true);
                }

                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(_direction.x, 0f, _direction.z), 5f * Time.deltaTime);
                //transform.LookAt(_direction);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _agent.SetDestination(_agent.transform.position);
                _agent.velocity = Vector3.zero;
                _playerAnim.SetBool("run", false);
            }



            //if (_agent.desiredVelocity == Vector3.zero) ;
            //{
            //    _playerAnim.SetBool("run", false);
            //}
        }
    }

    private void Mouse_Movement()
    {
        if (Input.GetMouseButton(0) && _isMovable)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                _agent.SetDestination(ray.GetPoint(distance));
                _playerAnim.SetBool("run", true);
                //_direction = ray.GetPoint(distance);
            }
            

            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(_direction.x, 0f, _direction.z), 5f * Time.deltaTime);
            
            //transform.LookAt(_direction);
        }

        if (_agent.remainingDistance <= 0.2f)
        {
            _playerAnim.SetBool("run", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Yellow_Pizzas"))
        {
            GameManager.pizaCounter += oven.PlayerInteraction();
            oven.setOrderStatus(true);
            collectLoopCounter = oven.GetIndexCounter();
            StartCoroutine(CollectObjectsRoutine(collectablePizzasPool, _parentStack, Color.yellow, collectLoopCounter));
            oven.AllReset();
            oven.SetIndexCounter();
        }


            if (other.gameObject.CompareTag("Stand_Trigger"))
        {
            for (int i = 0; i < collectablePizzasPool.Count; i++)
            {
                if (collectablePizzasPool[i].gameObject.activeInHierarchy)
                {
                    collectablePizzasPool[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public IEnumerator CollectObjectsRoutine(List<Transform> collectableObjects, Transform carryingObject, Color color, int pizzaCount)
    {

        for (int i = 0; i < pizzaCount; i++)
        {
            if (!collectableObjects[i].gameObject.activeInHierarchy)
            {
                collectableObjects[i].gameObject.SetActive(true);
                collectableObjects[i].GetComponent<MeshRenderer>().material.color = color;
                //collectableObjects[i].transform.SetParent(carryingObject);
                collectableObjects[i].rotation = carryingObject.rotation;
                collectableObjects[i].transform.position = carryingObject.position + new Vector3(0, carryingObject.localScale.y, 0) * (i + 1);
                yield return new WaitForSeconds(0.05f);
            } 
        }
    }
}
