using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Abstractions;
using BookStore.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Users.UpdateUser
{
    internal class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound);
            }

            user.Update(
                new FirstName(request.FirstName),
                new LastName(request.LastName));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
