using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.User.Common;
using MediatR;

namespace Bacheton.Application.User.Queries.TopUsers;

public class TopUsersQueryHandler : IRequestHandler<TopUsersQuery, List<TopUserResult>>
{
    private readonly IUserRepository _userRepository;

    public TopUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<TopUserResult>> Handle(TopUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetTopUsersAsync();
    }
}