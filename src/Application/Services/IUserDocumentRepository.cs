using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;

namespace Template.Application.Services
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
