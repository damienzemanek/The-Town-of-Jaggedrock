using DependencyInjection;
using UnityEngine;
using static LocationRandomizer;

public class LadyInBlack : RuntimeInjectableMonoBehaviour
{

#region Privates
    [Inject] TimeCycle time;
    [Inject] Despawner despawner;
    LocationRandomizer locations;
#endregion

#region Class Methods
    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        locations = this.TryGet<LocationRandomizer>();
    }


    private void OnEnable()
    {
        if (WontShowUpAtDayAndIsDay()) return;
    }
#endregion


#region Methods
    bool WontShowUpAtDayAndIsDay()
    {
        if (time.IsDay())
        {
            despawner.DisableNPC(gameObject);
            return true;
        }
        return false;
    }

 #endregion

}
