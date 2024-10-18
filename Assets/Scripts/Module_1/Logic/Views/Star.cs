namespace Constellation.Module_1.Logic
{
    public class Star : BaseConstellationView
    {
        public int Id { get; private set; }
        
        public bool IsInConstellation { get; set; }

        public void SetId(int id)
        {
            Id = id;
            gameObject.name = $"star_{id}";
        }
    }
}