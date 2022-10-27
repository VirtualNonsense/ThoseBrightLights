using ThoseBrightLights.Core;

namespace ThoseBrightLights.Models
{
    
    /// <summary>
    /// contains everything to know about the screen size
    /// for screen width on a specific layer use camera
    /// </summary>
    public interface IScreen
    {
        int ScreenHeight { get; }
        int ScreenWidth { get; }
        
        Camera Camera { get; }
    }
}