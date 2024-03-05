using DMS.Domain.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Services
{
    public interface IUserDocumentRepository
    {
        Task<UserDocument?> GetByIdAsync(int id);
        Task<List<UserDocument>?> GetAllAsync();
        Task<UserDocument?> GetByDocumentIdAsync(int DocumentId);
        Task<UserDocument> SaveAsync(UserDocumentModel model);
        Task<UserDocument> CreateAsync(UserDocumentModel model);
        Task<UserDocument> UpdateAsync(UserDocumentModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
