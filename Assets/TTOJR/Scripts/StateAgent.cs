using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class StateAgent : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public StateAgent Provide()
    {
        return this;
    }
    
    [SerializeReference] public List<IState> states = new();
    public HashSet<IState> hashStates;

    private void Start()
    {
        hashStates = states.ToHashSet();
    }

    public void UpdateState(IState hashState, object val)
    {
        hashStates.TryGetValue(hashState, out IState state);
        state.ChangeDataValue(val);
    }

    public IState GetState(IState state)
    {
        hashStates.TryGetValue(state, out var outState);
        print($"StateAgent: Attempted to get state: {outState}.");
        return outState;
    }

}

[field: Serializable]
public interface IState
{
    public void Use(object obj);
    public void ChangeDataValue(object val);
    public object GetDataValue();
}

[Serializable]
public abstract class State<T> : IState
{
    [field: SerializeField] public abstract UseFunctionality<T> functionality { get; }
    public void Use(object obj)
    {
        Debug.Log(functionality);
        Debug.Log(obj);
        functionality.Use((T)obj);
    }
    [field: SerializeField] protected virtual T data { get; set; }
    public void ChangeDataValue(object val) => ChangeDataValue((T)val);
    public void ChangeDataValue(T val) => data = val;
    public object GetDataValue() => data;

    public override bool Equals(object obj) => obj != null && obj.GetType() == GetType();
    public override int GetHashCode() => GetType().GetHashCode();
}

[Serializable]
public class CanPlace : State<CanPlace.Data>, IState
{
    Place _functionality = new Place();
    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

    [Serializable]
    public struct Data
    {
        public bool canPlace;
        public Transform placeLocation;
        public GameObject objectToPlace;
    }

}

