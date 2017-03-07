namespace MyConcordiaID.Models.Event.Statistic
{
    /// <summary>
    /// statistic about the administration of the event 
    /// </summary>
    public class Administration
    {
        public int Creator = 1;
        public int Mods { get; set; }
        public int Scanners { get; set; }
    }
}
