using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class TankRapariga : MonoBehaviour
{
    private TankAI tankai;

    // Start is called before the first frame update
    void Start()
    {
        tankai = GetComponent<TankAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Parede(6.0f))
        {
            Debug.Log("PAREDE!!!");
        }
        else
        {
            Debug.Log("LIN");
        }

        if(tankai.Targets != null)
        {
            tankai.TurretLookAt(tankai.Targets[0]);
        }
    }

    [Task]
    public void PickRandomDestination()
    {
        tankai.Agent.ResetPath();
        Vector3 destination = new Vector3(Random.Range(-35.0f, 35.0f), 0.0f, Random.Range(-35.0f, 35.0f));
        tankai.Agent.SetDestination(destination);
        Task.current.Succeed();
    }

    [Task]
    public bool IsHealthLessThan(float health)
    {
        return tankai.Health < health;
    }

    [Task]
    public bool InDanger(float minDistance)
    {
        return Vector3.Distance(tankai.Targets[0], transform.position) <= minDistance;
    }

    [Task]
    public bool NotDanger(float minDistance)
    {
        return Vector3.Distance(tankai.Targets[0], transform.position) > minDistance;
    }

    [Task]
    public void explosao()
    {
        tankai.SelfDestruction();
        Task.current.Succeed();
    }

    [Task]
    public bool ninguem()
    {
        return tankai.Targets.Count == 0;
    }

    [Task]
    public bool haum()
    {
        return tankai.Targets.Count == 1;
    }

    [Task]
    public bool doisoumais()
    {
        return tankai.Targets.Count >= 2;
    }

    [Task]
    public void Atacar()
    {
        tankai.LookAt(tankai.Targets[0]);
        tankai.StartFire();
        Task.current.Succeed();   
    }

    [Task]
    public void Parar()
    {
        tankai.TurretRotate(0.0f);
        tankai.StopFire();
        Task.current.Succeed();

    }

    [Task]
    public void MoveDestination()
    {
        //Arrived at destinantion
        if (tankai.Agent.remainingDistance <= tankai.Agent.stoppingDistance && !tankai.Agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public bool Parede(float min)
    {
        int layer = LayerMask.GetMask("Obstacles");
        Ray ray = new Ray(tankai.Position, tankai.TurretDirection);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * min, Color.blue);
        return Physics.Raycast(ray, min, layer);   
    }

    [Task]
    public bool Player(float min)
    {
        int layer = LayerMask.GetMask("Players");
        Ray ray = new Ray(tankai.TurretDirection, tankai.transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * min, Color.blue);
        return Physics.Raycast(ray, min, layer);
    }

    [Task]
    public void Mover(float i)
    {
        tankai.Agent.ResetPath();
        tankai.Move(i);
        Task.current.Succeed();
    }

    [Task]
    public void Rotate(float i)
    {
        tankai.Rotate(i);
        Task.current.Succeed();
    }

    [Task]
    public void RotateForPlayer()
    {
        tankai.LookAt(tankai.Targets[0]);
        Task.current.Succeed();
    }

    [Task]
    public void Stop()
    {
        tankai.Agent.isStopped = true;
        Task.current.Succeed();
    }
}