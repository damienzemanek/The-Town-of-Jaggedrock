using Sirenix.OdinInspector;
using UnityEngine;

public class Favor : MonoBehaviour
{
    [HideInInspector] [SerializeField] int _favor;
    [ShowInInspector] public int favor
    {
        get => _favor;
        set
        {
            int clamped = Mathf.Clamp(value, -10, 10);
            _favor = clamped;
        }
    }

    public enum FavorStatus
    {
        Hated,
        Unliked,
        Neutral,
        Liked,
        Friend
    }

    [ShowInInspector] public FavorStatus status
    {
        get
        {
            int clampedFavor = Mathf.Clamp(favor, -10, 10);
            if (clampedFavor <= -6) return FavorStatus.Hated;
            if (clampedFavor <= -2) return FavorStatus.Unliked;
            if (clampedFavor <= 1) return FavorStatus.Neutral;
            if (clampedFavor <= 6) return FavorStatus.Liked;
            return FavorStatus.Friend;
        }
    }


}
