namespace SE_Praktikum.Models
{
    public interface IScreen
    {
        public enum Size
        {
            little = 100,
            medium = 720,
            big = 1000
        }

        int ScreenHeight { get; set; }
        int ScreenWidth { get; set; }
    }
}