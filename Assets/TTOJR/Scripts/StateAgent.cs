using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor.XR;
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
    public void Use(object obj) => functionality.Use((T)obj);
    [field: SerializeField] protected virtual T data { get; set; }
    public void ChangeDataValue(object val) => ChangeDataValue((T)val);
    public void ChangeDataValue(T val) => data = val;
    public object GetDataValue() => data;

    public override bool Equals(object obj) => obj != null && obj.GetType() == GetType();
    public override int GetHashCode() => GetType().GetHashCode();
}

[Serializable]
public abstract class RequiredItem
{
    [field: SerializeField] public abstract bool hasRequiredItem { get; set; }
    [field: SerializeField] public abstract Item requiredItem { get; set; }
    public abstract void SetHasRequiredItem(bool val);
}


[Serializable]
public abstract class Uses : RequiredItem
{
    [field: SerializeField] public virtual bool usedUp { get; set; }
    [field: SerializeField] public virtual int uses { get; set; }
    [field: SerializeField] public virtual int initialUses { get; set; }
    public virtual void Use() { uses--; if (uses <= 0) RunOutOfUses(); }
    public virtual void RunOutOfUses() { usedUp = true; }
    public virtual void UsesInit() { uses = initialUses; usedUp = false; }
}


[Serializable]
public class CanPlace : State<CanPlace.Data>, IState
{
    Place _functionality = new Place();
    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

    [Serializable]
    public class Data : Uses
    {
        [field: SerializeField] public override bool hasRequiredItem { get; set; }
        [field: SerializeField] public override Item requiredItem { get; set; }

        public bool canPlace;
        public Transform placeLocation;
        public GameObject objectToPlace;

        public override void SetHasRequiredItem(bool val) => hasRequiredItem = val;
        public void SetCanPlace(bool val) => canPlace = val;
        public void SetPlaceLocation(Transform val) => placeLocation = val;
        public void SetObjectToPlace(GameObject val) => objectToPlace = val;

    }

}


[Serializable]
public class CanSpray : State<CanSpray.Data>, IState
{
    Spray _functionality = new Spray();
    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

    [Serializable]
    public class Data : RequiredItem
    {
        [field: SerializeField] public override bool hasRequiredItem { get; set; }
        [field: SerializeField] public override Item requiredItem { get; set; }
        public bool canSpray;
        public Destination sprayDestination;
        public override void SetHasRequiredItem(bool val) => hasRequiredItem = val;
        public void SetCanSpray(bool val) => canSpray = val;
        public void SetSprayLocation(Destination val) => sprayDestination = val;
    }

}


