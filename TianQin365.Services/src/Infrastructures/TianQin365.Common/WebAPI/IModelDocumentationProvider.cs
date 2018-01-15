using System;
using System.Reflection;

namespace TianQin365.Common.WebAPI
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}
