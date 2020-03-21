using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;
using org.mariuszgromada.math.mxparser;


namespace ssuai
{

    /// <summary>
    ///Factor for amount of drones assigned to a certain task, set at factor creaton
    /// </summary>
    public class AssignedDronePercentage : Factor
    {
        private CentralIntelligence _ci;
        private string _task;
        private string _droneName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CI">the central intelligence</param>
        /// <param name="function">mathematical function for calculating utility</param>
        /// <param name="droneType"> name of drone, as written in the _name field of Lua file. </param>
        /// <param name="task">the task to get the number of drones doing (as written in _status in the Lua file) </param>
        public AssignedDronePercentage(CentralIntelligence CI, string function, string droneName, string task)
        {
            MathFunction = new Function(function);
            _ci = CI;
            _droneName = droneName;
            _task = task;
        }

        public override void UpdateUtility()
        {
            //number of drones of the correct type
            int noOfDrones = 0;
            //number of drones of the correct type assigned to the correct job
            int noOfAssignedDrones = 0;

            foreach (Drone drone in _ci.Drones)
            {
                //if the drone is of the correct type
                if (drone.GetValue("_name").String == _droneName)
                {
                    //if the drones is doing the correct job
                    if (drone.GetValue("_status").String == _task)
                    {
                        noOfAssignedDrones++;
                    }
                    noOfDrones++;
                }
            }

            //how many percent of the drones of this type are doing this job?
            float percentageAssignedToJob = (float)noOfAssignedDrones / (float)noOfDrones;

            //calculate utility
            _utility = (float)MathFunction.calculate(percentageAssignedToJob);
        }
    }
}