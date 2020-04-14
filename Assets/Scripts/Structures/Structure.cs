using UnityEngine;
using MoonSharp.Interpreter;

namespace RTS
{
    /// <summary>
    /// Parent class for types of buildings.
    /// </summary>
    public class Structure : MonoBehaviour
    {
        public int MaxHP { get; protected set; }
        public int HP { get; protected set; }

        [Tooltip("The file to read stats from")]
        [SerializeField]
        private string _luaFile;

        public string BuildingType { get; protected set; } = "UNSETBUILDINGNAME";   //TODO fix this

        private void Awake()
        {

            //initialize data
            ReadStatsFromFile();

            //If I have an inventory add me to the list of buildings with inventories
            if (gameObject.GetComponent<Inventory>() != null)
            {
                WorldInfo.Depots.Add(this.gameObject);
            }
        }

        private void Update()
        {
            //if dead die
            if (HP == 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Reads the building's stats from file.
        /// </summary>
        private void ReadStatsFromFile()
        {
            //open lua file
            Script script = new Script();
            var buildingTable = script.DoFile(_luaFile).Table;
            HP = (int)buildingTable.Get("_hp").Number;
            MaxHP = (int)buildingTable.Get("_hp").Number;
        }

        /// <summary>
        /// Dies.
        /// </summary>
        public void Die()
        {
            //remove resource from global resource list
            WorldInfo.Resources.Remove(gameObject);

            //destroy the gameobject
            Destroy(gameObject);
        }
    }
}