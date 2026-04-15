using BaseLib.Abstracts;
using TheValkyrie.TheValkyrieCode.Extensions;
using Godot;

namespace TheValkyrie.TheValkyrieCode.Character;

public class TheValkyriePotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheValkyrie.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}