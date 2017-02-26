namespace MyConcordiaID.Models.Graduation
{
    public interface IGraduationRepository
    {
        GraduationStatus GetMarshallingCard(string netName);
        GraduationStatus GetMarshallingCardRequestDenied(string netName);
    }
}
