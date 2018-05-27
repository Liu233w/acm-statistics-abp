// <copyright file="SignalRFeature.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SignalR
{
    public static class SignalRFeature
    {
        public static bool IsAvailable
        {
            get
            {
#if FEATURE_SIGNALR
                return true;
#elif FEATURE_SIGNALR_ASPNETCORE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsAspNetCore
        {
            get
            {
#if FEATURE_SIGNALR_ASPNETCORE
                return true;
#else
                return false;
#endif
            }
        }
    }
}
