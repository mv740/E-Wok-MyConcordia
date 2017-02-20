namespace MyConcordiaID.Models.Log
{
    public class Log
    {
        public enum Action
        {
            AddComment,
            SendPicture,
            ApprovePicture,
            DeniedPicture,
            ReApprovedPicture,
            CreatePictureUpdatePeriod,
            ModifiedPictureUpdatePeriod
        }
    }
}
