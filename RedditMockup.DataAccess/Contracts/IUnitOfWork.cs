using RedditMockup.DataAccess.Repositories;

namespace RedditMockup.DataAccess.Contracts;

public interface IUnitOfWork
{
    AnswerRepository? AnswerRepository { get; }

    PersonRepository? PersonRepository { get; }

    ProfileRepository? ProfileRepository { get; }

    QuestionRepository? QuestionRepository { get; }

    RoleRepository? RoleRepository { get; }

    UserRepository? UserRepository { get; }

    UserRoleRepository? UserRoleRepository { get; }

    QuestionVoteRepository? QuestionVoteRepository { get; }

    AnswerVoteRepository? AnswerVoteRepository { get; }

    int Commit();

    Task<int> CommitAsync(CancellationToken cancellationToken);
}