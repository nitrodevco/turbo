namespace Turbo.Core.Database.Dtos;

public class MoodLightPresetDto
{
    public int Id { get; set; }
    public int PresetId { get; set; }
    public int ItemId { get; set; }
    public int EffectType { get; set; }
    public int Brightness { get; set; }
    public string ColorHex { get; set; }
}
