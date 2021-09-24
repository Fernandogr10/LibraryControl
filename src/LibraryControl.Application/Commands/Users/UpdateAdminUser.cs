using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryControl.Application.Common.Interfaces.Repositories;
using LibraryControl.Domain.ValueObjects;
using MediatR;

namespace LibraryControl.Application.Commands.Users
{
    public static class UpdateAdminUser
    {
        public record Command(
            Guid Id,
            string Name,
            Email Email,
            string Password,
            bool Admin) : IRequest<Guid>;
        
        public class Handler : IRequestHandler<Command, Guid>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _repository.FindById(request.Id);

                if (user is null)
                    return Guid.Empty;
                
                user.AdminUpdate(request.Name, request.Email, request.Password, request.Admin);

                await _repository.Update(user);

                return user.Id;
            }
        }
    }
}
