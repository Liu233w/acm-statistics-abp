// <copyright file="WorkerUsernamesAppService.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using System.Threading.Tasks;
    using Abp.Application.Services;
    using Abp.Authorization;
    using Abp.Domain.Repositories;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Helpers;
    using AcmStatisticsAbp.SubmissionStatistics.Dto;

    [AbpAuthorize(PermissionNames.Pages_WorkerUsername)]
    public class WorkerUsernamesAppService : AsyncCrudAppService<WorkerUsernames, WorkerUsernamesDto, long>, IWorkerUsernamesAppService
    {
        public WorkerUsernamesAppService(IRepository<WorkerUsernames, long> repository)
            : base(repository)
        {
        }

        public override Task<WorkerUsernamesDto> Create(WorkerUsernamesDto input)
        {
            // ReSharper disable once PossibleInvalidOperationException
            input.UserId = this.AbpSession.UserId.Value;
            return base.Create(input);
        }

        public override async Task<WorkerUsernamesDto> Update(WorkerUsernamesDto input)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var userId = this.AbpSession.UserId.Value;

            var entity = await this.Repository.FirstOrThrowAsync(input.Id);
            if (entity.UserId != userId)
            {
                throw new AbpAuthorizationException("不能修改其他用户的数据");
            }

            entity.Title = input.Title;
            entity.Usernames = input.Usernames;

            return this.MapToEntityDto(entity);
        }
    }
}
