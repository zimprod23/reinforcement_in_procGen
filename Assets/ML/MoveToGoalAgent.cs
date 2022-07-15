using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Material winMat;
    [SerializeField] Material loseMat;
    [SerializeField] MeshRenderer floorMesh;

    Vector3 originalPos ;//new Vector3(222.6548f,106.4f,-693.4116f);
    void Awake(){
         originalPos = this.transform.localPosition;
    }
    public float speed = 30f;
    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = originalPos;
    }
    public override void CollectObservations(VectorSensor sensor){
         sensor.AddObservation(this.transform.localPosition);
         sensor.AddObservation(targetTransform.transform.localPosition);
    }

public override void Heuristic(in ActionBuffers actionsOut){
     ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
     continuousActions[0] = Input.GetAxis("Horizontal");
     continuousActions[1] = Input.GetAxis("Vertical");
}

  public override void OnActionReceived(ActionBuffers actions){
     float moveX = actions.ContinuousActions[0];
     float moveZ = actions.ContinuousActions[1];

     transform.localPosition += new Vector3(moveX,0,moveZ) * speed * Time.deltaTime;
  }
 
   void OnTriggerEnter(Collider other){
     if(other.gameObject.name == "Sphere"){
        floorMesh.material = winMat;
        SetReward(1f);
        EndEpisode();
     }else if(other.tag == "Wall"){
       floorMesh.material = loseMat;
        SetReward(-1f);
        EndEpisode();
     }
     
  }
}
