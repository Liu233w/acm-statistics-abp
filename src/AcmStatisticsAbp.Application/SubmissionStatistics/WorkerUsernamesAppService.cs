// <copyright file="WorkerUsernamesAppService.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using Abp.Authorization;
    using Abp.Domain.Repositories;
    using Abp.UI;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Exceptions;
    using AcmStatisticsAbp.Helpers;
    using AcmStatisticsAbp.SubmissionStatistics.Dto;

    [AbpAuthorize(PermissionNames.Pages_WorkerUsername)]
    public class WorkerUsernamesAppService : AsyncCrudAppService<WorkerUsernames, WorkerUsernamesDto, long,
        PagedAndSortedResultRequestDto, WorkerUsernamesDto>, IWorkerUsernamesAppService
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
            var entity = await this.CheckOwnerAndGetEntity(input.Id);

            entity.Title = input.Title;
            entity.Usernames = input.Usernames;

            return this.MapToEntityDto(entity);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var entity = await this.CheckOwnerAndGetEntity(input.Id);
            await this.Repository.DeleteAsync(entity);
        }

        public override async Task<WorkerUsernamesDto> Get(EntityDto<long> input)
        {
            var entity = await this.CheckOwnerAndGetEntity(input.Id);
            return this.MapToEntityDto(entity);
        }

        protected override IQueryable<WorkerUsernames> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            // 覆写这个方法就可以给 GetAll 添加权限控制了
            return this.Repository.GetAll().Where(item => item.UserId == this.AbpSession.UserId.Value);
        }

        /// <summary>
        /// 尝试获取实体。如果此实体不属于用户，将抛出异常。
        /// </summary>
        /// <param name="id"></param>
        private async Task<WorkerUsernames> CheckOwnerAndGetEntity(long id)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var userId = this.AbpSession.UserId.Value;

            var entity = await this.Repository.FirstOrThrowAsync(id);
            if (entity.UserId != userId)
            {
                var exception = new UserFriendlyException(StaticErrorCode.EntityNotFound, "未找到此ID");
                throw exception;
            }

            return entity;
        }
    }
}
