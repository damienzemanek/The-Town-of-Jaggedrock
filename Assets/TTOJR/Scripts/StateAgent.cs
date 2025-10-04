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
    //[Provide]
    //public StateAgent Provide()
    //{
    //    return this;
    //}
    
    //[SerializeReference] public List<IState> states = new();
    //public HashSet<IState> hashStates;

    //private void Start()
    //{
    //    hashStates = states.ToHashSet();
    //}

    //public void UpdateState(IState hashState, object val)
    //{
    //    hashStates.TryGetValue(hashState, out IState state);
    //    state.ChangeDataValue(val);
    //}

    //public IState GetState(IState state)
    //{
    //    hashStates.TryGetValue(state, out var outState);
    //    print($"StateAgent: Attempted to get state: {outState}.");
    //    return outState;
    //}

}

//[field: Serializable]
//public interface IState
//{
//    public StateAgent agent { get; set; }
//    public void Use(object obj, Inventory inv, Action callback = null);
//    public void ChangeDataValue(object val);
//    public object GetDataValue();
//}

//[Serializable]
//public class CanPlace : State<CanPlace.Data>, IState
//{
//    Place _functionality = new Place();
//    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

//    [Serializable]
//    public class Data : Uses
//    {
//        public bool canPlace;
//        public Transform placeLocation;
//        public GameObject objectToPlace;
//        public void SetCanPlace(bool val) => canPlace = val;
//        public void SetPlaceLocation(Transform val) => placeLocation = val;
//        public void SetObjectToPlace(GameObject val) => objectToPlace = val;

//    }

//}


//[Serializable]
//public class CanSpray : State<CanSpray.Data>, IState
//{
//    Spray _functionality = new Spray();
//    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

//    [Serializable]
//    public class Data : RequiredItem
//    {
//        public bool canSpray;
//        public IDestination sprayDestination;
//        public void SetCanSpray(bool val) => canSpray = val;
//        public void SetSprayLocation(IDestination val) => sprayDestination = val;
//    }

//}


//[Serializable]
//public class CanUnlock : State<CanUnlock.Data>, IState
//{
//    Unlock _functionality = new Unlock();
//    [field: SerializeField] public override UseFunctionality<Data> functionality { get => _functionality; }

//    [Serializable]
//    public class Data : RequiredItem
//    {
//        public HashSet<Item> keys;
//        public IDestination doorDestination;
//        public void SetDoorDestination(IDestination val) => doorDestination = val;
//    }

//}


