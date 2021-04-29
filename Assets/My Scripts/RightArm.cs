using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArm : MonoBehaviour
{
    public AvocadoController Avocado;
    public HingeJoint2D hj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Avocado.isFacingRight())
        {
            this.gameObject.transform.rotation.eulerAngles.Set(0f, 0f, 30f);

            JointMotor2D nextMotor = new JointMotor2D();
            nextMotor.maxMotorTorque = 0.005f;
            nextMotor.motorSpeed = 400f;

            JointAngleLimits2D nextLimits = new JointAngleLimits2D();
            nextLimits.max = -30f;
            nextLimits.min = -60f;

            hj.motor = nextMotor;
            hj.limits = nextLimits;
        }
        else
        {
            this.gameObject.transform.rotation.eulerAngles.Set(0f, 0f, -30f);

            JointMotor2D nextMotor = new JointMotor2D();
            nextMotor.maxMotorTorque = 0.005f;
            nextMotor.motorSpeed = -400f;

            JointAngleLimits2D nextLimits = new JointAngleLimits2D();
            nextLimits.max = 60f;
            nextLimits.min = 30f;

            hj.motor = nextMotor;
            hj.limits = nextLimits;
        }
    }
}
