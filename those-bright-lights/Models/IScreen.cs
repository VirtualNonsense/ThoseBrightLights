namespace SE_Praktikum.Models
{
    public enum Size
    {
        little = 100,
        medium = 720,
        big = 1000
    }

    public interface IScreen
    {
        int ScreenHeight { get; set; }
        int ScreenWidth { get; set; }

        void SetScreenFormat(Size previous);
    }
}