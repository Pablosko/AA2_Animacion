using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


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

                relativePositions[i] = i < 1 ? Vector3.zero : LegTargets[i].InverseTransformPoint(LegTargets[i - 1].position);
                Debug.Log(relativePositions[i]);
            }
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
            MyTentacleController leg = _legs[i];
            Transform[] legBones = leg.Bones;

            if (legBones != null && legBones.Length > 0)
            {
                for (int j = 0; j < legBones.Length - 1; j++)
                {
                    Transform currentBone = legBones[j];
                    Transform nextBone = legBones[j + 1];

                    float distanceToBase = Vector3.Distance(currentBone.position, legFutureBases[i].position);

                    // Si la distancia es mayor que un umbral, movemos el hueso hacia el Future Base
                    if (distanceToBase <= 1.0f)
                    {
                        Vector3 newPosition = Vector3.Lerp(currentBone.position, legFutureBases[i].position, distanceToBase);
                        currentBone.position = newPosition;

                    }
                    Vector3 direction = (nextBone.position - currentBone.position).normalized;
                    nextBone.position = currentBone.position + direction * nextBone.position.magnitude;
                }
            }
        }
            #endregion
        }
    }
}
