namespace SE_Praktikum.Models
{
    public class AnimationSettings
    {
        public bool IsLooping;
        
        /// <summary>
        /// Determines how long it takes to update the frame. [sec] 
        /// </summary>
        public float UpdateInterval; 
        
        public AnimationSettings(bool isLooping = false, float updateInterval = 1f)
        {
            IsLooping = isLooping;
            UpdateInterval = updateInterval;
        }
    }
}