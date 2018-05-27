// <copyright file="IWorkerUsernamesAppService.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using Abp.Application.Services;
    using AcmStatisticsAbp.SubmissionStatistics.Dto;

    public interface IWorkerUsernamesAppService : IAsyncCrudAppService<WorkerUsernamesDto, long>
    {
    }
}
