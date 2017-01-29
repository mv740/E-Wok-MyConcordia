using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
