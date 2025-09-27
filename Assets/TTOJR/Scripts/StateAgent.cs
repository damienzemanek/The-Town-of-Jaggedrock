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

    public HashSet<IState> HashStates() => states.ToHashSet();

    public void UpdateState(IState hashState, object val)
    {
        HashStates().TryGetValue(hashState, out IState state);
        state.ChangeDataValue(val);
    }
}

[SerializeField]
public interface IState
{
    public void ChangeDataValue(object val);
    public object GetDataValue();
}

[Serializable]
public abstract class State<T> : IState
{
    [field: SerializeField] protected virtual T data { get; set; }
    public void ChangeDataValue(object val) => ChangeDataValue((T)val);
    public void ChangeDataValue(T val) => data = val;
    public object GetDataValue() => data;

    public override bool Equals(object obj) => obj != null && obj.GetType() == GetType();
    public override int GetHashCode() => GetType().GetHashCode();
}

[Serializable]
public class Placing : State<bool>, IState
{
    [field:SerializeField] public bool canPlace { get => data; set => data = value; }

}

