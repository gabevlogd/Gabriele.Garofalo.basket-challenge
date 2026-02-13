using UnityEngine;

namespace BasketChallenge.Core
{
    [CreateAssetMenu(fileName = "HUD", menuName = "HUDs/HUD", order = 0)]
    public class HUDClass : ScriptableObject
    {
        public string hudName = "HUD";
        
        public HUD hudPrefab;
        
        public virtual HUD CreateHUD()
        {
            return CreateHUD<HUD>();
        }
        
        protected T CreateHUD<T>() where T : HUD
        {
            HUDFactory<T> factory = new HUDFactory<T>();
            T newHud = factory.CreateHUD(hudName);
            return newHud;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(hudName);
        }
    }
}