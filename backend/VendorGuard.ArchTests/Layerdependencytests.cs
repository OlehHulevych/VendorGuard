// use ArchUnitNET.xUnitV3 if you installed the xUnit v3 package
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace VendorGuard.ArchTests;

/// <summary>
/// Enforces the Clean Architecture rules from the specification (section 10.1):
/// Domain -> nothing; Application -> Domain; Infrastructure -> Application, Domain; Api -> all.
/// </summary>
public class LayerDependencyTests
{
    // ---------- Assemblies ----------

    private static readonly System.Reflection.Assembly DomainAssembly = VendorGuard.Domain.AssemblyReference.Assembly;
    private static readonly System.Reflection.Assembly ApplicationAssembly = VendorGuard.Application.AssemblyReference.Assembly;
    private static readonly System.Reflection.Assembly InfrastructureAssembly = VendorGuard.Infrastructure.AssemblyReference.Assembly;
    private static readonly System.Reflection.Assembly ApiAssembly = VendorGuard.Api.AssemblyReference.Assembly;

    // External libraries that must not leak into inner layers.
    private static readonly System.Reflection.Assembly EfCoreAssembly = typeof(Microsoft.EntityFrameworkCore.DbContext).Assembly;
    private static readonly System.Reflection.Assembly NpgsqlAssembly = typeof(Npgsql.NpgsqlConnection).Assembly;
    private static readonly System.Reflection.Assembly GeminiAssembly = typeof(Google.GenAI.Client).Assembly;
    private static readonly System.Reflection.Assembly PdfPigAssembly = typeof(UglyToad.PdfPig.PdfDocument).Assembly;
    private static readonly System.Reflection.Assembly ClosedXmlAssembly = typeof(ClosedXML.Excel.XLWorkbook).Assembly;
    private static readonly System.Reflection.Assembly AspNetHttpAssembly = typeof(Microsoft.AspNetCore.Http.HttpContext).Assembly;
    private static readonly System.Reflection.Assembly AspNetMvcAssembly = typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly;

    // Load once: this is the expensive part.
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            DomainAssembly, ApplicationAssembly, InfrastructureAssembly, ApiAssembly,
            EfCoreAssembly, NpgsqlAssembly, GeminiAssembly, PdfPigAssembly, ClosedXmlAssembly,
            AspNetHttpAssembly, AspNetMvcAssembly)
        .Build();

    // ---------- Layers ----------

    private static readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInAssembly(DomainAssembly).As("Domain layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application layer");

    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure layer");

    private static readonly IObjectProvider<IType> ApiLayer =
        Types().That().ResideInAssembly(ApiAssembly).As("Api layer");

    // ---------- Forbidden external groups ----------

    private static readonly IObjectProvider<IType> PersistenceFrameworks =
        Types().That().ResideInAssembly(EfCoreAssembly, NpgsqlAssembly).As("EF Core / Npgsql");

    private static readonly IObjectProvider<IType> WebFrameworks =
        Types().That().ResideInAssembly(AspNetHttpAssembly, AspNetMvcAssembly).As("ASP.NET Core");

    private static readonly IObjectProvider<IType> ExternalProviders =
        Types().That().ResideInAssembly(GeminiAssembly, PdfPigAssembly, ClosedXmlAssembly)
            .As("External providers (Gemini, PdfPig, ClosedXML)");

    // ---------- Layer direction ----------

    [Fact]
    public void Domain_should_not_depend_on_other_layers()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(ApplicationLayer)
            .AndShould().NotDependOnAny(InfrastructureLayer)
            .AndShould().NotDependOnAny(ApiLayer)
            .Because("the Domain is the core and must not know about outer layers")
            .Check(Architecture);
    }

    [Fact]
    public void Application_should_not_depend_on_infrastructure_or_api()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .AndShould().NotDependOnAny(ApiLayer)
            .Because("Application defines interfaces; Infrastructure implements them")
            .Check(Architecture);
    }

    [Fact]
    public void Infrastructure_should_not_depend_on_api()
    {
        Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(ApiLayer)
            .Because("Infrastructure must be usable without the web host")
            .Check(Architecture);
    }

    // ---------- Frameworks and external providers ----------

    [Fact]
    public void Domain_should_not_depend_on_frameworks_or_providers()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(PersistenceFrameworks)
            .AndShould().NotDependOnAny(WebFrameworks)
            .AndShould().NotDependOnAny(ExternalProviders)
            .Because("business rules must stay pure and easy to unit-test")
            .Check(Architecture);
    }

    [Fact]
    public void Application_should_not_depend_on_web_or_external_providers()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(WebFrameworks)
            .AndShould().NotDependOnAny(ExternalProviders)
            .Because("no Gemini, PdfPig or ClosedXML types may leak outside Infrastructure (spec 6.1)")
            .Check(Architecture);
    }

    // ---------- API layer conventions ----------

    [Fact]
    public void Controllers_should_not_depend_on_infrastructure()
    {
        // Program.cs may reference Infrastructure for DI registration,
        // but controllers must go through MediatR, never DbContext or providers directly.
        Classes().That().Are(ApiLayer).And().HaveNameEndingWith("Controller").Should()
            .NotDependOnAny(InfrastructureLayer)
            .AndShould().NotDependOnAny(PersistenceFrameworks)
            .Because("controllers only translate HTTP to commands and queries")
            .Check(Architecture);
    }

    // ---------- Coding conventions ----------

    [Fact]
    public void Handlers_should_be_sealed()
    {
        Classes().That().Are(ApplicationLayer).And().HaveNameEndingWith("Handler").Should()
            .BeSealed()
            .Because("handlers are not designed for inheritance")
            .Check(Architecture);
    }
}