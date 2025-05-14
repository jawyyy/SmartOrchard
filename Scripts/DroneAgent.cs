using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

using static DroneController;

public class DroneAgent : Agent
{
    //public Vector3 initPos = new Vector3(0f, 3.3f, 0f);
    //public Quaternion initRot = Quaternion.identity;
    private string agent_id;

    public BehaviorParameters bps;
    public Rigidbody rb;
    public Transform tr;

    public DroneController droneCtrl;
    public GameObject target;
    public GameObject alternative;
    public GameObject peerAgent;

    public override void Initialize()
    {
        bps = GetComponent<BehaviorParameters>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        droneCtrl = GetComponent<DroneController>();
        if (agent_id == "DroneAgent1") {
            peerAgent = GameObject.Find("DroneAgent2");
        }
        else peerAgent = GameObject.Find("DroneAgent1");
        
    }

    public override void OnEpisodeBegin()
    {
        //tr.localPosition = initPos + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        //tr.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (target != null)
        {
            //target.localPosition = new Vector3(Random.Range(-4f, 4f), 1.5f, Random.Range(-4f, 4f));
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(tr.localPosition);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(rb.angularVelocity);

        if (target != null)
        {
            sensor.AddObservation(target.localPosition - tr.localPosition);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.ContinuousActions;

        droneCtrl.m = action[0];
        droneCtrl.s = action[1];
        droneCtrl.a = action[2];
        droneCtrl.y = action[3];

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 테스트용 수동 입력
        var action = actionsOut.ContinuousActions;

        float forward = Input.GetAxis("Vertical"); //z
        float strafe = Input.GetAxis("Horizontal"); //x
        float altitude = Input.GetKey(KeyCode.Space) ? 1f : Input.GetKey(KeyCode.LeftShift) ? -1f : 0f; // y
        float yaw = Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.E) ? 1f : 0f; // yaw

        action[0] = forward;
        action[1] = strafe;
        action[2] = altitude;
        action[3] = yaw;
    }    

    // Start is called before the first frame update
    void Start()
    {
        agent_id = this.gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
