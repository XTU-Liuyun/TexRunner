using Microsoft.Xna.Framework;

namespace TexRunner.Entities
{
    public interface IDayNightCycle
    {
        int NightCount { get; }
        bool IsNight { get; }

        Color ClearColor { get; }
    }
}
