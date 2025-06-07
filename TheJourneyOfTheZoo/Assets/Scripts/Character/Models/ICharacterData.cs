namespace Character.Models
{
    public interface ICharacterData
    {
        float MoveSpeed { get; }
        float JumpForce { get; }
        StyleNameData StyleName { get; }
    }
}