using NZWalk.API.Models.Domain;
using System.Net;

namespace NZWalk.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
