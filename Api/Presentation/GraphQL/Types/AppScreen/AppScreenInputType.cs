using GraphQL.Types;

namespace Presentation.GraphQL.Types.AppScreen
{
    public class AppScreenInputType : InputObjectGraphType
    {
        public AppScreenInputType()
        {
            Field<IntGraphType>("AppScreenID").Description("AppScreen ID");
            Field<IntGraphType>("ParentAppScreenID").Description("Parent AppScreen ID");
            Field<StringGraphType>("ParentScreen").Description("Parent Screen Name");
            Field<StringGraphType>("Screen").Description("Screen Name");
            Field<StringGraphType>("Url").Description("Screen URL");
            Field<IntGraphType>("SortOrder").Description("Sort Order");
            Field<StringGraphType>("Icon").Description("Screen Icon");
            Field<IntGraphType>("UserID").Description("User ID");
            Field<BooleanGraphType>("Available").Description("Is Available");
        }
    }
}
