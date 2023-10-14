﻿using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos.Comment;
using FluentValidation;
using VidifyStream.Logic.Validators.Appendage;

namespace VidifyStream.Logic.Validators.Comments
{
    public class ReplyPostDTOValidator : AbstractValidator<ReplyPostDTO>
    {
        public ReplyPostDTOValidator(DataContext dataContext)
        {
            RuleFor(x => x.Text).NotEmpty().Length(0, 250);
            RuleFor(x => x.RepliedToId).Id().MustAsync(async (repliedToId, cancellation) =>
            {
                var repliedTo = await dataContext.Comments.FindAsync(repliedToId);
                return repliedTo != null;
            }).WithMessage("A comment with {PropertyValue} ID does not exist.");
        }
    }
}
