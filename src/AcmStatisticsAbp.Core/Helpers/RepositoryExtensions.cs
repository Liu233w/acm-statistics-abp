// <copyright file="RepositoryExtensions.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Abp.Domain.Entities;
    using Abp.Domain.Repositories;
    using Abp.UI;
    using AcmStatisticsAbp.Exceptions;

    public static class RepositoryExtensions
    {
        /// <summary>
        /// 查找符合要求的Entity，找不到时抛出 <see cref="UserFriendlyException"/>
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPk">主键类型</typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        public static async Task<TEntity> FirstOrThrowAsync<TEntity, TPk>(this IRepository<TEntity, TPk> repository, TPk id)
            where TEntity : class, IEntity<TPk>
        {
            try
            {
                return await repository.GetAsync(id);
            }
            catch (EntityNotFoundException e)
            {
                var exception = new UserFriendlyException("未找到此ID", e)
                {
                    Code = StaticErrorCode.EntityNotFound,
                };
                throw exception;
            }
        }
    }
}
