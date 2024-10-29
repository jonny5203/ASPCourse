using ProjectMicroservices.Model.BaseModel;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.UnitOfWork.Interface;

public interface IUnitOfWork : IDisposable
{
    IActorRepository Actors { get; }
    IDirectorRepository Directors { get; }
    IGenreRepository Genres { get; }
    IMovieRepository Movies { get; }
    IReviewRepository Reviews { get; }
    void SaveChanges();
}