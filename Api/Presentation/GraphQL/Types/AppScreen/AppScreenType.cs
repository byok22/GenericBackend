using GraphQL.Types;
using Shared.Dtos;

namespace Presentation.GraphQL.Types.AppScreen
{
    public class AppScreenType : ObjectGraphType<AppScreenDto>
    {
        public AppScreenType()
        {
            Field(x => x.AppScreenID).Description("AppScreen ID");
            Field(x => x.ParentAppScreenID).Description("Parent AppScreen ID");
            Field(x => x.ParentScreen).Description("Parent Screen Name");
            Field(x => x.Screen).Description("Screen Name");
            Field(x => x.Url).Description("Screen URL");
            Field(x => x.SortOrder).Description("Sort Order");
            Field(x => x.Icon).Description("Screen Icon");
            Field(x => x.UserID).Description("User ID");
            Field(x => x.Available).Description("Is Available");
        }
    }
}
