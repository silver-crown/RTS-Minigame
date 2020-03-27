using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using Yeeter;

namespace RTS
{
    /// <summary>
    /// Abstract class for all gatherable resources to derive from.
    /// </summary>
    public class Resource : MonoBehaviour
    {
        [SerializeField]
        public int HP { get; protected set; } = 40;
        public int MaxHP { get; protected set; } = 40;

        public string ResourceType { get; protected set; }

        [Tooltip("The file to read stats from")]
        [SerializeField]
        private string _luaFile;
        private LuaObjectComponent _luaObjectComponent;

        //Action used to update health bar
        public System.Action OnMined;
        public System.Action OnDestroyed;

        private void Awake()
        {
            //initialize data
            _luaObjectComponent = GetComponent<LuaObjectComponent>();
            _luaObjectComponent.Load(_luaFile);
            MaxHP = (int)_luaObjectComponent.Table.Get("_maxHP").Number;
            HP = MaxHP;
            ResourceType = _luaObjectComponent.Table.Get("_resourceType").String;


            //add self to resource list
            WorldInfo.Resources.Add(gameObject);
        }

        private void Update()
        {
            if (HP == 0)
            {
                Die();
            }

            if (Input.GetKeyDown("m"))
            {
                Debug.Log(Mine(11));
                Debug.Log(HP);
            }
        }

        /// <summary>
        /// Takes a request of how much to mine the resource for. Returns how much you managed to mine.
        /// </summary>
        /// <param name="amount">How much is requested from the resource </param>
        /// <returns></returns>
        public int Mine(int amount)
        {
            int amountMined = 0;

            //Decrease the health of the resource by the amount requested if possible, if not deplete it fully. Return the amount mined for.
            amountMined = System.Math.Min(HP, amount);
            HP -= amountMined;
            //Invoke mining action so subscribers (the healthbar) can do their thing
            OnMined?.Invoke();

            return amountMined;
        }

        /// <summary>
        /// Destroy object and delete it from lists.
        /// </summary>
        public void Die()
        {
            //remove resource from global resource list
            WorldInfo.Resources.Remove(gameObject);

            OnDestroyed?.Invoke();

            //destroy the gameobject
            Destroy(gameObject);
        }
    }
}