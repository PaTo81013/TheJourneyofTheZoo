namespace Character.Models
{
    public class CharacterDataDummy : ICharacterData
    {
        public float MoveSpeed => 5f;
        public float JumpForce => 3f;
        public StyleNameData StyleName => new StyleNameData("", "", 0);
    }
}