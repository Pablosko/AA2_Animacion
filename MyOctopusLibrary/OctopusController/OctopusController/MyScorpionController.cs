using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace OctopusController
{
    public class MyScorpionController
    {
        //TAIL
        Transform tailTarget;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        Vector3[] relativePositions;
        MyTentacleController[] _legs = new MyTentacleController[6];
        bool movingLegs;

        float legLenght;

        #region public
        public void InitLegs(Transform[] LegRoots, Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            legTargets = new Transform[LegTargets.Length];
            legFutureBases = new Transform[LegTargets.Length];
            relativePositions = new Vector3[LegTargets.Length];
            //Legs init
            for (int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                legFutureBases[i] = LegFutureBases[i];
                legTargets[i] = LegTargets[i];

                relativePositions[i] = i < 1 ? Vector3.zero : LegTargets[i].InverseTransformPoint(LegTargets[i - 1].position);
                Debug.Log(relativePositions[i]);
            }
            legLenght = (_legs[0].Bones[_legs[0].Bones.Length - 1].position - _legs[0]._endEffectorSphere.position).magnitude;
        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //TODO: Initialize anything needed for the Gradient Descent implementation
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {

        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {
            Debug.Log("START WALKING");
            movingLegs = true;
        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            if(movingLegs)
                updateLegs();
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            //
        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {

        }
        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {
             for (int i = 0; i < _legs.Length; i++)
             {
                Vector3[] Transforms = new Vector3[_legs[i].Bones.Length];

                if ((legFutureBases[i].position - legTargets[i].position).magnitude > (legLenght* _legs[i].Bones.Length))
                {
                    Vector3 direccion2 = (legTargets[i].position -legFutureBases[i].position);
                    direccion2.Normalize();

                    for(int e = 0; e < _legs[i].Bones.Length; e++)
                    {
                        _legs[i].Bones[e].position = legTargets[i].position + (-direccion2*(legLenght* (_legs[i].Bones.Length -e)));
                        if(e+1 != _legs[i].Bones.Length)
                            LookAt( ref _legs[i].Bones[e], ref _legs[i].Bones[e+1]);
                        else
                        {
                            LookAt(ref _legs[i].Bones[e], ref legTargets[i]);

                        }
                    }


                }
  
               

             }
            #endregion
        }

        void LookAt(ref Transform start, ref Transform end)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 directionToLook = end.position - start.position;

            // Ajusta la dirección para ignorar la componente Y
            directionToLook.y = 0f;

            // Normaliza la dirección para evitar problemas de escala
            directionToLook.Normalize();

            // Calcula la rotación basada en la dirección, utilizando el eje "up" como dirección de "up"
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.up, directionToLook);

            // Aplica la rotación al transform actual
            start.rotation = lookRotation;
        }
    }
}
