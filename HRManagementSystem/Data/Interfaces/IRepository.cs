using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HRManagementSystem.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    // 查询方法
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    // 添加方法
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    // 更新方法
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);

    // 删除方法
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);

    // 计数方法
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    // 是否存在
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}