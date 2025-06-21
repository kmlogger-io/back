using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Login;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IDbCommit _dbCommit;

    public Handler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _dbCommit = dbCommit;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<Domain.Entities.User>(request);
        var isAuthenticated = await _userRepository.Authenticate(user, cancellationToken);
        
        if (!isAuthenticated || user.Notifications.Any())
            return new BaseResponse<Response>(403, "Invalid password or user", null, user.Notifications.ToList());

        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        var accessTokenExpiry = DateTime.UtcNow.AddHours(8);
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7); 
        
        user.AssignToken(accessToken);
        user.AssignRefreshToken(refreshToken, refreshTokenExpiry);
        
        await _userRepository.UpdateRefreshToken(user, cancellationToken);
        await _dbCommit.Commit(cancellationToken);          

        return new BaseResponse<Response>(200, "Login successful",
            new Response(accessToken, refreshToken, accessTokenExpiry, refreshTokenExpiry,
            new UserInfo(user.Id, user.FullName?.FirstName ?? "", user.Email.Address ?? ""))
        );
    }
}